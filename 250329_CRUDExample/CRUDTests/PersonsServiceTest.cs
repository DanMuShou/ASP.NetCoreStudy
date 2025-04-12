using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;
using Services;
using Xunit.Abstractions;

namespace CRUDTests;

public class PersonsServiceTest
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;

    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IFixture _fixture;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        var dbContextMock = new DbContextMock<ApplicationDbContext>(
            new DbContextOptionsBuilder<ApplicationDbContext>().Options
        );

        var dbContext = dbContextMock.Object;
        dbContextMock.CreateDbSetMock(db => db.Countries, []);
        dbContextMock.CreateDbSetMock(db => db.Persons, []);

        _countriesService = new CountriesService(dbContext);
        _personsService = new PersonsService(dbContext, _countriesService);

        _testOutputHelper = testOutputHelper;
        _fixture = new Fixture();
    }

    #region AddPerson
    //null request -> throw argumentNullException
    [Fact]
    public async Task AddPerson_PersonNameNull()
    {
        //arrange
        var personAddRequest = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.PersonName, null as string)
            .Create();

        //act
        var action = async () => await _personsService.AddPerson(personAddRequest);

        //Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    //null request -> throw argumentNullException
    [Fact]
    public async Task AddPerson_NullPerson()
    {
        //arrange
        PersonAddRequest? request = null;
        //act
        var action = async () => await _personsService.AddPerson(request);
        //assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    //right request -> return personResponse(包含 person guid)
    [Fact]
    public async Task AddPerson_RightRequest()
    {
        //arrange
        //fixture Creat()使用默认参数自动生成 AttributeName{NewGuid} -> 某些格式不正确
        //使用Builder来自定义
        var personAddRequest = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .Create();
        //act
        var response = await _personsService.AddPerson(personAddRequest);
        var allPersonResponsesGuid = (await _personsService.GetAllPersons())
            .Select(temp => temp.PersonId)
            .ToList();

        response.PersonId.Should().NotBe(Guid.Empty);
        allPersonResponsesGuid.Should().Contain(response.PersonId);
    }
    #endregion

    #region GetPersonByPersonId

    //null personId -> null personResponse
    [Fact]
    public async Task GetPersonByPersonId_NullPersonId()
    {
        //arrange
        Guid? personId = null;
        //act
        var actualResponse = await _personsService.GetPersonByPersonId(personId);
        //assert
        actualResponse.Should().BeNull();
    }

    //error personId -> argumentNullException
    [Fact]
    public async Task GetPersonByPersonId_ErrorPersonId()
    {
        //arrange
        Guid? personId = Guid.NewGuid();
        //act
        var actualResponse = await _personsService.GetPersonByPersonId(personId);
        //assert
        actualResponse.Should().BeNull();
    }

    //right personId -> return personResponse
    [Fact]
    public async Task GetPersonByPersonId_RightPersonId()
    {
        var personAddRequest = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .Create();

        var personResponse = await _personsService.AddPerson(personAddRequest);
        var getPersonResponse = await _personsService.GetPersonByPersonId(personResponse.PersonId);

        getPersonResponse.Should().Be(personResponse);
    }

    #endregion

    #region GetAllPersons
    // return Empty or default
    [Fact]
    public async Task GetAllPersons_WithEmptyPersonService()
    {
        //arrange
        var emptyGet = await _personsService.GetAllPersons();
        //act
        emptyGet.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllPersons_Default()
    {
        var countryAddRequest1 = _fixture.Create<CountryAddRequest>();
        var countryAddRequest2 = _fixture.Create<CountryAddRequest>();
        var countryAddRequest3 = _fixture.Create<CountryAddRequest>();

        var countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
        var countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);
        var countryResponse3 = await _countriesService.AddCountry(countryAddRequest3);

        var personAddRequest1 = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse1.CountryId)
            .Create();

        var personAddRequest2 = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse2.CountryId)
            .Create();
        var personAddRequest3 = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse3.CountryId)
            .Create();

        var personAddRequestList = new List<PersonAddRequest>()
        {
            personAddRequest1,
            personAddRequest2,
            personAddRequest3,
        };

        var personResponseList = new List<PersonResponse>();
        foreach (var personAddRequest in personAddRequestList)
        {
            var personResponse = await _personsService.AddPerson(personAddRequest);
            personResponseList.Add(personResponse);
        }

        var getAllPersonResponseList = await _personsService.GetAllPersons();

        //打印输出
        _testOutputHelper.WriteLine("预期输出 开始");
        foreach (var personAddRequest in personAddRequestList)
        {
            _testOutputHelper.WriteLine(personAddRequest.ToString());
        }
        _testOutputHelper.WriteLine("预期输出 结束");

        //打印输出
        _testOutputHelper.WriteLine("实际输出 开始");
        foreach (var personResponse in getAllPersonResponseList)
        {
            _testOutputHelper.WriteLine(personResponse.ToString());
        }
        _testOutputHelper.WriteLine("实际输出 结束");

        getAllPersonResponseList.Should().NotBeNull();
        //判断等价BeEquivalentTo
        getAllPersonResponseList.Should().BeEquivalentTo(personResponseList);
    }
    #endregion

    #region GetFilteredPerson
    //searchTest is "PersonName" or empty -> return allPersonResponse
    [Fact]
    public async Task GetFilteredPerson_Default()
    {
        var countryAddRequest1 = _fixture.Create<CountryAddRequest>();
        var countryAddRequest2 = _fixture.Create<CountryAddRequest>();
        var countryAddRequest3 = _fixture.Create<CountryAddRequest>();

        var countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
        var countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);
        var countryResponse3 = await _countriesService.AddCountry(countryAddRequest3);

        var personAddRequest1 = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse1.CountryId)
            .Create();

        var personAddRequest2 = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse2.CountryId)
            .Create();
        var personAddRequest3 = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse3.CountryId)
            .Create();

        var personAddRequestList = new List<PersonAddRequest>()
        {
            personAddRequest1,
            personAddRequest2,
            personAddRequest3,
        };

        var personResponseList = new List<PersonResponse>();
        foreach (var personAddRequest in personAddRequestList)
        {
            var personResponse = await _personsService.AddPerson(personAddRequest);
            personResponseList.Add(personResponse);
        }

        var getFilteredPersonList = await _personsService.GetFilteredPersons(
            nameof(Person.PersonName),
            ""
        );

        getFilteredPersonList.Should().BeEquivalentTo(personResponseList);
    }

    //searchTest is "PersonName" or empty -> return allPersonResponse
    [Fact]
    public async Task GetFilteredPerson_SearchByPersonName()
    {
        var countryAddRequest1 = _fixture.Create<CountryAddRequest>();
        var countryAddRequest2 = _fixture.Create<CountryAddRequest>();
        var countryAddRequest3 = _fixture.Create<CountryAddRequest>();

        var countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
        var countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);
        var countryResponse3 = await _countriesService.AddCountry(countryAddRequest3);

        var personAddRequest1 = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.PersonName, $"SearchString-Test1{Guid.NewGuid()}")
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse1.CountryId)
            .Create();

        var personAddRequest2 = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.PersonName, $"SearchString-Test2{Guid.NewGuid()}")
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse2.CountryId)
            .Create();
        var personAddRequest3 = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.PersonName, $"SearchString-Nothing{Guid.NewGuid()}")
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse3.CountryId)
            .Create();

        var personAddRequestList = new List<PersonAddRequest>()
        {
            personAddRequest1,
            personAddRequest2,
            personAddRequest3,
        };

        var personResponseList = new List<PersonResponse>();
        foreach (var personAddRequest in personAddRequestList)
        {
            var personResponse = await _personsService.AddPerson(personAddRequest);
            personResponseList.Add(personResponse);
        }

        var personResponseFilteredList = personResponseList
            .Where(personResponse =>
                personResponse.PersonName?.Contains("Test", StringComparison.OrdinalIgnoreCase)
                == true
            )
            .ToList();

        var getFilteredPerson = (
            await _personsService.GetFilteredPersons(nameof(Person.PersonName), "Test")
        ).ToList();

        _testOutputHelper.WriteLine("预期输出");
        foreach (var personResponse in personResponseFilteredList)
        {
            _testOutputHelper.WriteLine(personResponse.PersonName);
        }

        _testOutputHelper.WriteLine("实际输出");
        foreach (var personResponse in getFilteredPerson)
        {
            _testOutputHelper.WriteLine(personResponse.PersonName);
        }

        //每一个getFilteredPerson中personResponse都包含测试文字
        getFilteredPerson
            .Should()
            .OnlyContain(temp =>
                temp.PersonName != null
                && temp.PersonName.Contains("Test", StringComparison.OrdinalIgnoreCase)
            );
        getFilteredPerson.Should().BeEquivalentTo(personResponseFilteredList);
    }
    #endregion

    #region GetSortesPersons
    //DESC 排序 -> 排序完成的list
    [Fact]
    public async Task GetSortPersons_SortByPersonName()
    {
        var countryAddRequest1 = _fixture.Create<CountryAddRequest>();
        var countryAddRequest2 = _fixture.Create<CountryAddRequest>();
        var countryAddRequest3 = _fixture.Create<CountryAddRequest>();

        var countryResponse1 = await _countriesService.AddCountry(countryAddRequest1);
        var countryResponse2 = await _countriesService.AddCountry(countryAddRequest2);
        var countryResponse3 = await _countriesService.AddCountry(countryAddRequest3);

        var personAddRequest1 = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.PersonName, $"OrderIndex-3{Guid.NewGuid()}")
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse1.CountryId)
            .Create();
        var personAddRequest2 = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.PersonName, $"OrderIndex-2{Guid.NewGuid()}")
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse2.CountryId)
            .Create();
        var personAddRequest3 = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.PersonName, $"OrderIndex-1{Guid.NewGuid()}")
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse3.CountryId)
            .Create();

        var personAddRequestList = new List<PersonAddRequest>()
        {
            personAddRequest1,
            personAddRequest2,
            personAddRequest3,
        };

        foreach (var personAddRequest in personAddRequestList)
        {
            await _personsService.AddPerson(personAddRequest);
        }

        var allPersonResponseList = await _personsService.GetAllPersons();

        var getSortPersonResponses = await _personsService.GetSortPersons(
            allPersonResponseList,
            nameof(Person.PersonName),
            SortOrderOptions.DESC
        );

        //测试排序是否正确
        getSortPersonResponses.Should().BeInDescendingOrder(temp => temp.PersonName);
    }

    #endregion

    #region UpdatePerson

    //null personUpdateRequest -> throw ArgumentNullException
    [Fact]
    public async Task UpdatePerson_NullUpdateRequest()
    {
        PersonUpdateRequest? personUpdateRequest = null;

        var action = async () => await _personsService.UpdatePerson(personUpdateRequest);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    //invalid personUpdateRequest.PersonId -> throw ArgumentException
    [Fact]
    public async Task UpdatePerson_UpdateRequestWithInvalidPersonId()
    {
        var personUpdateRequest = _fixture.Build<PersonUpdateRequest>().Create();
        var action = async () => await _personsService.UpdatePerson(personUpdateRequest);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    //null personUpdateRequest.PersonName -> throw ArgumentException
    [Fact]
    public async Task UpdatePerson_UpdateRequestWithNullPersonName()
    {
        var countryAddRequest = _fixture.Create<CountryAddRequest>();
        var countryResponse = await _countriesService.AddCountry(countryAddRequest);

        var personAddRequest = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.PersonName, $"OrderIndex-3{Guid.NewGuid()}")
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse.CountryId)
            .Create();

        var personResponse = await _personsService.AddPerson(personAddRequest);

        var personUpdateRequest = personResponse.ToPersonUpdateRequest();

        personUpdateRequest.PersonName = null;

        var action = async () => await _personsService.UpdatePerson(personUpdateRequest);
        await action.Should().ThrowAsync<ArgumentException>();
    }

    //add right person - change person name and person email --> update person response
    [Fact]
    public async Task UpdatePerson_UpdateRequestWithValidPersonNameAndPersonEmail()
    {
        var countryAddRequest = _fixture.Create<CountryAddRequest>();
        var countryResponse = await _countriesService.AddCountry(countryAddRequest);

        var personAddRequest = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse.CountryId)
            .Create();
        var personResponse = await _personsService.AddPerson(personAddRequest);

        var personUpdateRequest = personResponse.ToPersonUpdateRequest();
        personUpdateRequest.PersonName = "changed text1";
        personUpdateRequest.Email = "changed_test1@gmail.com";

        var updatedPersonResponse = await _personsService.UpdatePerson(personUpdateRequest);
        var inquirePersonResponse = await _personsService.GetPersonByPersonId(
            personResponse.PersonId
        );

        inquirePersonResponse.Should().Be(updatedPersonResponse);
    }
    #endregion

    #region DeletePerson

    //invalid personId --> return false
    [Fact]
    public async Task DeletePerson_InvalidPersonId()
    {
        var countryAddRequest = _fixture.Create<CountryAddRequest>();
        var countryResponse = await _countriesService.AddCountry(countryAddRequest);

        var personAddRequest = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse.CountryId)
            .Create();
        await _personsService.AddPerson(personAddRequest);

        var result = await _personsService.DeletePerson(Guid.NewGuid());
        result.Should().BeFalse();
    }

    //valid personId --> return true
    [Fact]
    public async Task DeletePerson_ValidPersonId()
    {
        var countryAddRequest = _fixture.Create<CountryAddRequest>();
        var countryResponse = await _countriesService.AddCountry(countryAddRequest);

        var personAddRequest = _fixture
            .Build<PersonAddRequest>()
            .With(p => p.Email, $"Email {Guid.NewGuid()}@test.com")
            .With(p => p.CountryId, countryResponse.CountryId)
            .Create();
        var personResponse = await _personsService.AddPerson(personAddRequest);

        var result = await _personsService.DeletePerson(personResponse.PersonId);
        result.Should().BeTrue();
    }

    //null personId --> throw ArgumentNullException
    [Fact]
    public async Task DeletePerson_NullPersonId()
    {
        Guid? guid = null;
        var action = async () => await _personsService.DeletePerson(guid);
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    #endregion
}
