using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace _250329_CRUDExample.Controllers;

[Route("[controller]/[action]")]
public class CountriesController : Controller
{
    private readonly ICountriesService _countriesService;

    public CountriesController(ICountriesService countriesService)
    {
        _countriesService = countriesService;
    }

    [HttpGet]
    public IActionResult UploadFromExcel()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadFromExcel(IFormFile? excelFile)
    {
        if (excelFile == null || excelFile.Length == 0)
        {
            ViewBag.ErrorMessage = "error1 : 请选择 .xlsx 文件";
            return View();
        }

        if (
            !Path.GetExtension(excelFile.FileName)
                .Equals(".xlsx", StringComparison.CurrentCultureIgnoreCase)
        )
        {
            ViewBag.ErrorMessage = "error2 : 请选择 .xlsx 文件";
            return View();
        }

        var countriesInsertedCount = await _countriesService.UploadCountriesFromExcelFile(
            excelFile
        );
        ViewBag.Message = $"{countriesInsertedCount} 个国家已导入";
        return View();
    }
}
