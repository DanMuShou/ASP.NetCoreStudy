using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;

namespace ContactsManager.Core.ServiceContracts;

public interface IPersonsSorterService
{
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
}
