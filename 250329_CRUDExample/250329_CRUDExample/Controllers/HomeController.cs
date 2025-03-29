using Microsoft.AspNetCore.Mvc;

namespace _250329_CRUDExample.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    [Route("/home")]
    public IActionResult Home()
    {
        return View();
    }
}