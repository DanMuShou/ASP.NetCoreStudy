using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;

namespace _250329_CRUDExample.Controllers;

[Route("[controller]/[action]")]
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
    [HttpGet]
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

    [HttpGet]
    public IActionResult Create()
    {
        //类表示 SelectList 或 MultiSelectList 中的一个项。这个类通常在 HTML 中呈现为 <option> 元素，并带有指定的属性值
        // var selectListItem = new SelectListItem() { Text = "aa", Value = "1" };
        //<option value="1"> aa </option>

        var countryResponseList = _countriesService.GetAllCountries();
        ViewBag.Countries = countryResponseList.Select(temp => new SelectListItem()
        {
            Text = temp.CountryName,
            Value = temp.CountryId.ToString(),
        });

        return View();
    }

    [HttpPost]
    public IActionResult Create(PersonAddRequest personAddRequest)
    {
        //验证模型是否正确填写
        if (!ModelState.IsValid)
        {
            var allCountryResponseList = _countriesService.GetAllCountries();
            ViewBag.Countries = allCountryResponseList.Select(temp => new SelectListItem()
            {
                Text = temp.CountryName,
                Value = temp.CountryId.ToString(),
            });
            ViewBag.Errors = ModelState
                .Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return View();
        }

        var personResponse = _personsService.AddPerson(personAddRequest);
        return RedirectToAction("Home", "Person");
    }

    [HttpGet("{personId:guid}")]
    public IActionResult Edit(Guid personId)
    {
        var targetPersonResponse = _personsService.GetPersonByPersonId(personId);
        if (targetPersonResponse == null)
        {
            return RedirectToAction("Home", "Person");
        }

        var personUpdateRequest = targetPersonResponse.ToPersonUpdateRequest();

        var countryResponseList = _countriesService.GetAllCountries();
        ViewBag.Countries = countryResponseList.Select(temp => new SelectListItem()
        {
            Text = temp.CountryName,
            Value = temp.CountryId.ToString(),
        });

        return View(personUpdateRequest);
    }

    [HttpPost("{personId:guid}")]
    public IActionResult Edit(Guid personId, PersonUpdateRequest personUpdateRequest)
    {
        var targetPersonResponse = _personsService.GetPersonByPersonId(personId);
        if (targetPersonResponse == null)
        {
            return RedirectToAction("Home", "Person");
        }

        //验证模型是否正确填写
        if (ModelState.IsValid)
        {
            var updatePersonResponse = _personsService.UpdatePerson(personUpdateRequest);
            return RedirectToAction("Home", "Person");
        }
        else
        {
            var countryResponseList = _countriesService.GetAllCountries();
            ViewBag.Errors = ModelState
                .Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return View();
        }
    }

    [HttpGet("{personId:guid}")]
    public IActionResult Delete(Guid? personId)
    {
        var deletePersonResponse = _personsService.GetPersonByPersonId(personId);
        if (deletePersonResponse == null)
        {
            return RedirectToAction("Home", "Person");
        }

        return View(deletePersonResponse);
    }

    [HttpPost("{personId:guid}")]
    public IActionResult Delete(Guid personId, PersonUpdateRequest personUpdateRequest)
    {
        var deletePersonResponse = _personsService.GetPersonByPersonId(personId);
        if (deletePersonResponse == null)
        {
            return RedirectToAction("Home", "Person");
        }

        _personsService.DeletePerson(personId);
        return RedirectToAction("Home", "Person");
    }
}
