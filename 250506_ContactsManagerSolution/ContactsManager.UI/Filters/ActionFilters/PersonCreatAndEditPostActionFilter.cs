using ContactsManager.Core.ServiceContracts;
using ContactsManagerSolution.Controllers;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContactsManagerSolution.Filters.ActionFilters;

public class PersonCreatAndEditPostActionFilter(
    ICountriesGetterService countriesGetterService,
    ILogger<PersonCreatAndEditPostActionFilter> logger
) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        if (context.Controller is not PersonController personController)
        {
            await next();
            return;
        }

        //验证模型是否正确填写
        if (!personController.ModelState.IsValid)
        {
            var countryList = await countriesGetterService.GetAllCountries();
            personController.ViewBag.Countries = countryList.Select(c => new SelectListItem()
            {
                Text = c.CountryName,
                Value = c.CountryId.ToString(),
            });
            personController.ViewBag.Errors = personController
                .ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var personRequest = context.ActionArguments["personRequest"];
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            context.Result = personController.View(actionDescriptor?.ActionName, personRequest);
        }

        //该方法是ASP.NET Core管道的关键，用于继续执行后续的Action或结果处理。
        //若不调用next()，请求会停留在当前Filter中，无法进入控制器Action方法或返回响应。
        await next();

        logger.LogInformation(
            "过滤调用: {FilterName} -> {FilterAction}",
            nameof(PersonCreatAndEditPostActionFilter),
            nameof(OnActionExecutionAsync)
        );
    }
}
