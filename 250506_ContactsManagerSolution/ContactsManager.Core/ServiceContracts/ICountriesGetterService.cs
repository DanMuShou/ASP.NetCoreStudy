using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts;

public interface ICountriesGetterService
{
    
    /// <summary>
    /// 获取所有国家
    /// </summary>
    /// <returns>所有国家 List 列表</returns>
    Task<List<CountryResponse>> GetAllCountries();

    /// <summary>
    /// 根据国家ID获取国家
    /// </summary>
    /// <param name="countryId">国家的Guid</param>
    /// <returns>返回添加后响应</returns>
    Task<CountryResponse?> GetCountryByCountryId(Guid? countryId);
}