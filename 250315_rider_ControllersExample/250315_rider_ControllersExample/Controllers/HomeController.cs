using Microsoft.AspNetCore.Mvc;
using _250315_rider_ControllersExample.Models;

namespace _250315_rider_ControllersExample.Controllers;

[Controller]
public class HomeController : Controller
{
    [Route("home")]
    [Route("/")]
    public ContentResult Index()
    {
        // var result = new ContentResult()
        // {
        //     ContentType = "text/plain;charset=utf-8",
        //     Content = "Hell from Index 你好index!",
        // };
        
        
        
        return Content("<h1>Welcome</h1> <h2>你好 index</h2>", "text/html;charset=utf-8");
    }

    [Route("about")]
    public string About()
    {
        return "Hello from About !";
    }
    
    [Route("contact-us/{mobile:regex(^\\d{{10}}$)}")]
    public string Contact()
    {
        return "Hello from Contact !";
    }
    
    [Route("person")]
    public JsonResult Person()
    {
        var person = new Person()
        {
            Id = Guid.NewGuid(),
            FirstName = "Bob",
            LastName = "Smith",
            Age = 20,
        };
        return Json(person);
    }

    [Route("file-download")]
    public VirtualFileResult FileDownload()
    {
        // return new VirtualFileResult("/sample.pdf", "application/pdf");
        return File("/sample.pdf", "application/pdf");
    }
    
    [Route("file-download2")]
    public PhysicalFileResult FileDownload2()
    {
        // return new PhysicalFileResult(
        //     @"E:\\.Project\\GitUp\\ASP.NetCoreStudy\\250315_rider_ControllersExample\\250315_rider_ControllersExample\\myRoot\\sample.png", 
        //     "application/png");

        return PhysicalFile(
            @"E:\\.Project\\GitUp\\ASP.NetCoreStudy\\250315_rider_ControllersExample\\250315_rider_ControllersExample\\myRoot\\sample.png",
            "application/png");
    }
    
    [Route("file-download3")]
    public FileContentResult FileDownload3()
    {
        var bytes = System.IO.File.ReadAllBytes(
            @"E:\\.Project\\GitUp\\ASP.NetCoreStudy\\250315_rider_ControllersExample\\250315_rider_ControllersExample\\myRoot\\sample.png");
        // return new FileContentResult(bytes, "application/png");
        return File(bytes, "application/png");
    }
 
}