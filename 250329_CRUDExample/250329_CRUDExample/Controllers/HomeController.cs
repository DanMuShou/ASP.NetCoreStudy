using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace _250329_CRUDExample.Controllers;

public class HomeController : Controller
{
    [Route("[action]")]
    public IActionResult Error()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature != null)
        {
            ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;
        }
        return View();
    }
}
