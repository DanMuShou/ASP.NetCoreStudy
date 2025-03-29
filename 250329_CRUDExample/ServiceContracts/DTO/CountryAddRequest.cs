using Entities;

namespace ServiceContracts.DTO;

/// <summary>
/// 添加国家的DTO
/// </summary>
public class CountryAddRequest
{
    public string? CountryName { get; set; }
}

public static class CountryAddRequestExtensions
{
    public static Country ToCountry(this CountryAddRequest countryAddRequest)
    {
        return new Country() { CountryName = countryAddRequest.CountryName };
    }
}
