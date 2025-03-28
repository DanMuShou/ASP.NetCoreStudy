using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationExample.Controllers;

[Controller]
public class HomeController : Controller
{
    public WeatherApiOptions _weaterOptions;

    public HomeController(IOptions<WeatherApiOptions> weatherApiOptions)
    {
        _weaterOptions = weatherApiOptions.Value;
    }

    [HttpGet("/")]
    [HttpGet("/home")]
    public IActionResult Home()
    {
        // var weatherApiSection = _configuration.GetSection("WeatherApi").Get<WeatherApiOptions>();
        // var weatherApiSection = new WeatherApiOptions();
        // _weaterOptions.GetSection("WeatherApi").Bind(weatherApiSection);

        ViewBag.ClientID = _weaterOptions?.ClientID;
        ViewBag.ClientSecret = _weaterOptions?.ClientSecret;

        return View();
    }
}
