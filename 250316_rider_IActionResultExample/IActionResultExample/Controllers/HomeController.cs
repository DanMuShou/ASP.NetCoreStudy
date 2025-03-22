using IActionResultExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers;

[Controller]
public class HomeController : Controller
{
    [Route("bookstore/{bookId?}/{isLoggedIn?}")]
    public IActionResult Index([FromRoute]int? bookId, [FromRoute]bool? isLoggedIn, Book book)
    {
        //id不存在
        // if (!Request.Query.TryGetValue("bookid",out var bookIdStr))
        //     return BadRequest("bookId is not supplied");
        //bookId为空|bookId为空字符串
        // if(string.IsNullOrEmpty(bookIdStr))
        //     return BadRequest("bookId can't be null or empty");
        // bookId为0
        // if (bookId <= 0)
        //     return BadRequest("bookId can't be less 0");
        //bookId为 1-99 之间
        // var bookId = Convert.ToInt16(bookIdStr);
        //未登录报错
        // if (Convert.ToBoolean(Request.Query["isloggedin"]) == false)
        // {
        //     return StatusCode(401, "You must be logged in");
        // }    // return Unauthorized("You must be logged in");
        // return File("sample.jpg", "application/jpg");
        //重定向 - 302 Found
        // return new RedirectToActionResult("Books", "Store",new{});
        //301
        // return new RedirectToActionResult("Books", "Store", new { }, true);
        // return RedirectPermanent("https://www.baidu.com");
        // return LocalRedirect("/store/books/" + bookId);
        // return new LocalRedirectResult($"/store/books/{bookId}");
        // return RedirectToAction("Books", "Store", new { id = bookId  });
        // return RedirectToActionPermanent("Books", "Store", new { id = bookId });
        
        if(bookId.HasValue == false)
            return BadRequest("bookId is not supplied");
        if(bookId is < 0 or > 100)
            return NotFound("bookId must be between 1 and 99");
        if (!(isLoggedIn ?? false))
            return StatusCode(401, "You must be logged in");
        return Content($"Book id: {bookId}, Book: {book}");
    }

    [Route("register")]
    public IActionResult Register(Person person)
    {
        if (ModelState.IsValid == false)
        {
            var errors = string.Join("\n", 
                ModelState.Values.
                    SelectMany(value => value.Errors).
                    Select(err => err.ErrorMessage));
            return BadRequest($"person not invalid\n {errors}");
        }
        
        return Content($"{person}");
    }
}