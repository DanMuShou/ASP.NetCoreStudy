using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;

namespace _250329_CRUDExample.Controllers;

[Route("[controller]")]
public class PersonController : Controller
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;

    public PersonController(IPersonsService personsService, ICountriesService countriesService)
    {
        _personsService = personsService;
        _countriesService = countriesService;
    }

    [HttpGet("/")]
    public IActionResult Home(
        string searchBy,
        string? searchString,
        string sortBy = nameof(PersonResponse.PersonName),
        SortOrderOptions sortOrder = SortOrderOptions.ASC
    )
    {
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            { nameof(PersonResponse.PersonName), "人员名称" },
            { nameof(PersonResponse.Email), "邮箱" },
            { nameof(PersonResponse.DateOfBirth), "出生日期" },
            { nameof(PersonResponse.Gender), "性别" },
            { nameof(PersonResponse.CountryName), "国家名称" },
            { nameof(PersonResponse.Address), "地址" },
        };

        var filterPersonResponseList = _personsService.GetFilteredPersons(searchBy, searchString);

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        //Sort
        var filterSortPersonResponseList = _personsService.GetSortPersons(
            filterPersonResponseList,
            sortBy,
            sortOrder
        );
        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder.ToString();

        return View(filterSortPersonResponseList); //View/Person/Home.cshtml
    }

    [HttpGet("[action]")]
    public IActionResult Create()
    {
        var countryResponseList = _countriesService.GetAllCountries();
        ViewBag.CountryResponses = countryResponseList;
        return View();
    }

    [HttpPost("[action]")]
    public IActionResult Create(PersonAddRequest personAddRequest)
    {
        //验证模型是否正确填写
        if (!ModelState.IsValid)
        {
            var allCountryResponseList = _countriesService.GetAllCountries();
            ViewBag.CountryResponses = allCountryResponseList;
            ViewBag.Errors = ModelState
                .Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return View();
        }

        var personResponse = _personsService.AddPerson(personAddRequest);
        return RedirectToAction("Home", "Person");
    }
}
