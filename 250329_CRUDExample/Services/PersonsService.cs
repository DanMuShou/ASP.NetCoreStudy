using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;
using Services.Helpers;

namespace Services;

public class PersonsService : IPersonsService
{
    private readonly ICountriesService _countriesService;
    private readonly List<Person> _persons;

    public PersonsService()
    {
        _persons = [];
        _countriesService = new CountriesService();
    }

    private PersonResponse ConvertPersonToPersonResponse(Person person)
    {
        var personResponse = person.ToPersonResponse();
        personResponse.CountryName = _countriesService.GetCountryByCountryID(person.CountryId)?.CountryName;
        return personResponse;
    }

    public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
    {
        if (personAddRequest == null)
            ArgumentNullException.ThrowIfNull(personAddRequest);

        //模型验证
        ValidationHelper.ModelValidation(personAddRequest);

        var person = personAddRequest.ToPerson();
        person.PersonId = Guid.NewGuid();
        _persons.Add(person);

        return ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetAllPersons()
    {
        return _persons.Select(person => person.ToPersonResponse()).ToList();
    }

    public PersonResponse? GetPersonByPersonID(Guid? personId)
    {
        if (personId == null)
            return null;

        var person = _persons.FirstOrDefault(p => p.PersonId == personId);
        return person == null ? null : ConvertPersonToPersonResponse(person);
    }

    public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
    {
        var allPerson = GetAllPersons();
        var matchingPerson = allPerson;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            return matchingPerson;

        switch (searchBy)
        {
            case nameof(Person.PersonName):
                matchingPerson = allPerson
                    .Where(temp =>
                        !string.IsNullOrEmpty(temp.PersonName)
                        && temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
                break;
            case nameof(Person.Email):
                matchingPerson = allPerson
                    .Where(temp =>
                        !string.IsNullOrEmpty(temp.Email)
                        && temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
                break;
            case nameof(Person.DateOfBirth):
                matchingPerson = allPerson
                    .Where(temp =>
                        (temp.DateOfBirth != null)
                        && temp.DateOfBirth.Value.ToString("dd MMMM yyyy")
                            .Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
                break;
            case nameof(Person.Gender):
                matchingPerson = allPerson
                    .Where(temp =>
                        !string.IsNullOrEmpty(temp.Gender)
                        && temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
                break;
            case nameof(Person.CountryId):
                matchingPerson = allPerson
                    .Where(temp =>
                        (temp.CountryName != null)
                        && temp.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
                break;
            case nameof(Person.Address):
                matchingPerson = allPerson
                    .Where(temp =>
                        !string.IsNullOrEmpty(temp.Address)
                        && temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
                break;
            default:
                matchingPerson = allPerson;
                break;
        }
        return matchingPerson;
    }

    public List<PersonResponse> GetSortPersons(
        List<PersonResponse> allPersons,
        string sortBy,
        SortOrderOptions sortOrder
    )
    {
        if (string.IsNullOrEmpty(sortBy))
            return allPersons;

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

        return sortedPersons;
    }

    public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(personUpdateRequest);

        //model validation
        ValidationHelper.ModelValidation(personUpdateRequest);
        //update person
        var targetPerson = _persons.FirstOrDefault(temp => temp.PersonId == personUpdateRequest.PersonId);
        if (targetPerson == null)
            throw new ArgumentException("给出的Person无法找到");

        targetPerson.PersonName = personUpdateRequest.PersonName;
        targetPerson.Email = personUpdateRequest.Email;
        targetPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
        targetPerson.Gender = personUpdateRequest.Gender.ToString();
        targetPerson.CountryId = personUpdateRequest.CountryId;
        targetPerson.Address = personUpdateRequest.Address;
        targetPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

        return ConvertPersonToPersonResponse(targetPerson);
    }

    public bool DeletePerson(Guid? personId)
    {
        ArgumentNullException.ThrowIfNull(personId);
        var targetPerson = _persons.FirstOrDefault(temp => temp.PersonId == personId);

        if (targetPerson == null)
            return false;

        //删除所有该person id的记录
        _persons.RemoveAll(temp => temp.PersonId == personId);
        return true;
    }
}
