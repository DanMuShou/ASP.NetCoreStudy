using Microsoft.AspNetCore.Http;

namespace ContactsManager.Core.ServiceContracts;

public interface ICountriesUploaderService
{
    /// <summary>
    /// 上传并加载Excel文件
    /// </summary>
    /// <param name="formFile">包含国家的excel文件</param>
    /// <returns>返回加载国家的数量</returns>
    Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
}