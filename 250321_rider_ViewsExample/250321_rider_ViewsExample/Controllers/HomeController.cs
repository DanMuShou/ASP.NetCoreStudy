using _250321_rider_ViewsExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace _250321_rider_ViewsExample.Controllers;

[Controller]
public class HomeController : Controller
{
    [Route("home")]
    [Route("/")]
    public ActionResult Home()
    {
        ViewData["pageTitle"] = "Asp.Net Core Demo App";

        List<Person> persons =
        [
            new Person()
            {
                Name = "Rider",
                DateOfBirth = Convert.ToDateTime("1980-01-01"),
                PersonGender = Gender.Female,
            },
            new Person()
            {
                Name = "Vs",
                DateOfBirth = Convert.ToDateTime("1980-01-01"),
                PersonGender = Gender.Female,
            },
            new Person()
            {
                Name = "VsCode",
                DateOfBirth = null,
                PersonGender = Gender.Female,
            },
        ];

        ViewData["persons"] = persons;
        // return View();
        // return new ViewResult() { ViewName = "Home" };
        // return View(); //Views/Home/Home.cshtml
        return View(); //Views/Home/Home.cshtml
    }
}
