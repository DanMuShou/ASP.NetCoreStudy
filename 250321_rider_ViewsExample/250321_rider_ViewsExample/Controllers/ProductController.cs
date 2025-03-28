using Microsoft.AspNetCore.Mvc;

namespace _250321_rider_ViewsExample.Controllers;

[Controller]
//HomeCon... 控制着/Views/Home/...
//ProductCon... 控制着/Views/Product/...
public class ProductController : Controller
{
    [Route("products/all")]
    public IActionResult All()
    {
        //Views/Products/All.cshtml
        //Views/Shared/All.cshtml
        return View();
    }
}
