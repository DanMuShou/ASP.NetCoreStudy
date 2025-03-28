namespace _250328_rider_StocksApp.ServiceContracts;

// ReSharper disable once IdentifierTypo
public interface IFinnhubService
{
    Task<Dictionary<string, object>> GetStockPriceQuote(string stockSymbol);
}
