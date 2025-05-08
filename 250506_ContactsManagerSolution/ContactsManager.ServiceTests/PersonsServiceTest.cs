using System.Linq.Expressions;
using AutoFixture;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContract;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using ContactsManager.Infrastructure.DbContext;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using Xunit.Abstractions;

namespace ContactsManager.ServiceTests;

public class PersonsServiceTest
{
    private readonly IPersonsAdderService _personsAdderService;
    private readonly IPersonsDeleterService _personsDeleterService;
    private readonly IPersonsGetterService _personsGetterService;
    private readonly IPersonsSorterService _personsSorterService;
    private readonly IPersonsUpdaterService _personsUpdaterService;

    private readonly Mock<IPersonsRepository> _personsRepositoryMock;

    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IFixture _fixture;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        // Moq创建模拟对象
        _personsRepositoryMock = new Mock<IPersonsRepository>();
        // 获取模拟对象的实例
        var personsRepository = _personsRepositoryMock.Object;

        _personsAdderService = new PersonsAdderService(
            personsRepository,
            new Mock<ILogger<PersonsAdderService>>().Object,
            new Mock<IDiagnosticContext>().Object
        );
        _personsDeleterService = new PersonsDeleterService(
            personsRepository,
            new Mock<ILogger<PersonsDeleterService>>().Object,
            new Mock<IDiagnosticContext>().Object
        );
        _personsGetterService = new PersonsGetterService(
            personsRepository,
            new Mock<ILogger<PersonsGetterService>>().Object,
            new Mock<IDiagnosticContext>().Object
        );
        _personsSorterService = new PersonsSorterService(
            personsRepository,
            new Mock<ILogger<PersonsSorterService>>().Object,
            new Mock<IDiagnosticContext>().Object
        );
        _personsUpdaterService = new PersonsUpdaterService(
            personsRepository,
            new Mock<ILogger<PersonsUpdaterService>>().Object,
            new Mock<IDiagnosticContext>().Object
        );
        _testOutputHelper = testOutputHelper;
        _fixture = new Fixture();
    }

    #region AddPerson

    //right request -> return personResponse(包含 person guid)
    [Fact]
    public async Task AP_FullPersonAddRequest_ToBeSuccessful()
    {
        //fixture Creat()使用默认参数自动生成 AttributeName{NewGuid} -> 某些格式不正确
        //使用Builder来自定义
        var personAddRequest = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .Create();

        var expectedPerson = personAddRequest.ToPerson();
        //如果提供相同的参数 - 返回的值应该相同
        //SetUp...It.Any<>.. - 定义当 IPersonsRepository 的 AddPerson 方法被调用时的行为，无论传入的 Person 对象参数是什么
        //.ReturnsAsync(person)指定该方法调用将异步返回一个 Task<Person>，其结果为 person（在测试中预先定义的 Person 对象）。
        _personsRepositoryMock
            .Setup(p => p.AddPerson(It.IsAny<Person>()))
            .ReturnsAsync(expectedPerson);
        var expectedPersonResponse = expectedPerson.ToPersonResponse();

        var actualPersonResponse = await _personsAdderService.AddPerson(personAddRequest);

        //模拟的 IPersonsRepository 返回的 expectedPerson 对象可能未正确生成 PersonId（例如使用 Guid.Empty 或固定值）。
        expectedPersonResponse.PersonId = actualPersonResponse.PersonId;

        //不应该调用其他的方法 一个方法一个测试
        // var allPersonResponsesGuid = (await _personsService.GetAllPersons())
        //     .Select(temp => temp.PersonId)
        //     .ToList();

        actualPersonResponse.PersonId.Should().NotBe(Guid.Empty);
        actualPersonResponse.Should().Be(expectedPersonResponse);
    }

    //null request -> throw argumentNullException
    [Fact]
    public async Task AP_NullPersonAddResponse_ToBeArgumentNullException()
    {
        PersonAddRequest? request = null;
        var action = async () => await _personsAdderService.AddPerson(request);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    //null request -> throw argumentNullException
    [Fact]
    public async Task AP_NullPersonName_ToBeArgumentException()
    {
        //arrange
        var personAddRequest = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.PersonName, null as string)
            .Create();

        var expectedPerson = personAddRequest.ToPerson();
        _personsRepositoryMock
            .Setup(p => p.AddPerson(It.IsAny<Person>()))
            .ReturnsAsync(expectedPerson);

        //act
        var action = async () => await _personsAdderService.AddPerson(personAddRequest);

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    #endregion

    #region GetPersonByPersonId

    //null personId -> null personResponse
    [Fact]
    public async Task GPBPI_NullPersonId_ToBeNull()
    {
        //arrange
        Guid? personId = null;
        //act
        var actualResponse = await _personsGetterService.GetPersonByPersonId(personId);
        //assert
        actualResponse.Should().BeNull();
    }

    //error personId -> argumentNullException
    [Fact]
    public async Task GPBPI_InvalidPersonId()
    {
        //arrange
        Guid? personId = Guid.NewGuid();
        //act
        var actualResponse = await _personsGetterService.GetPersonByPersonId(personId);
        //assert
        actualResponse.Should().BeNull();
    }

    //right personId -> return personResponse
    [Fact]
    public async Task GPBPI_ValidPersonId_ToBeSuccessful()
    {
        var person = _fixture
            .Build<Person>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.Country, null as Country)
            .Create();

        var expectedPersonResponse = person.ToPersonResponse();
        // 模拟存储库的 GetPersonByPersonId 方法返回该 person 对象
        _personsRepositoryMock
            .Setup(p => p.GetPersonByPersonId(person.PersonId))
            .ReturnsAsync(person);

        // 调用服务层的 GetPersonByPersonId 方法
        var actualPersonResponse = await _personsGetterService.GetPersonByPersonId(person.PersonId);

        actualPersonResponse.Should().Be(expectedPersonResponse);
    }

    #endregion

    #region GetAllPersons
    // return Empty or default
    [Fact]
    public async Task GAP_ServiceNullList_ToBeEmpty()
    {
        _personsRepositoryMock.Setup(p => p.GetAllPersons()).ReturnsAsync([]);
        var emptyGet = await _personsGetterService.GetAllPersons();
        emptyGet.Should().BeEmpty();
    }

    [Fact]
    public async Task GAP_ServiceFewPersons_ToBeSuccessful()
    {
        var personList = new List<Person>()
        {
            _fixture
                .Build<Person>()
                .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
                .With(p => p.Country, null as Country)
                .Create(),
            _fixture
                .Build<Person>()
                .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
                .With(p => p.Country, null as Country)
                .Create(),
            _fixture
                .Build<Person>()
                .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
                .With(p => p.Country, null as Country)
                .Create(),
        };

        var expectedPersonResponse = personList.Select(p => p.ToPersonResponse()).ToList();

        _personsRepositoryMock.Setup(p => p.GetAllPersons()).ReturnsAsync(personList);

        var actualPersonResponse = await _personsGetterService.GetAllPersons();

        //打印输出
        _testOutputHelper.WriteLine("预期输出 开始");
        foreach (var personAddRequest in expectedPersonResponse)
        {
            _testOutputHelper.WriteLine(personAddRequest.ToString());
        }
        _testOutputHelper.WriteLine("预期输出 结束");

        //打印输出
        _testOutputHelper.WriteLine("实际输出 开始");
        foreach (var personResponse in actualPersonResponse)
        {
            _testOutputHelper.WriteLine(personResponse.ToString());
        }
        _testOutputHelper.WriteLine("实际输出 结束");

        actualPersonResponse.Should().NotBeNull();
        //判断等价BeEquivalentTo
        actualPersonResponse.Should().BeEquivalentTo(expectedPersonResponse);
    }
    #endregion

    #region GetFilteredPerson
    //searchTest is "PersonName" or empty -> return allPersonResponse
    [Fact]
    public async Task GFP_EmptySearchText_TobeSuccessful()
    {
        var personList = new List<Person>()
        {
            _fixture
                .Build<Person>()
                .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
                .With(p => p.Country, null as Country)
                .Create(),
            _fixture
                .Build<Person>()
                .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
                .With(p => p.Country, null as Country)
                .Create(),
            _fixture
                .Build<Person>()
                .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
                .With(p => p.Country, null as Country)
                .Create(),
        };

        var expectedPersonResponse = personList.Select(p => p.ToPersonResponse()).ToList();

        //测试关注的是服务层对 GetFilteredPersons 的调用是否正确（如参数传递、业务逻辑处理），而非数据层的实际查询能力。通过模拟返回值，可以专注于服务层的逻辑验证。
        _personsRepositoryMock
            .Setup(p => p.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
            .ReturnsAsync(personList);

        var targetPersonResponse = await _personsGetterService.GetFilteredPersons(
            nameof(Person.PersonName),
            ""
        );

        targetPersonResponse.Should().BeEquivalentTo(expectedPersonResponse);
    }

    //searchTest is "PersonName" or empty -> return allPersonResponse
    [Fact]
    public async Task GFP_SearchPersonName_ToBeSuccessful()
    {
        var personList = new List<Person>()
        {
            _fixture
                .Build<Person>()
                .With(p => p.PersonName, $"Filter_Index1")
                .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
                .With(p => p.Country, null as Country)
                .Create(),
            _fixture
                .Build<Person>()
                .With(p => p.PersonName, $"Filter_Index2")
                .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
                .With(p => p.Country, null as Country)
                .Create(),
            _fixture
                .Build<Person>()
                .With(p => p.PersonName, $"Filter_Index3")
                .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
                .With(p => p.Country, null as Country)
                .Create(),
        };

        var testText = "Index";

        var expectedPersonResponse = personList.Select(p => p.ToPersonResponse()).ToList();

        _personsRepositoryMock
            .Setup(p => p.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
            .ReturnsAsync(personList);

        var actualPersonResponse = await _personsGetterService.GetFilteredPersons(
            nameof(Person.PersonName),
            testText
        );

        _testOutputHelper.WriteLine("预期输出");
        foreach (var personResponse in expectedPersonResponse)
        {
            _testOutputHelper.WriteLine(personResponse.PersonName);
        }

        _testOutputHelper.WriteLine("实际输出");
        foreach (var personResponse in actualPersonResponse)
        {
            _testOutputHelper.WriteLine(personResponse.PersonName);
        }

        //每一个getFilteredPerson中personResponse都包含测试文字
        actualPersonResponse.Should().BeEquivalentTo(expectedPersonResponse);
    }
    #endregion

    #region GetSortesPersons
    //DESC 排序 -> 排序完成的list
    [Fact]
    public async Task GSP_SortByPersonName_ToBeSuccessful()
    {
        var personList = new List<Person>()
        {
            _fixture
                .Build<Person>()
                .With(p => p.PersonName, $"Sort_Index3")
                .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
                .With(p => p.Country, null as Country)
                .Create(),
            _fixture
                .Build<Person>()
                .With(p => p.PersonName, $"Sort_Index2")
                .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
                .With(p => p.Country, null as Country)
                .Create(),
            _fixture
                .Build<Person>()
                .With(p => p.PersonName, $"Sort_Index1")
                .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
                .With(p => p.Country, null as Country)
                .Create(),
        };

        var personResponse = personList.Select(p => p.ToPersonResponse()).ToList();

        var actualPersonResponses = await _personsSorterService.GetSortPersons(
            personResponse,
            nameof(Person.PersonName),
            SortOrderOptions.DESC
        );

        //测试排序是否正确
        actualPersonResponses.Should().BeInDescendingOrder(temp => temp.PersonName);
    }

    #endregion

    #region UpdatePerson

    //null personUpdateRequest -> throw ArgumentNullException
    [Fact]
    public async Task UP_NullUpdateRequest_ToBeArgumentNullException()
    {
        PersonUpdateRequest? personUpdateRequest = null;
        var action = async () => await _personsUpdaterService.UpdatePerson(personUpdateRequest);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    //invalid personUpdateRequest.PersonId -> throw ArgumentException
    [Fact]
    public async Task UP_InvalidPersonId_ToBeArgumentException()
    {
        var personUpdateRequest = _fixture.Build<PersonUpdateRequest>().Create();
        var action = async () => await _personsUpdaterService.UpdatePerson(personUpdateRequest);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    //null personUpdateRequest.PersonName -> throw ArgumentException
    [Fact]
    public async Task UP_NullPersonName_ToBeArgumentException()
    {
        var person = _fixture
            .Build<Person>()
            .With(p => p.PersonName, null as string)
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.Gender, GenderOptions.Other.ToString())
            .With(p => p.Country, null as Country)
            .Create();

        var personResponse = person.ToPersonResponse();
        var updatePersonRequest = personResponse.ToPersonUpdateRequest();
        var action = async () => await _personsUpdaterService.UpdatePerson(updatePersonRequest);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    //add right person - change person name and person email --> update person response
    [Fact]
    public async Task UP_ValidPersonInfo_ToBeSuccessful()
    {
        var person = _fixture
            .Build<Person>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.Gender, GenderOptions.Other.ToString())
            .With(p => p.Country, null as Country)
            .Create();

        var expectedPersonResponse = person.ToPersonResponse();

        _personsRepositoryMock
            .Setup(p => p.GetPersonByPersonId(person.PersonId))
            .ReturnsAsync(person);
        _personsRepositoryMock.Setup(p => p.UpdatePerson(person)).ReturnsAsync(person);

        var updatePersonRequest = expectedPersonResponse.ToPersonUpdateRequest();
        var actualPersonResponse = await _personsUpdaterService.UpdatePerson(updatePersonRequest);

        actualPersonResponse.Should().Be(expectedPersonResponse);
    }
    #endregion

    #region DeletePerson

    //null personId --> throw ArgumentNullException
    [Fact]
    public async Task DP_NullPersonId_ToBeArgumentNullException()
    {
        Guid? guid = null;
        var action = async () => await _personsDeleterService.DeletePerson(guid);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    //invalid personId --> return false
    [Fact]
    public async Task DP_InvalidPersonId_ToBeFalse()
    {
        var person = _fixture
            .Build<Person>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.Country, null as Country)
            .Create();

        _personsRepositoryMock
            .Setup(p => p.GetPersonByPersonId(person.PersonId))
            .ReturnsAsync(person);

        var result = await _personsDeleterService.DeletePerson(Guid.NewGuid());
        result.Should().BeFalse();
    }

    //valid personId --> return true
    [Fact]
    public async Task DP_ValidPersonId_ToBeTrue()
    {
        var person = _fixture
            .Build<Person>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.Country, null as Country)
            .Create();

        var personResponse = person.ToPersonResponse();

        _personsRepositoryMock
            .Setup(p => p.GetPersonByPersonId(person.PersonId))
            .ReturnsAsync(person);
        _personsRepositoryMock
            .Setup(p => p.DeletePersonByPersonId(person.PersonId))
            .ReturnsAsync(true);

        var result = await _personsDeleterService.DeletePerson(personResponse.PersonId);
        result.Should().BeTrue();
    }

    #endregion
}
