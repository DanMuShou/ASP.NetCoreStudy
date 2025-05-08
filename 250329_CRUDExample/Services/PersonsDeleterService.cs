using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;

namespace Services;

public class PersonsDeleterService(
    IPersonsRepository personsRepository,
    ILogger<PersonsDeleterService> logger,
    IDiagnosticContext diagnosticContext
) : IPersonsDeleterService
{
    public async Task<bool> DeletePerson(Guid? personId)
    {
        ArgumentNullException.ThrowIfNull(personId);
        var targetPerson = await personsRepository.GetPersonByPersonId(personId.Value);

        if (targetPerson == null)
            return false;

        //删除该person id的记录
        return await personsRepository.DeletePersonByPersonId(targetPerson.PersonId);
    }
}
