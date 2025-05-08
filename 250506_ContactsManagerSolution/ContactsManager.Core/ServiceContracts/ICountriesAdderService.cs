using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts;

public interface ICountriesAdderService
{
    /// <summary>
    /// 添加国家到list中
    /// </summary>
    /// <param name="countryAddRequest">需要被添加的国家请求</param>
    /// <returns>返回添加后的响应(包含ID)</returns>
    Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

}