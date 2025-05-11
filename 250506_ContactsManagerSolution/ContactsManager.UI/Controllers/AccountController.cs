using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagerSolution.Controllers;

[Route("[controller]/[action]")]
public class AccountController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager
) : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState
                .Values.SelectMany(m => m.Errors)
                .Select(e => e.ErrorMessage);
            return View(registerDto);
        }

        var user = new ApplicationUser
        {
            //UserName : 用于登录标识并不是用户名称
            UserName = registerDto.Email,
            Email = registerDto.Email,
            PhoneNumber = registerDto.Phone,
            PersonName = registerDto.PersonName,
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            //isPersistent ? 即使关闭浏览器，用户仍然保持登录状态（记住我）: 关闭浏览器后 Cookie 将被清除（会话登录）
            await signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Home", "Person");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("注册", error.Description);
        }

        return View(registerDto);
    }
}
