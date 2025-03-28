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

        // ViewBag.persons = persons;
        // return View();
        // return new ViewResult() { ViewName = "Home" };
        // return View(); //Views/Home/Home.cshtml
        return View("Home", persons); //Views/Home/Home.cshtml
    }

    [Route("personDetails/{name}")]
    public IActionResult Details(string? name)
    {
        if (name == null)
            return NotFound("Person name can't be null");

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

        var targetPerson = persons.FirstOrDefault(temp => temp.Name == name);
        if (targetPerson == null)
            return NotFound($"Person with name {name} not found");

        return View("PersonDetails", targetPerson);
    }

    [Route("personWithProduct")]
    public IActionResult PersonWithProduct()
    {
        var person = new Person()
        {
            Name = "Rider",
            PersonGender = Gender.Female,
            DateOfBirth = Convert.ToDateTime("2000-1-1"),
        };
        var product = new Product() { ProductId = 1, ProductName = "RiderProduct" };
        var personAndProductWrapperModel = new PersonAndProductWrapperModel()
        {
            Person = person,
            Product = product,
        };
        return View(personAndProductWrapperModel);
    }

    [Route("home/allProduct")]
    public IActionResult All()
    {
        //Views/Home/All.cshtml
        //Views/Shared/All.cshtml
        return View();
    }
}
