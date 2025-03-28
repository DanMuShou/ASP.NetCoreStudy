using Microsoft.AspNetCore.Mvc;

namespace _250326_rider_EnvironmentsExample.Controllers;

[Controller]
public class HomeController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public HomeController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    [Route("/")]
    [Route("/home")]
    public IActionResult Home()
    {
        ViewBag.CurrentEnv = _webHostEnvironment.EnvironmentName;
        return View();
    }

    [Route("/home")]
    public IActionResult Home2()
    {
        return View();
    }
}
