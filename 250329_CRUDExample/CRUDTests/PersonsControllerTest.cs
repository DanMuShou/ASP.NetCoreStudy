using _250329_CRUDExample.Controllers;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;
using Services;

namespace CRUDTests;

public class PersonsControllerTest
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;

    private readonly Mock<IPersonsService> _personsServiceMock;
    private readonly Mock<ICountriesService> _countriesServiceMock;

    private readonly Fixture _fixture;

    public PersonsControllerTest()
    {
        _personsServiceMock = new Mock<IPersonsService>();
        _countriesServiceMock = new Mock<ICountriesService>();

        _personsService = _personsServiceMock.Object;
        _countriesService = _countriesServiceMock.Object;

        _fixture = new Fixture();
    }

    #region Home

    [Fact]
    public async Task H_ShouldReturnHomeViewWithPersonList()
    {
        var personResponse = _fixture.Create<List<PersonResponse>>();
        var personController = new PersonController(_personsService, _countriesService);

        _personsServiceMock
            .Setup(p => p.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(personResponse);
        _personsServiceMock
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

        _countriesServiceMock.Setup(c => c.GetAllCountries()).ReturnsAsync(countryList);
        _personsServiceMock.Setup(p => p.AddPerson(personAddRequest)).ReturnsAsync(personResponse);

        var personController = new PersonController(_personsService, _countriesService);

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

        _countriesServiceMock.Setup(c => c.GetAllCountries()).ReturnsAsync(countryList);
        _personsServiceMock.Setup(p => p.AddPerson(personAddRequest)).ReturnsAsync(personResponse);

        var personController = new PersonController(_personsService, _countriesService);

        var result = await personController.Create(personAddRequest);
        //重定向结果
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        viewResult.ActionName.Should().Be("Home");
        viewResult.ControllerName.Should().Be("Person");
    }
    #endregion
}
