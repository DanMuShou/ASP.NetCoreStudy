using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagerSolution.Controllers;

public class HomeController : Controller
{
    [AllowAnonymous]
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
