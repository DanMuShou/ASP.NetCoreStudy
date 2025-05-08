using Microsoft.Extensions.Logging;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;

namespace Services;

public class PersonsSorterService(
    IPersonsRepository personsRepository,
    ILogger<PersonsSorterService> logger,
    IDiagnosticContext diagnosticContext
) : IPersonsSorterService
{
    public Task<List<PersonResponse>> GetSortPersons(
        List<PersonResponse> allPersons,
        string sortBy,
        SortOrderOptions sortOrder
    )
    {
        logger.Log(LogLevel.Information, "服务调用: PersonsService -> GetSortPersons");

        if (string.IsNullOrEmpty(sortBy))
            return Task.FromResult(allPersons);

        var sortedPersons = (sortBy, sortOrder) switch
        {
            (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase)
                .ToList(),

            (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase)
                .ToList(),

            (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.DateOfBirth)
                .ToList(),
            (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.DateOfBirth)
                .ToList(),

            (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),
            (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.Age)
                .ToList(),

            (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase)
                .ToList(),

            (nameof(PersonResponse.CountryName), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.CountryName), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase)
                .ToList(),

            (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase)
                .ToList(),
            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons
                .OrderBy(temp => temp.ReceiveNewsLetters)
                .ToList(),
            (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons
                .OrderByDescending(temp => temp.ReceiveNewsLetters)
                .ToList(),

            _ => allPersons,
        };

        return Task.FromResult(sortedPersons);
    }
}
