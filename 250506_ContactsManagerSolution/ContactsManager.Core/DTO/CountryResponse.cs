using ContactsManager.Core.Domain.Entities;

namespace ContactsManager.Core.DTO;

/// <summary>
/// DTO 适用于大多数CountriesService的返回类
/// </summary>
public class CountryResponse
{
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }

    //重写Equals方法，通过CountryId和CountryName属性值比较两个CountryResponse对象是否相等
    // 仅当传入对象为CountryResponse类型且两个属性值完全匹配时返回true。
    public override bool Equals(object? obj)
    {
        if (obj is CountryResponse countryResponse)
        {
            return CountryId == countryResponse.CountryId
                && CountryName == countryResponse.CountryName;
        }
        return false;
    }

    public override int GetHashCode()
    {
        // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
        return base.GetHashCode();
    }
}

public static class CountryResponseExtensions
{
    //国家对响应的转化
    public static CountryResponse ToCountryResponse(this Country country)
    {
        return new CountryResponse
        {
            CountryId = country.CountryId,
            CountryName = country.CountryName,
        };
    }
}
