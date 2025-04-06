using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly PersonsDbContext _db;

    public CountriesService(PersonsDbContext personsDbContext)
    {
        _db = personsDbContext;
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
        if (_db.Countries.Any(temp => temp.CountryName == countryAddRequest.CountryName))
            throw new ArgumentException("给出的国家名称已经添加");

        //保存数据
        _db.Countries.Add(country);
        _db.SaveChanges();

        //返回国家响应
        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries() =>
        _db.Countries.Select(country => country.ToCountryResponse()).ToList();

    public CountryResponse? GetCountryByCountryID(Guid? countryId)
    {
        if (countryId == null)
            return null;
        var country = _db.Countries.FirstOrDefault(temp => temp.CountryId == countryId)?.ToCountryResponse();
        return country;
    }
}
