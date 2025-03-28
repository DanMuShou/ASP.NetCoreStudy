using Microsoft.AspNetCore.Mvc;

namespace _250322_rider_LayoutViewsExample.Controllers;

[Controller]
public class ProductsController : Controller
{
    [Route("products")]
    public IActionResult Products()
    {
        return View();
    }
    
    //URL : products/search/1
    [Route("products/search/{productId:int?}")]
    public IActionResult Search(int? productId)
    {
        ViewBag.ProductId = productId; 
        return View();
    }
    
    [Route("products/order")]
    public IActionResult Order()
    {
        return View();
    }
}