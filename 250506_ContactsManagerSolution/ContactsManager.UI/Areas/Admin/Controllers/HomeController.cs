using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagerSolution.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,others")]
[Route("[area]/[controller]/[action]")]
public class HomeController : Controller
{
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        return View();
    }
}
