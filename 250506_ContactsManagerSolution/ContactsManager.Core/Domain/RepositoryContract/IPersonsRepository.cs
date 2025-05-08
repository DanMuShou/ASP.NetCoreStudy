using System.Linq.Expressions;
using ContactsManager.Core.Domain.Entities;

namespace ContactsManager.Core.Domain.RepositoryContract;

/// <summary>
/// 表示用于管理 人员实体数据库 的访问逻辑
/// </summary>
public interface IPersonsRepository
{
    /// <summary>
    /// 添加人员对象到数据库中
    /// </summary>
    /// <param name="person">需要添加的人员</param>
    /// <returns>国家对象添加完成后返回该对象</returns>
    Task<Person> AddPerson(Person person);

    /// <summary>
    /// 从数据库获取所有人员对象
    /// </summary>
    /// <returns>数据库中所有人员对象</returns>
    Task<List<Person>> GetAllPersons();

    /// <summary>
    /// 根据人员Id从数据库中获取人员对象
    /// </summary>
    /// <param name="personId">需要查询的对象的Id</param>
    /// <returns>根据人员Id从数据库中查询的国家 未找到则为Null</returns>
    Task<Person?> GetPersonByPersonId(Guid personId);

    /// <summary>
    /// 根据人员过滤条件从数据库中获取人员对象
    /// </summary>
    /// <param name="predicate">用来过滤的LINQ表达式</param>
    /// <returns>所有符合过滤条件的人员对象</returns>
    Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

    /// <summary>
    /// 根据人员Id从数据库中删除人员对象
    /// </summary>
    /// <param name="personId">需要删除的对象的Id</param>
    /// <returns>是否删除成功</returns>
    Task<bool> DeletePersonByPersonId(Guid personId);

    /// <summary>
    /// 根据给定的人员对象的Id来更新数据库中的人员对象
    /// </summary>
    /// <param name="person">需要更新的对象</param>
    /// <returns>更新后的对象</returns>
    Task<Person> UpdatePerson(Person person);
}
