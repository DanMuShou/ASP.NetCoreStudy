using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
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
    public async Task<IActionResult> Home(
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

        var filterPersonResponseList = await _personsService.GetFilteredPersons(
            searchBy,
            searchString
        );

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentSearchString = searchString;

        //Sort
        var filterSortPersonResponseList = await _personsService.GetSortPersons(
            filterPersonResponseList,
            sortBy,
            sortOrder
        );
        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder.ToString();

        return View(filterSortPersonResponseList); //View/Person/Home.cshtml
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        //类表示 SelectList 或 MultiSelectList 中的一个项。这个类通常在 HTML 中呈现为 <option> 元素，并带有指定的属性值
        // var selectListItem = new SelectListItem() { Text = "aa", Value = "1" };
        //<option value="1"> aa </option>

        var countryResponseList = await _countriesService.GetAllCountries();
        ViewBag.Countries = countryResponseList.Select(temp => new SelectListItem()
        {
            Text = temp.CountryName,
            Value = temp.CountryId.ToString(),
        });

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
    {
        //验证模型是否正确填写
        if (!ModelState.IsValid)
        {
            var allCountryResponseList = await _countriesService.GetAllCountries();
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

        var personResponse = await _personsService.AddPerson(personAddRequest);
        return RedirectToAction("Home", "Person");
    }

    [HttpGet("{personId:guid}")]
    public async Task<IActionResult> Edit(Guid personId)
    {
        var targetPersonResponse = await _personsService.GetPersonByPersonId(personId);
        if (targetPersonResponse == null)
        {
            return RedirectToAction("Home", "Person");
        }

        var personUpdateRequest = targetPersonResponse.ToPersonUpdateRequest();

        var countryResponseList = await _countriesService.GetAllCountries();
        ViewBag.Countries = countryResponseList.Select(temp => new SelectListItem()
        {
            Text = temp.CountryName,
            Value = temp.CountryId.ToString(),
        });

        return View(personUpdateRequest);
    }

    [HttpPost("{personId:guid}")]
    public async Task<IActionResult> Edit(Guid personId, PersonUpdateRequest personUpdateRequest)
    {
        var targetPersonResponse = await _personsService.GetPersonByPersonId(personId);
        if (targetPersonResponse == null)
        {
            return RedirectToAction("Home", "Person");
        }

        //验证模型是否正确填写
        if (ModelState.IsValid)
        {
            // var updatePersonResponse = await _personsService.UpdatePerson(personUpdateRequest);
            return RedirectToAction("Home", "Person");
        }
        else
        {
            // var countryResponseList = await _countriesService.GetAllCountries();
            ViewBag.Errors = ModelState
                .Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return View();
        }
    }

    [HttpGet("{personId:guid}")]
    public async Task<IActionResult> Delete(Guid? personId)
    {
        var deletePersonResponse = await _personsService.GetPersonByPersonId(personId);
        if (deletePersonResponse == null)
        {
            return RedirectToAction("Home", "Person");
        }

        return View(deletePersonResponse);
    }

    [HttpPost("{personId:guid}")]
    public async Task<IActionResult> Delete(Guid personId, PersonUpdateRequest personUpdateRequest)
    {
        var deletePersonResponse = await _personsService.GetPersonByPersonId(personId);
        if (deletePersonResponse == null)
        {
            return RedirectToAction("Home", "Person");
        }

        await _personsService.DeletePerson(personId);
        return RedirectToAction("Home", "Person");
    }

    [HttpGet]
    public async Task<IActionResult> PersonsPdf()
    {
        var allPersonList = await _personsService.GetAllPersons();
        return new ViewAsPdf("PersonsPdf", allPersonList, ViewData)
        {
            PageMargins = new Margins(20, 20, 20, 20),
            PageOrientation = Orientation.Landscape,
        };
    }

    [HttpGet]
    public async Task<IActionResult> PersonsCsv()
    {
        var memoryStream = await _personsService.GetPersonCsv();
        //指定MIME类型为二进制流 表示文件内容为未知类型 强制浏览器以下载方式处理（而非直接打开）
        return File(memoryStream, "application/octet-stream", "Person.csv");
    }

    [HttpGet]
    public async Task<IActionResult> PersonsExcel()
    {
        var memoryStream = await _personsService.GetPersonExcel();
        return File(
            memoryStream,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Person.xlsx"
        );
    }
}
