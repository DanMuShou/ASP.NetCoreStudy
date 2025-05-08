using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagerSolution.Filters.AuthorizationFilter;

//filter执行顺序:
//Authorization Filter -> Resource Filter -> Model Binding & Validation
//  -> Action Filter -> Action method -> Action Filter -> Exception Filter
//  -> Result Filter -> IActionResult -> Result Filter -> Resource Filter

// AuthorizationFilter: 验证用户是否登录，验证用户是否有权限访问当前资源
public class TokenAuthorizationFilter() : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (
            !context.HttpContext.Request.Cookies.TryGetValue("Auth-Key", out var auth)
            || auth != "A100"
        )
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }
    }
}
