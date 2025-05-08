using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagerSolution.Controllers;

[Route("[controller]/[action]")]
public class CountriesController(ICountriesUploaderService countriesUploaderService) : Controller
{
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

        var countriesInsertedCount = await countriesUploaderService.UploadCountriesFromExcelFile(
            excelFile
        );
        ViewBag.Message = $"{countriesInsertedCount} 个国家已导入";
        return View();
    }
}
