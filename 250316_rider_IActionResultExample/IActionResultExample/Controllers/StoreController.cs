using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers;

[Controller]
public class StoreController:Controller
{
    [Route("store/books/{id}")]
    public IActionResult Books()
    {
        var id = Convert.ToInt32(Request.RouteValues["id"]);
        return Content($"<h1>Book Store, BookID = {id}</h1>", "text/html");
    }
}