// using ContactsManager.Core.DTO;
// using Microsoft.AspNetCore.Http;
//
// namespace ContactsManager.Core.ServiceContracts;
//
// /// <summary>
// /// 国家业务逻辑
// /// </summary>
// public interface ICountriesService
// {
//     /// <summary>
//     /// 添加国家到list中
//     /// </summary>
//     /// <param name="countryAddRequest">需要被添加的国家请求</param>
//     /// <returns>返回添加后的响应(包含ID)</returns>
//     Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
//
//     /// <summary>
//     /// 获取所有国家
//     /// </summary>
//     /// <returns>所有国家 List 列表</returns>
//     Task<List<CountryResponse>> GetAllCountries();
//
//     /// <summary>
//     /// 根据国家ID获取国家
//     /// </summary>
//     /// <param name="countryId">国家的Guid</param>
//     /// <returns>返回添加后响应</returns>
//     Task<CountryResponse?> GetCountryByCountryId(Guid? countryId);
//
//     /// <summary>
//     /// 上传并加载Excel文件
//     /// </summary>
//     /// <param name="formFile">包含国家的excel文件</param>
//     /// <returns>返回加载国家的数量</returns>
//     Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
// }
