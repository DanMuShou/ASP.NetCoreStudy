using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services;

public class PersonsUpdaterService(
    IPersonsRepository personsRepository,
    ILogger<PersonsUpdaterService> logger,
    IDiagnosticContext diagnosticContext
) : IPersonsUpdaterService
{
    public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        logger.Log(LogLevel.Information, "服务调用: PersonsService -> UpdatePerson");

        ArgumentNullException.ThrowIfNull(personUpdateRequest);

        //model validation
        ValidationHelper.ModelValidation(personUpdateRequest);
        //update person
        var targetPerson = await personsRepository.GetPersonByPersonId(personUpdateRequest.PersonId);
        if (targetPerson == null)
        {
            throw new InvalidPersonIdException("给出的 人员Id 无法找到");
        }

        targetPerson.PersonName = personUpdateRequest.PersonName;
        targetPerson.Email = personUpdateRequest.Email;
        targetPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
        targetPerson.Gender = personUpdateRequest.Gender.ToString();
        targetPerson.CountryId = personUpdateRequest.CountryId;
        targetPerson.Address = personUpdateRequest.Address;
        targetPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

        var updatedPerson = await personsRepository.UpdatePerson(targetPerson);
        return updatedPerson.ToPersonResponse();
    }
}
