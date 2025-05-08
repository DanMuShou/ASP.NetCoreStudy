using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using ContactsManagerSolution.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagerSolution.Filters.ActionFilters;

//IActionFilter 是 ASP.NET Core 中定义的一个接口，用于在控制器的操作方法（Action）执行前后插入自定义逻辑。
public class PersonListActionFilter(ILogger<PersonListActionFilter> logger) : IActionFilter
{
    private const string DefaultSearchBy = nameof(PersonResponse.PersonName);

    public void OnActionExecuting(ActionExecutingContext context)
    {
        logger.LogInformation(
            "过滤调用: {FilterName} -> {FilterAction}",
            nameof(PersonListActionFilter),
            nameof(OnActionExecuting)
        );

        context.HttpContext.Items.Add("actionArguments", context.ActionArguments);
        if (context.ActionArguments.TryGetValue("searchBy", out var searchByObj))
        {
            var searchBy = Convert.ToString(searchByObj);
            if (!string.IsNullOrEmpty(searchBy))
            {
                var searchOptionList = new List<string>()
                {
                    nameof(PersonResponse.PersonName),
                    nameof(PersonResponse.Email),
                    nameof(PersonResponse.DateOfBirth),
                    nameof(PersonResponse.Gender),
                    nameof(PersonResponse.CountryId),
                    nameof(PersonResponse.Address),
                };

                // 判断是否为有效地查询字段 无效则重置为默认值
                if (searchOptionList.All(temp => temp != searchBy))
                {
                    //结构化日志记录：适用于需要在日志分析工具中进行复杂查询和过滤的场景，提供了更好的灵活性和可维护性。
                    logger.LogInformation(
                        "无效的查询字段: {InvalidSearchBy}, 已重置为默认值: {DefaultSearchBy}",
                        searchBy,
                        DefaultSearchBy
                    );
                    context.ActionArguments["searchBy"] = DefaultSearchBy;
                }
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        logger.LogInformation(
            "过滤调用: {FilterName} -> {FilterAction}",
            nameof(PersonListActionFilter),
            nameof(OnActionExecuted)
        );

        if (context.HttpContext.Items.TryGetValue("actionArguments", out var actionArgumentsObj))
        {
            if (
                context.Controller is PersonController personsController
                && actionArgumentsObj is IDictionary<string, object> actionArguments
            )
            {
                if (actionArguments.TryGetValue("searchBy", out var searchByObj))
                    personsController.ViewBag.CurrentSearchBy = Convert.ToString(searchByObj);

                if (actionArguments.TryGetValue("searchString", out var searchStringObj))
                    personsController.ViewBag.CurrentSearchString = Convert.ToString(
                        searchStringObj
                    );

                if (actionArguments.TryGetValue("sortBy", out var sortByObj))
                    personsController.ViewBag.CurrentSortBy = Convert.ToString(sortByObj);
                else
                    personsController.ViewBag.CurrentSortBy = nameof(PersonResponse.PersonName);

                if (actionArguments.TryGetValue("sortOrder", out var sortOrderObj))
                    personsController.ViewBag.CurrentSortOrder = Convert.ToString(sortOrderObj);
                else
                    personsController.ViewBag.CurrentSortOrder = nameof(SortOrderOptions.ASC);

                personsController.ViewBag.SearchFields = new Dictionary<string, string>()
                {
                    { nameof(PersonResponse.PersonName), "人员名称" },
                    { nameof(PersonResponse.Email), "邮箱" },
                    { nameof(PersonResponse.DateOfBirth), "出生日期" },
                    { nameof(PersonResponse.Gender), "性别" },
                    { nameof(PersonResponse.CountryName), "国家名称" },
                    { nameof(PersonResponse.Address), "地址" },
                };
            }
        }
    }
}
