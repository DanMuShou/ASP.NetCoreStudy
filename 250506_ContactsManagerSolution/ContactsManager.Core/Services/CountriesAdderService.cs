using ContactsManager.Core.Domain.RepositoryContract;
using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts;

namespace ContactsManager.Core.Services;

public class CountriesAdderService(ICountriesRepository countriesRepository) : ICountriesAdderService
{
    public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
    {
        //传入参数校验
        ArgumentNullException.ThrowIfNull(countryAddRequest);
        //参数校验
        if (countryAddRequest.CountryName == null)
            throw new ArgumentException(null, nameof(countryAddRequest));

        //请求 -> 国家实体
        var country = countryAddRequest.ToCountry();
        //生成GuidId
        country.CountryId = Guid.NewGuid();
        //添加国家
        if ((await countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName)) != null)
            throw new ArgumentException("给出的国家名称已经添加");

        //保存数据
        await countriesRepository.AddCountry(country);

        //返回国家响应
        return country.ToCountryResponse();
    }

}