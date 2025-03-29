using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService()
    {
        _countries = [];
    }

    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        //传入参数校验
        ArgumentNullException.ThrowIfNull(countryAddRequest);
        //参数校验
        if (countryAddRequest.CountryName == null)
            throw new ArgumentException(nameof(countryAddRequest));

        //请求 -> 国家实体
        var country = countryAddRequest.ToCountry();
        //生成GuidId
        country.CountryId = Guid.NewGuid();
        //添加国家
        if (_countries.Any(temp => temp.CountryName == countryAddRequest.CountryName))
            throw new ArgumentException("给出的国家名称已经添加");

        _countries.Add(country);
        //返回国家响应
        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries() =>
        _countries.Select(country => country.ToCountryResponse()).ToList();

    public CountryResponse? GetCountryByCountryID(Guid? countryId)
    {
        if (countryId == null)
            return null;

        var country = _countries.FirstOrDefault(temp => temp.CountryId == countryId);
        return country?.ToCountryResponse();
    }
}
