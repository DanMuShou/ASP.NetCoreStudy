using Autofac;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace _250324_rider_DIExample.Controllers;

public class HomeController : Controller
{
    public readonly ICitiesService CitiesService1;
    private readonly ICitiesService _citiesService2;
    private readonly ICitiesService _citiesService3;

    private readonly ILifetimeScope _lifeTimeScope;

    public HomeController(
        ICitiesService citiesService1,
        ICitiesService citiesService2,
        ICitiesService citiesService3,
        ILifetimeScope lifeTimeScope
    )
    {
        CitiesService1 = citiesService1;
        _citiesService2 = citiesService2;
        _citiesService3 = citiesService3;

        _lifeTimeScope = lifeTimeScope;
    }

    [Route("/")]
    [Route("/Home")]
    public IActionResult Home()
    {
        var cities = CitiesService1.GetCities();

        ViewBag.InstanceId1 = CitiesService1.ServiceInstanceId;
        ViewBag.InstanceId2 = _citiesService2.ServiceInstanceId;
        ViewBag.InstanceId3 = _citiesService3.ServiceInstanceId;

        //Service
        // //执行结束时 using自动调用Dispose
        // using (var scope = _lifeTimeScope.CreateScope())
        // {
        //     //没有注册就抛出异常     GetService为null
        //     var citiesService = scope.ServiceProvider.GetRequiredService<ICitiesService>();
        //
        //     ViewBag.InstanceId_CitiesServicece_InScope1 = citiesService.ServiceInstanceId;
        // } //end calls CitiesService.Dispose

        //Autofac
        //执行结束时 using自动调用Dispose
        using (var scope = _lifeTimeScope.BeginLifetimeScope())
        {
            //没有注册就抛出异常     GetService为null
            var citiesService = scope.Resolve<ICitiesService>();
            ViewBag.InstanceId_CitiesServicece_InScope2 = citiesService.ServiceInstanceId;
        } //end calls CitiesService.Dispose

        return View(cities);
    }
}
