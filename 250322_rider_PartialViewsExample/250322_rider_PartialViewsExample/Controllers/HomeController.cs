using _250322_rider_PartialViewsExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace _250322_rider_PartialViewsExample.Controllers;

[Controller]
public class HomeController : Controller
{
    [Route("/")]
    [Route("home")]
    public IActionResult Home()
    {
        return View();
    }

    [Route("about")]
    public IActionResult About()
    {
        ViewData["ListTitle"] = "Cities";
        ViewData["ListItems"] = new List<string>() { "北京", "上海", "广州", "深圳", "重庆" };
        return View();
    }

    [Route("programming-languages")]
    public IActionResult ProgrammingLanguages()
    {
        var listModel = new ListModel()
        {
            ListTitle = "Programming Languages List",
            ListItems =
            [
                "C#",
                "Java",
                "Python",
                "JavaScript",
                "Ruby",
                "Swift",
                "Kotlin",
                "Go",
                "Rust",
            ],
        };

        return PartialView("_ListPartialView", listModel);
    }
}
