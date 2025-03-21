using Microsoft.AspNetCore.Mvc;

namespace _250321_rider_ViewsExample.Controllers;

[Controller]
public class HomeController : Controller
{
    [Route("home")]
    [Route("/")]
    public ActionResult Home()
    {
        // return View();
        // return new ViewResult() { ViewName = "Home" };
        // return View(); //Views/Home/Home.cshtml
        return View(); //Views/Home/Home.cshtml
    }
}