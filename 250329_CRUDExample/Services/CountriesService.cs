using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _countries;

    public CountriesService(bool initialize = true)
    {
        _countries = [];
        if (initialize)
        {
            //{9BA97D3E-A636-407E-84BB-A75A122FDC85}
            //{4DD4190E-9021-4C0D-B56F-F09D58A12C62}
            //{1A62C6E0-FF6F-4074-824E-BB0CDE3BE6AC}
            //{0CF07808-3D11-4D54-BF58-093C052155B2}
            //{B13163F7-49FC-44D0-A295-B1602A219604}

            _countries.AddRange(
                new List<Country>()
                {
                    new Country()
                    {
                        CountryId = Guid.Parse("9BA97D3E-A636-407E-84BB-A75A122FDC85"),
                        CountryName = "中国",
                    },
                    new Country()
                    {
                        CountryId = Guid.Parse("4DD4190E-9021-4C0D-B56F-F09D58A12C62"),
                        CountryName = "美国",
                    },
                    new Country()
                    {
                        CountryId = Guid.Parse("1A62C6E0-FF6F-4074-824E-BB0CDE3BE6AC"),
                        CountryName = "英国",
                    },
                    new Country()
                    {
                        CountryId = Guid.Parse("0CF07808-3D11-4D54-BF58-093C052155B2"),
                        CountryName = "法国",
                    },
                    new Country()
                    {
                        CountryId = Guid.Parse("B13163F7-49FC-44D0-A295-B1602A219604"),
                        CountryName = "日本",
                    },
                }
            );
        }
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
