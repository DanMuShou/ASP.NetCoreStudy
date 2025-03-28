using Microsoft.AspNetCore.Mvc;

namespace _250322_rider_LayoutViewsExample.Controllers;

[Controller]
public class HomeController : Controller
{
    [Route("/")]
    [Route("home")]
    public IActionResult Home()
    {
        return View();
    }
    
    [Route("aboutCompany")]
    public IActionResult AboutCompany()
    {
        return View();
    }
    
    [Route("contactSupport")]
    public IActionResult ContactSupport()
    {
        return View();
    }
}