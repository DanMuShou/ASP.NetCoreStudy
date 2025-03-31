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
    private readonly List<Person> _persons = [];

    public PersonsService(bool initialize = true)
    {
        _countriesService = new CountriesService();

        if (initialize)
        {
            //1ECB0873-CC04-939B-16F5-97FAAE76AA93
            //A47277A6-0C22-A0C7-E104-AE4D5A7476C4
            //52EE1468-5FDD-08D5-96EA-68BAFB7E2C37
            //3FEC9D22-7302-2813-2DE8-C1F29B38DD2B
            //17B7F594-1E65-8D4C-A06C-2F47851DDB19
            //04D03CE7-51DB-27A7-AF9E-A482E903FD1E
            // PersonName,Email,DateOfBrith,Gender,Address,ReceiveNewsLetters
            // Marius,mdanielsson0@tinypic.com,1992-06-01,Female,1411 Huxley Point,false
            // Kienan,kjobey1@liveinternet.ru,1991-11-26,Female,41674 Kedzie Way,true
            // Dael,diorio2@webeden.co.uk,1995-05-21,Male,643 Spohn Circle,true
            // Briano,bhenlon3@blogs.com,1999-12-29,Female,3 Lakewood Junction,false
            // Deane,dluberto4@usa.gov,1993-06-15,Female,3359 Farwell Avenue,true

            _persons.AddRange(
                [
                    new Person
                    {
                        PersonId = Guid.Parse("1ECB0873-CC04-939B-16F5-97FAAE76AA93"),
                        PersonName = "Marius",
                        Email = "mdanielsson0@tinypic.com",
                        DateOfBirth = Convert.ToDateTime("1/22/1991"),
                        Gender = "Female",
                        Address = "1411 Huxley Point",
                        ReceiveNewsLetters = false,
                        CountryId = Guid.Parse("9BA97D3E-A636-407E-84BB-A75A122FDC85"),
                    },
                    new Person
                    {
                        PersonId = Guid.Parse("A47277A6-0C22-A0C7-E104-AE4D5A7476C4"),
                        PersonName = "Kienan",
                        Email = "kjobey1@liveinternet.ru",
                        DateOfBirth = Convert.ToDateTime("12/23/1999"),
                        Gender = "Male",
                        Address = "41674 Kedzie Way",
                        ReceiveNewsLetters = true,
                        CountryId = Guid.Parse("4DD4190E-9021-4C0D-B56F-F09D58A12C62"),
                    },
                    new Person
                    {
                        PersonId = Guid.Parse("52EE1468-5FDD-08D5-96EA-68BAFB7E2C37"),
                        PersonName = "Briano",
                        Email = "bhenlon3@blogs.com",
                        DateOfBirth = Convert.ToDateTime("8/10/1998"),
                        Gender = "Female",
                        Address = "3 Lakewood Junction",
                        ReceiveNewsLetters = false,
                        CountryId = Guid.Parse("1A62C6E0-FF6F-4074-824E-BB0CDE3BE6AC"),
                    },
                    new Person
                    {
                        PersonId = Guid.Parse("3FEC9D22-7302-2813-2DE8-C1F29B38DD2B"),
                        PersonName = "Deane",
                        Email = "dluberto4@usa.gov",
                        DateOfBirth = Convert.ToDateTime("3/20/1993"),
                        Gender = "Male",
                        Address = "3359 Farwell Avenue",
                        ReceiveNewsLetters = true,
                        CountryId = Guid.Parse("0CF07808-3D11-4D54-BF58-093C052155B2"),
                    },
                    new Person
                    {
                        PersonId = Guid.Parse("17B7F594-1E65-8D4C-A06C-2F47851DDB19"),
                        PersonName = "Cordell",
                        Email = "cwisby5@ucsd.edu",
                        DateOfBirth = Convert.ToDateTime("3/6/1992"),
                        Gender = "Male",
                        Address = "5004 Gerald Circle",
                        ReceiveNewsLetters = true,
                        CountryId = Guid.Parse("B13163F7-49FC-44D0-A295-B1602A219604"),
                    },
                    new Person
                    {
                        PersonId = Guid.Parse("04D03CE7-51DB-27A7-AF9E-A482E903FD1E"),
                        PersonName = "Hamil",
                        Email = "hsakins6@seattletimes.com",
                        DateOfBirth = Convert.ToDateTime("6/14/1993"),
                        Gender = "Female",
                        Address = "29 Lien Center",
                        ReceiveNewsLetters = false,
                        CountryId = Guid.Parse("9BA97D3E-A636-407E-84BB-A75A122FDC85"),
                    },
                ]
            );
        }
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
        return _persons.Select(ConvertPersonToPersonResponse).ToList();
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
            case nameof(PersonResponse.PersonName):
                matchingPerson = allPerson
                    .Where(temp =>
                        !string.IsNullOrEmpty(temp.PersonName)
                        && temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
                break;
            case nameof(PersonResponse.Email):
                matchingPerson = allPerson
                    .Where(temp =>
                        !string.IsNullOrEmpty(temp.Email)
                        && temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
                break;
            case nameof(PersonResponse.DateOfBirth):
                matchingPerson = allPerson
                    .Where(temp =>
                        (temp.DateOfBirth != null)
                        && temp.DateOfBirth.Value.ToString("dd MMMM yyyy")
                            .Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
                break;
            case nameof(PersonResponse.Gender):
                matchingPerson = allPerson
                    .Where(temp =>
                        !string.IsNullOrEmpty(temp.Gender)
                        && temp.Gender.Equals(searchString, StringComparison.CurrentCultureIgnoreCase)
                    )
                    .ToList();
                break;
            case nameof(PersonResponse.CountryId):
                matchingPerson = allPerson
                    .Where(temp =>
                        (temp.CountryName != null)
                        && temp.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
                break;
            case nameof(PersonResponse.Address):
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
