using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;

namespace ServiceContracts;

public interface IPersonsService
{
    /// <summary>
    /// 添加person到list中
    /// </summary>
    /// <param name="personAddRequest">需要被添加的person请求</param>
    /// <returns>添加的person</returns>
    Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

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
    /// 根据搜索条件获取person的响应
    /// </summary>
    /// <param name="allPersons">需要排序的PersonList</param>
    /// <param name="sortBy">排序的属性</param>
    /// <param name="sort">排序规则 ASC DESC</param>
    /// <returns>排序完成的List</returns>
    Task<List<PersonResponse>> GetSortPersons(
        List<PersonResponse> allPersons,
        string sortBy,
        SortOrderOptions sort
    );

    /// <summary>
    /// 更新person获得person响应
    /// </summary>
    /// <param name="personUpdateRequest">需要更新的person</param>
    /// <returns>更新完成后响应</returns>
    Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

    /// <summary>
    /// 依据person guid 删除person
    /// </summary>
    /// <param name="personId">person的唯一标识符</param>
    /// <returns>是否删除成功</returns>
    Task<bool> DeletePerson(Guid? personId);

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
