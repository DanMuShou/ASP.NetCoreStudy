using System.Text.Json;
using _250328_rider_StocksApp.ServiceContracts;

namespace _250328_rider_StocksApp.Services;

//curl "https://finnhub.io/api/v1/quote?symbol=AAPL&token=cvj4rnhr01qpsvoubsb0cvj4rnhr01qpsvoubsbg"
//cvj4rnhr01qpsvoubsb0cvj4rnhr01qpsvoubsbg

// ReSharper disable once IdentifierTypo
public class FinnhubService : IFinnhubService
{
    //客户端工厂
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<Dictionary<string, object>> GetStockPriceQuote(string stockSymbol)
    {
        using var httpClient = _httpClientFactory.CreateClient();
        //http的请求
        var httpRequestMessage = new HttpRequestMessage()
        {
            RequestUri = new Uri(
                $"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"
            ),
            Method = HttpMethod.Get,
        };
        //发送http请求
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        //读取响应
        var stream = await httpResponseMessage.Content.ReadAsStreamAsync();

        //读取流
        var streamReader = new StreamReader(stream);
        var response = await streamReader.ReadToEndAsync();
        var responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

        if (responseDictionary == null)
            throw new InvalidOperationException("没有找到输出");
        if (responseDictionary.TryGetValue("error", out var value))
            throw new InvalidOperationException($"Error : {value}");

        return responseDictionary;
    }
}
