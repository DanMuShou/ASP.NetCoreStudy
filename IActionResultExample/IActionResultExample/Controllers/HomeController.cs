using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers;

[Controller]
public class HomeController : Controller
{
    [Route("bookstore")]
    public IActionResult Index()
    {
        //id不存在
        if (!Request.Query.TryGetValue("bookid",out var bookIdStr))
            return BadRequest("bookId is not supplied");
      
        //bookId为空|bookId为空字符串
        if(string.IsNullOrEmpty(bookIdStr))
            return BadRequest("bookId can't be null or empty");

        //bookId为 1-99 之间
        var bookId = Convert.ToInt16(bookIdStr);
        if(bookId is < 0 or > 100)
            return NotFound("bookId must be between 1 and 99");

        //未登录报错
        if (Convert.ToBoolean(Request.Query["isloggedin"]) == false)
        {
            return StatusCode(401, "You must be logged in");
        }    // return Unauthorized("You must be logged in");
        
        // return File("sample.jpg", "application/jpg");
        //重定向 - 302 Found
        // return new RedirectToActionResult("Books", "Store",new{});
        //301
        return new RedirectToActionResult("Books", "Store", new { }, true);
    }
    
}