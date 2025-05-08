using ContactsManager.Core.Domain.RepositoryContract;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Helpers;
using ContactsManager.Core.ServiceContracts;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ContactsManager.Core.Services;

public class PersonsAdderService(
    IPersonsRepository personsRepository,
    ILogger<PersonsAdderService> logger,
    IDiagnosticContext diagnosticContext
) : IPersonsAdderService
{
    public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
    {
        if (personAddRequest == null)
            ArgumentNullException.ThrowIfNull(personAddRequest);

        //模型验证
        ValidationHelper.ModelValidation(personAddRequest);

        var person = personAddRequest.ToPerson();
        person.PersonId = Guid.NewGuid();

        await personsRepository.AddPerson(person);

        return person.ToPersonResponse();
    }
}
