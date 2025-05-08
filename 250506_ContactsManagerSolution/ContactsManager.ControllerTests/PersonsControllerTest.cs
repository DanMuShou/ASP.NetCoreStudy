using AutoFixture;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using ContactsManager.Core.ServiceContracts;
using ContactsManagerSolution.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ContactsManager.ControllerTests;

public class PersonsControllerTest
{
    private readonly IPersonsAdderService _personsAdderService;
    private readonly IPersonsDeleterService _personsDeleterService;
    private readonly IPersonsGetterService _personsGetterService;
    private readonly IPersonsSorterService _personsSorterService;
    private readonly IPersonsUpdaterService _personsUpdaterService;

    private Mock<IPersonsAdderService> _personsAdderServiceMock;
    private Mock<IPersonsDeleterService> _personsDeleterServiceMock;
    private Mock<IPersonsGetterService> _personsGetterServiceMock;
    private Mock<IPersonsSorterService> _personsSorterServiceMock;
    private Mock<IPersonsUpdaterService> _personsUpdaterServiceMock;

    private readonly ICountriesAdderService _countriesAdderService;
    private readonly ICountriesGetterService _countriesGetterService;
    private readonly ICountriesUploaderService _countriesUploaderService;

    private Mock<ICountriesAdderService> _countriesAdderServiceMock;
    private Mock<ICountriesGetterService> _countriesGetterServiceMock;
    private Mock<ICountriesUploaderService> _countriesUploaderServiceMock;

    private readonly Fixture _fixture;

    public PersonsControllerTest()
    {
        _personsAdderServiceMock = new Mock<IPersonsAdderService>();
        _personsDeleterServiceMock = new Mock<IPersonsDeleterService>();
        _personsGetterServiceMock = new Mock<IPersonsGetterService>();
        _personsSorterServiceMock = new Mock<IPersonsSorterService>();
        _personsUpdaterServiceMock = new Mock<IPersonsUpdaterService>();

        _personsAdderService = _personsAdderServiceMock.Object;
        _personsDeleterService = _personsDeleterServiceMock.Object;
        _personsGetterService = _personsGetterServiceMock.Object;
        _personsSorterService = _personsSorterServiceMock.Object;
        _personsUpdaterService = _personsUpdaterServiceMock.Object;

        _countriesAdderServiceMock = new Mock<ICountriesAdderService>();
        _countriesGetterServiceMock = new Mock<ICountriesGetterService>();
        _countriesUploaderServiceMock = new Mock<ICountriesUploaderService>();

        _countriesAdderService = _countriesAdderServiceMock.Object;
        _countriesGetterService = _countriesGetterServiceMock.Object;
        _countriesUploaderService = _countriesUploaderServiceMock.Object;


        _fixture = new Fixture();
    }

    #region Home

    [Fact]
    public async Task H_ShouldReturnHomeViewWithPersonList()
    {
        var personResponse = _fixture.Create<List<PersonResponse>>();
        var personController = new PersonController(
            _personsAdderService,
            _personsDeleterService,
            _personsGetterService,
            _personsSorterService,
            _personsUpdaterService,
            _countriesGetterService
        );

        _personsGetterServiceMock
            .Setup(p => p.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(personResponse);
        _personsSorterServiceMock
            .Setup(p =>
                p.GetSortPersons(personResponse, It.IsAny<string>(), It.IsAny<SortOrderOptions>())
            )
            .ReturnsAsync(personResponse);

        var result = await personController.Home(
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            _fixture.Create<SortOrderOptions>()
        );

        var viewResult = Assert.IsType<ViewResult>(result);
        viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
        viewResult.ViewData.Model.Should().Be(personResponse);
    }

    #endregion

    #region Create

    [Fact]
    public async Task C_HasModelError_ToReturnCreateView()
    {
        var personAddRequest = _fixture.Create<PersonAddRequest>();
        var personResponse = personAddRequest.ToPerson().ToPersonResponse();

        var countryList = _fixture.Create<List<CountryResponse>>();

        _countriesGetterServiceMock.Setup(c => c.GetAllCountries()).ReturnsAsync(countryList);
        _personsAdderServiceMock
            .Setup(p => p.AddPerson(personAddRequest))
            .ReturnsAsync(personResponse);

        var personController = new PersonController(
            _personsAdderService,
            _personsDeleterService,
            _personsGetterService,
            _personsSorterService,
            _personsUpdaterService,
            _countriesGetterService
        );

        personController.ModelState.AddModelError("测试错误", "Error------");

        var result = await personController.Create(personAddRequest);

        var viewResult = Assert.IsType<ViewResult>(result);
        viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
        viewResult.ViewData.Model.Should().Be(personAddRequest);
    }

    [Fact]
    public async Task C_NoModelError_ToReturnCreateView()
    {
        var personAddRequest = _fixture.Create<PersonAddRequest>();
        var personResponse = personAddRequest.ToPerson().ToPersonResponse();

        var countryList = _fixture.Create<List<CountryResponse>>();

        _countriesGetterServiceMock.Setup(c => c.GetAllCountries()).ReturnsAsync(countryList);
        _personsAdderServiceMock
            .Setup(p => p.AddPerson(personAddRequest))
            .ReturnsAsync(personResponse);

        var personController = new PersonController(
            _personsAdderService,
            _personsDeleterService,
            _personsGetterService,
            _personsSorterService,
            _personsUpdaterService,
            _countriesGetterService
        );

        var result = await personController.Create(personAddRequest);
        //重定向结果
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        viewResult.ActionName.Should().Be("Home");
        viewResult.ControllerName.Should().Be("Person");
    }

    #endregion
}