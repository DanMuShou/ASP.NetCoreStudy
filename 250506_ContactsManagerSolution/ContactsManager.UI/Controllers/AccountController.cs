using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManagerSolution.Controllers;

// 允许匿名访问
//[AllowAnonymous]
[Route("[controller]/[action]")]
public class AccountController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    RoleManager<ApplicationRole> roleManager
) : Controller
{
    [HttpGet]
    [Authorize("NotAuthenticated")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [Authorize("NotAuthenticated")]
    [ValidateAntiForgeryToken]
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
            //检查登录身份
            if (registerDto.UserType == UserTypeOptions.Admin)
            {
                if ((await roleManager.FindByNameAsync(nameof(UserTypeOptions.Admin))) == null)
                {
                    var applicationRole = new ApplicationRole()
                    {
                        Name = nameof(UserTypeOptions.Admin),
                    };
                    await roleManager.CreateAsync(applicationRole);
                }
                await userManager.AddToRoleAsync(user, nameof(UserTypeOptions.Admin));
            }
            else
            {
                if ((await roleManager.FindByNameAsync(nameof(UserTypeOptions.User))) == null)
                {
                    var applicationRole = new ApplicationRole()
                    {
                        Name = nameof(UserTypeOptions.User),
                    };
                    await roleManager.CreateAsync(applicationRole);
                }
                await userManager.AddToRoleAsync(user, nameof(UserTypeOptions.User));
            }

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

    [HttpGet]
    [Authorize("NotAuthenticated")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [Authorize("NotAuthenticated")]
    public async Task<IActionResult> Login(LoginDto loginDto, string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState
                .Values.SelectMany(m => m.Errors)
                .Select(e => e.ErrorMessage);
            return View(loginDto);
        }

        // 使用提供的电子邮件和密码对用户进行异步身份验证
        var result = await signInManager.PasswordSignInAsync(
            loginDto.Email, // 用户登录使用的电子邮件地址
            loginDto.Password, // 用户提供的密码
            false, // isPersistent: 是否持久化登录（记住我）功能，false 表示关闭浏览器后登录状态失效
            false // lockoutOnFailure: 登录失败时是否锁定账户，false 表示不启用锁定机制
        );

        if (result.Succeeded)
        {
            //Admin
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user != null)
            {
                if (await userManager.IsInRoleAsync(user, nameof(UserTypeOptions.Admin)))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                // 不能使用Redirect 会导致注入攻击 my.com/?returnUrl=https://攻击.com
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Home", "Person");
        }

        ModelState.AddModelError("登录", "无效密码或邮箱");
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [AllowAnonymous]
    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return Json(user == null);
    }
}
