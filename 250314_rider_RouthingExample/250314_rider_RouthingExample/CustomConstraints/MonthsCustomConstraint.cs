using System.Text.RegularExpressions;

namespace _250314_rider_RoutingExample.CustomConstraints;

public class MonthsCustomConstraint : IRouteConstraint
{
    //sales-report/2020/apr
    public bool Match(
        HttpContext? httpContext, IRouter? route,
        string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        //检查值是否包括
        if (!values.TryGetValue(routeKey, out var value)) return false;

        var regex = new Regex("^(apr|jul|oct|jan)$");
        var monthValue = Convert.ToString(value);
        if (regex.IsMatch(monthValue))
            return true;

        return false;
    }
}