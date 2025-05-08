namespace ServiceContracts;

public interface IPersonsDeleterService
{
    /// <summary>
    /// 依据person guid 删除person
    /// </summary>
    /// <param name="personId">person的唯一标识符</param>
    /// <returns>是否删除成功</returns>
    Task<bool> DeletePerson(Guid? personId);
}
