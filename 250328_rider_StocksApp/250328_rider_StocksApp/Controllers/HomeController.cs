using _250328_rider_StocksApp.Models;
using _250328_rider_StocksApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace _250328_rider_StocksApp.Controllers;

public class HomeController : Controller
{
    private readonly FinnhubService _finnhubService;
    private readonly IOptions<TradingOption> _tradingOptions;

    //IOptions<TradingOptions> 接口用于在应用启动时读取配置数据 并将其注入到依赖项中
    public HomeController(FinnhubService finnhubService, IOptions<TradingOption> tradingOptions)
    {
        _finnhubService = finnhubService;
        _tradingOptions = tradingOptions;
    }

    [HttpGet("/")]
    [HttpGet("/home")]
    public async Task<IActionResult> Home()
    {
        _tradingOptions.Value.DefaultStockSymbol ??= "MSFT";

        var responseDictionary = await _finnhubService.GetStockPriceQuote(
            _tradingOptions.Value.DefaultStockSymbol
        );

        var stockModel = new StockModel()
        {
            StockSymbol = _tradingOptions.Value.DefaultStockSymbol,
            CurrentPrice = responseDictionary["c"].ToString(),
            LowestPrice = responseDictionary["l"].ToString(),
            HighestPrice = responseDictionary["h"].ToString(),
            OpenPrice = responseDictionary["o"].ToString(),
        };

        return View(stockModel);
    }
}
