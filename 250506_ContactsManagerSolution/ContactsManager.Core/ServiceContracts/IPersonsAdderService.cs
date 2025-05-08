using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts;

public interface IPersonsAdderService
{
    /// <summary>
    /// 添加person到list中
    /// </summary>
    /// <param name="personAddRequest">需要被添加的person请求</param>
    /// <returns>添加的person</returns>
    Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
}
