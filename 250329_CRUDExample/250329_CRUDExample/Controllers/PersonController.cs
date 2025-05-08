using _250329_CRUDExample.Filters;
using _250329_CRUDExample.Filters.ActionFilters;
using _250329_CRUDExample.Filters.AuthorizationFilter;
using _250329_CRUDExample.Filters.ExceptionFilter;
using _250329_CRUDExample.Filters.ResourceFilter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;

namespace _250329_CRUDExample.Controllers;

[Route("[controller]/[action]")]
// [ResponseHeaderActionFilter("My-Key-Controller1", "My-Value-Controller1", 3)]
[ResponseHeaderFilterFactory("My-Key-Controller1", "My-Value-Controller1", 3)]
[TypeFilter<HandleExceptionFilter>]
[TypeFilter<PersonAlwaysRunResultFilter>]
public class PersonController(
    IPersonsAdderService personsAdderService,
    IPersonsDeleterService personsDeleterService,
    IPersonsGetterService personsGetterService,
    IPersonsSorterService personsSorterService,
    IPersonsUpdaterService personsUpdaterService,
    ICountriesService countriesService
) : Controller
{
    [HttpGet]
    [Route("/")]
    [ServiceFilter(typeof(PersonListActionFilter), Order = 4)]
    // [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = ["My-Key-Action1", "My-Value-Action1", 4],Order = 4 // 显式设置 Order)]
    // [ResponseHeaderActionFilter("My-Key-Action1", "My-Value-Action1", 4)]
    [ResponseHeaderFilterFactory("My-Key-Controller1", "My-Value-Controller1", 3)]
    [TypeFilter(typeof(PersonListResultFilter))]
    [SkipFilter]
    public async Task<IActionResult> Home(
        string searchBy,
        string? searchString,
        string sortBy = nameof(PersonResponse.PersonName),
        SortOrderOptions sortOrder = SortOrderOptions.ASC
    )
    {
        var filterPersonResponseList = await personsGetterService.GetFilteredPersons(
            searchBy,
            searchString
        );

        //Sort
        var filterSortPersonResponseList = await personsSorterService.GetSortPersons(
            filterPersonResponseList,
            sortBy,
            sortOrder
        );

        // ViewData 的赋值在Filter里面完成

        return View(filterSortPersonResponseList); //View/Person/Home.cshtml
    }

    [HttpGet]
    // [ResponseHeaderActionFilter("My-Key-Action2", "My-Value-Action2", 4)]
    public async Task<IActionResult> Create()
    {
        //类表示 SelectList 或 MultiSelectList 中的一个项。这个类通常在 HTML 中呈现为 <option> 元素，并带有指定的属性值
        // var selectListItem = new SelectListItem() { Text = "aa", Value = "1" };
        //<option value="1"> aa </option>

        var countryResponseList = await countriesService.GetAllCountries();
        ViewBag.Countries = countryResponseList.Select(temp => new SelectListItem()
        {
            Text = temp.CountryName,
            Value = temp.CountryId.ToString(),
        });

        return View();
    }

    [HttpPost]
    [TypeFilter(typeof(PersonCreatAndEditPostActionFilter))]
    [TypeFilter(typeof(FeatureDisableResourceFilter), Arguments = [false])]
    public async Task<IActionResult> Create(PersonAddRequest personRequest)
    {
        await personsAdderService.AddPerson(personRequest);
        return RedirectToAction("Home", "Person");
    }

    [HttpGet("{personId:guid}")]
    [TypeFilter<TokenResultFilter>]
    public async Task<IActionResult> Edit(Guid personId)
    {
        var targetPersonResponse = await personsGetterService.GetPersonByPersonId(personId);
        if (targetPersonResponse == null)
        {
            return RedirectToAction("Home", "Person");
        }

        var personUpdateRequest = targetPersonResponse.ToPersonUpdateRequest();

        var countryResponseList = await countriesService.GetAllCountries();
        ViewBag.Countries = countryResponseList.Select(temp => new SelectListItem()
        {
            Text = temp.CountryName,
            Value = temp.CountryId.ToString(),
        });

        return View(personUpdateRequest);
    }

    [HttpPost("{personId:guid}")]
    [TypeFilter<TokenAuthorizationFilter>]
    [TypeFilter<PersonCreatAndEditPostActionFilter>]
    public async Task<IActionResult> Edit(Guid personId, PersonUpdateRequest personRequest)
    {
        var targetPersonResponse = await personsGetterService.GetPersonByPersonId(personId);
        if (targetPersonResponse == null)
        {
            return RedirectToAction("Home", "Person");
        }

        await personsUpdaterService.UpdatePerson(personRequest);
        return RedirectToAction("Home", "Person");
    }

    [HttpGet("{personId:guid}")]
    public async Task<IActionResult> Delete(Guid? personId)
    {
        var deletePersonResponse = await personsGetterService.GetPersonByPersonId(personId);
        if (deletePersonResponse == null)
        {
            return RedirectToAction("Home", "Person");
        }

        return View(deletePersonResponse);
    }

    [HttpPost("{personId:guid}")]
    public async Task<IActionResult> Delete(Guid personId, PersonUpdateRequest personUpdateRequest)
    {
        var deletePersonResponse = await personsGetterService.GetPersonByPersonId(personId);
        if (deletePersonResponse == null)
        {
            return RedirectToAction("Home", "Person");
        }

        await personsDeleterService.DeletePerson(personId);
        return RedirectToAction("Home", "Person");
    }

    [HttpGet]
    public async Task<IActionResult> PersonsPdf()
    {
        var allPersonList = await personsGetterService.GetAllPersons();
        return new ViewAsPdf("PersonsPdf", allPersonList, ViewData)
        {
            PageMargins = new Margins(20, 20, 20, 20),
            PageOrientation = Orientation.Landscape,
        };
    }

    [HttpGet]
    public async Task<IActionResult> PersonsCsv()
    {
        var memoryStream = await personsGetterService.GetPersonCsv();
        //指定MIME类型为二进制流 表示文件内容为未知类型 强制浏览器以下载方式处理（而非直接打开）
        return File(memoryStream, "application/octet-stream", "Person.csv");
    }

    [HttpGet]
    public async Task<IActionResult> PersonsExcel()
    {
        var memoryStream = await personsGetterService.GetPersonExcel();
        return File(
            memoryStream,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Person.xlsx"
        );
    }
}
