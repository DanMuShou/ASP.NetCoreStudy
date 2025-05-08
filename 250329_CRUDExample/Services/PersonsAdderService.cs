using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services;

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
