using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts;

public interface IPersonsGetterService
{
    /// <summary>
    /// 获取所有person的响应
    /// </summary>
    /// <returns>所有Person的List</returns>
    Task<List<PersonResponse>> GetAllPersons();

    /// <summary>
    /// 根据PersonId获取person的响应
    /// </summary>
    /// <param name="personId">Person Guid</param>
    /// <returns>返回Person</returns>
    Task<PersonResponse?> GetPersonByPersonId(Guid? personId);

    /// <summary>
    /// 根据搜索条件获取person的响应
    /// </summary>
    /// <param name="searchBy">搜索字段以搜索</param>
    /// <param name="searchString">搜索字符串以搜索</param>
    /// <returns>返回符合条件的PersonList</returns>
    Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);

    /// <summary>
    /// 获取person的csv
    /// </summary>
    /// <returns>返回 csv 信息流</returns>
    Task<MemoryStream> GetPersonCsv();

    /// <summary>
    /// 获取person的excel
    /// </summary>
    /// <returns>返回 Excel 的信息流</returns>
    Task<MemoryStream> GetPersonExcel();
}
