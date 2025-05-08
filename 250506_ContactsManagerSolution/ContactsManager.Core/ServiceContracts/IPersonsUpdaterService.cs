using ContactsManager.Core.DTO;

namespace ContactsManager.Core.ServiceContracts;

public interface IPersonsUpdaterService
{
    /// <summary>
    /// 更新person获得person响应
    /// </summary>
    /// <param name="personUpdateRequest">需要更新的person</param>
    /// <returns>更新完成后响应</returns>
    Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
}
