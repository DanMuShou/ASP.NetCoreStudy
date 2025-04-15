using Entities;

namespace RepositoryContracts;

/// <summary>
/// 表示用于管理 国家实体数据库 的访问逻辑
/// </summary>
public interface ICountriesRepository
{
    /// <summary>
    /// 添加国家对象到数据库中
    /// </summary>
    /// <param name="country">需要添加的国家</param>
    /// <returns>国家对象添加完成后返回该对象</returns>
    Task<Country> AddCountry(Country country);

    /// <summary>
    /// 从数据库获取所有国家对象
    /// </summary>
    /// <returns>数据库中所有国家对象</returns>
    Task<List<Country>> GetAllCountries();

    /// <summary>
    /// 根据国家Id从数据库中获取国家对象
    /// </summary>
    /// <param name="countryId">需要查询的国家Id</param>
    /// <returns>根据国家Id从数据库中查询的国家 未找到则为Null</returns>
    Task<Country?> GetCountryById(Guid countryId);

    /// <summary>
    /// 根据国家名称从数据库中获取国家对象
    /// </summary>
    /// <param name="countryName">需要查询的国家名称</param>
    /// <returns>根据国家名称从数据库中查询的国家 未找到则为Null</returns>
    Task<Country?> GetCountryByCountryName(string countryName);
}
