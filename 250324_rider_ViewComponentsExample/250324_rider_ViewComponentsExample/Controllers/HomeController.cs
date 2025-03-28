using _250324_rider_ViewComponentsExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace _250324_rider_ViewComponentsExample.Controllers;

[Controller]
public class HomeController : Controller
{
    [Route("/")]
    [Route("/home")]
    public IActionResult Home()
    {
        return View();
    }

    [Route("/about")]
    public IActionResult About()
    {
        return View();
    }

    [Route("friendList")]
    public IActionResult LoadFriendList()
    {
        var personGridModel = new PersonGridModel()
        {
            GridTitle = "朋友列表",
            Persons =
            [
                new Person() { Name = "张三", JobTitle = "UI" },
                new Person() { Name = "李四", JobTitle = "Game" },
                new Person() { Name = "王五", JobTitle = "Lader" },
            ],
        };

        return ViewComponent("Grid", new { geidModel = personGridModel });
    }
}
