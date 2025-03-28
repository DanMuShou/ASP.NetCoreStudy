﻿using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Enums;
using Services;
using Xunit.Abstractions;

namespace CRUDTests;

public class PersonsServiceTest
{
    private readonly IPersonsService _personsService;
    private readonly ICountriesService _countriesService;
    private readonly ITestOutputHelper _testOutputHelper;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        _personsService = new PersonsService();
        _countriesService = new CountriesService();
        _testOutputHelper = testOutputHelper;
    }

    #region AddPerson
    //null request -> throw argumentNullException
    [Fact]
    public void AddPerson_PersonNameNull()
    {
        //arrange
        var request = new PersonAddRequest() { PersonName = null };
        //assert
        Assert.Throws<ArgumentException>(() =>
        {
            //act
            _personsService.AddPerson(request);
        });
    }

    //null request -> throw argumentNullException
    [Fact]
    public void AddPerson_NullPerson()
    {
        //arrange
        PersonAddRequest? request = null;
        //assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            //act
            _personsService.AddPerson(request);
        });
    }

    //right request -> return personResponse(包含 person guid)
    [Fact]
    public void AddPerson_RightRequest()
    {
        //arrange
        var request = new PersonAddRequest()
        {
            PersonName = "test",
            Email = "test@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = Guid.NewGuid(),
            Address = "test",
            ReceiveNewsLetters = false,
        };
        //act
        var response = _personsService.AddPerson(request);
        var allPersonResponsesGuid = _personsService
            .GetAllPersons()
            .Select(temp => temp.PersonId)
            .ToList();
        Assert.Contains(response.PersonId, allPersonResponsesGuid);
        Assert.True(response.PersonId != Guid.Empty);
    }
    #endregion

    #region GetPersonByPersonId

    //null personId -> null personResponse
    [Fact]
    public void GetPersonByPersonId_NullPersonId()
    {
        //arrange
        Guid? personId = null;
        //act
        var actualResponse = _personsService.GetPersonByPersonID(personId);
        //assert
        Assert.Null(actualResponse);
    }

    //error personId -> argumentNullException
    [Fact]
    public void GetPersonByPersonId_ErrorPersonId()
    {
        //arrange
        Guid? personId = Guid.NewGuid();
        //act
        var actualResponse = _personsService.GetPersonByPersonID(personId);
        //assert
        Assert.Null(actualResponse);
    }

    //right personId -> return personResponse
    [Fact]
    public void GetPersonByPersonId_RightPersonId()
    {
        var countryAddRequest1 = new CountryAddRequest() { CountryName = "China" };
        var countryAddRequest2 = new CountryAddRequest() { CountryName = "Russia" };

        var countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
        var countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

        var personAddRequest1 = new PersonAddRequest()
        {
            PersonName = "test1",
            Email = "test1@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse1.CountryId,
            Address = "test",
            ReceiveNewsLetters = false,
        };

        var personAddRequest2 = new PersonAddRequest()
        {
            PersonName = "test2",
            Email = "test2@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse2.CountryId,
            Address = "test",
            ReceiveNewsLetters = false,
        };

        var personResponse1 = _personsService.AddPerson(personAddRequest1);
        var personResponse2 = _personsService.AddPerson(personAddRequest2);

        var allPersonResponse = _personsService.GetAllPersons();

        Assert.Contains(personResponse1.PersonId, allPersonResponse.Select(temp => temp.PersonId));
        Assert.Contains(personResponse2.PersonId, allPersonResponse.Select(temp => temp.PersonId));
        Assert.Equal(
            personResponse1.PersonId,
            _personsService.GetPersonByPersonID(personResponse1.PersonId)?.PersonId
        );
        Assert.Equal(
            personResponse2.PersonId,
            _personsService.GetPersonByPersonID(personResponse2.PersonId)?.PersonId
        );
    }

    #endregion

    #region GetAllPersons
    // return Empty or default
    [Fact]
    public void GetAllPersons_Empty()
    {
        //arrange
        var emptyGet = _personsService.GetAllPersons();
        //act
        Assert.Empty(emptyGet);
    }

    [Fact]
    public void GetAllPersons_Default()
    {
        var countryAddRequest1 = new CountryAddRequest() { CountryName = "China" };
        var countryAddRequest2 = new CountryAddRequest() { CountryName = "Russia" };

        var countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
        var countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

        var personAddRequest1 = new PersonAddRequest()
        {
            PersonName = "test1",
            Email = "test1@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse1.CountryId,
            Address = "test",
            ReceiveNewsLetters = false,
        };

        var personAddRequest2 = new PersonAddRequest()
        {
            PersonName = "test2",
            Email = "test2@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse2.CountryId,
            Address = "test",
            ReceiveNewsLetters = false,
        };

        var personAddRequestList = new List<PersonAddRequest>()
        {
            personAddRequest1,
            personAddRequest2,
        };

        //打印输出
        _testOutputHelper.WriteLine("Expected");
        foreach (var personAddRequest in personAddRequestList)
        {
            _testOutputHelper.WriteLine(personAddRequest.ToString());
        }

        var personResponseList = personAddRequestList
            .Select(personAddRequest => _personsService.AddPerson(personAddRequest))
            .ToList();

        var allPersonResponse = _personsService.GetAllPersons();

        //打印输出
        _testOutputHelper.WriteLine("Actual");
        foreach (var personResponse in allPersonResponse)
        {
            _testOutputHelper.WriteLine(personResponse.ToString());
        }

        foreach (var personResponse in personResponseList)
        {
            Assert.Contains(personResponse, allPersonResponse);
        }
    }
    #endregion

    #region GetFilteredPerson
    //searchTest is "PersonName" or empty -> return allPersonResponse
    [Fact]
    public void GetFilteredPerson_Default()
    {
        var countryAddRequest1 = new CountryAddRequest() { CountryName = "China" };
        var countryAddRequest2 = new CountryAddRequest() { CountryName = "Russia" };
        var countryAddRequest3 = new CountryAddRequest() { CountryName = "India" };

        var countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
        var countryResponse2 = _countriesService.AddCountry(countryAddRequest2);
        var countryResponse3 = _countriesService.AddCountry(countryAddRequest3);

        var personAddRequest1 = new PersonAddRequest()
        {
            PersonName = "test1",
            Email = "test1@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse1.CountryId,
            Address = "test",
            ReceiveNewsLetters = false,
        };

        var personAddRequest2 = new PersonAddRequest()
        {
            PersonName = "test2",
            Email = "test2@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse2.CountryId,
            Address = "test",
            ReceiveNewsLetters = false,
        };

        var personAddRequest3 = new PersonAddRequest()
        {
            PersonName = "test3",
            Email = "test3@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse3.CountryId,
            Address = "test",
            ReceiveNewsLetters = false,
        };

        var personAddRequestList = new List<PersonAddRequest>()
        {
            personAddRequest1,
            personAddRequest2,
            personAddRequest3,
        };

        //打印输出
        _testOutputHelper.WriteLine("Expected");
        foreach (var personAddRequest in personAddRequestList)
        {
            _testOutputHelper.WriteLine(personAddRequest.ToString());
        }

        var personResponseList = personAddRequestList
            .Select(personAddRequest => _personsService.AddPerson(personAddRequest))
            .ToList();

        var getFilteredPerson = _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

        //打印输出
        _testOutputHelper.WriteLine("Actual");
        foreach (var personResponse in getFilteredPerson)
        {
            _testOutputHelper.WriteLine(personResponse.ToString());
        }

        foreach (var personResponse in personResponseList)
        {
            Assert.Contains(personResponse, getFilteredPerson);
        }
    }

    //searchTest is "PersonName" or empty -> return allPersonResponse
    [Fact]
    public void GetFilteredPerson_SearchByPersonName()
    {
        var countryAddRequest1 = new CountryAddRequest() { CountryName = "China" };
        var countryAddRequest2 = new CountryAddRequest() { CountryName = "Russia" };
        var countryAddRequest3 = new CountryAddRequest() { CountryName = "India" };

        var countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
        var countryResponse2 = _countriesService.AddCountry(countryAddRequest2);
        var countryResponse3 = _countriesService.AddCountry(countryAddRequest3);

        var personAddRequest1 = new PersonAddRequest()
        {
            PersonName = "test1",
            Email = "test1@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse1.CountryId,
            Address = "test",
            ReceiveNewsLetters = false,
        };

        var personAddRequest2 = new PersonAddRequest()
        {
            PersonName = "test2",
            Email = "test2@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse2.CountryId,
            Address = "test",
            ReceiveNewsLetters = false,
        };

        var personAddRequest3 = new PersonAddRequest()
        {
            PersonName = "test3",
            Email = "test3@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse3.CountryId,
            Address = "test",
            ReceiveNewsLetters = false,
        };

        var personAddRequestList = new List<PersonAddRequest>()
        {
            personAddRequest1,
            personAddRequest2,
            personAddRequest3,
        };

        //打印输出
        _testOutputHelper.WriteLine("Expected");
        foreach (var personAddRequest in personAddRequestList)
        {
            _testOutputHelper.WriteLine(personAddRequest.ToString());
        }

        var personResponseList = personAddRequestList
            .Select(personAddRequest => _personsService.AddPerson(personAddRequest))
            .ToList();

        var getFilteredPerson = _personsService.GetFilteredPersons(
            nameof(Person.PersonName),
            "test2"
        );

        //打印输出
        _testOutputHelper.WriteLine("Actual");
        foreach (var personResponse in getFilteredPerson)
        {
            _testOutputHelper.WriteLine(personResponse.ToString());
        }

        foreach (var personResponse in personResponseList)
        {
            if (
                personResponse.PersonName?.Contains("test2", StringComparison.OrdinalIgnoreCase)
                == true
            )
            {
                Assert.Contains(personResponse, getFilteredPerson);
            }
        }
    }
    #endregion

    #region GetSortesPersons
    //DESC 排序 -> 排序完成的list
    [Fact]
    public void GetSortPersons_SortByPersonName()
    {
        var countryAddRequest1 = new CountryAddRequest() { CountryName = "China" };
        var countryAddRequest2 = new CountryAddRequest() { CountryName = "Russia" };
        var countryAddRequest3 = new CountryAddRequest() { CountryName = "India" };

        var countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
        var countryResponse2 = _countriesService.AddCountry(countryAddRequest2);
        var countryResponse3 = _countriesService.AddCountry(countryAddRequest3);

        var personAddRequest1 = new PersonAddRequest()
        {
            PersonName = "test1",
            Email = "test1@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse1.CountryId,
            Address = "China",
            ReceiveNewsLetters = false,
        };

        var personAddRequest2 = new PersonAddRequest()
        {
            PersonName = "test2",
            Email = "test2@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse2.CountryId,
            Address = "Russia",
            ReceiveNewsLetters = false,
        };

        var personAddRequest3 = new PersonAddRequest()
        {
            PersonName = "test3",
            Email = "test3@gmail.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            Gender = GenderOptions.Male,
            CountryId = countryResponse3.CountryId,
            Address = "India",
            ReceiveNewsLetters = false,
        };

        var personAddRequestList = new List<PersonAddRequest>()
        {
            personAddRequest1,
            personAddRequest2,
            personAddRequest3,
        };

        var personResponseList = personAddRequestList
            .Select(personAddRequest => _personsService.AddPerson(personAddRequest))
            .ToList();

        var allPersonResponseList = _personsService.GetAllPersons();

        var getSortPersonResponses = _personsService.GetSortPersons(
            allPersonResponseList,
            nameof(Person.PersonName),
            SortOrderOptions.DESC
        );

        //打印输出
        _testOutputHelper.WriteLine("Expected");
        foreach (var personResponse in allPersonResponseList)
        {
            _testOutputHelper.WriteLine(personResponse.ToString());
        }

        var orderByDescending = allPersonResponseList.OrderByDescending(temp => temp.PersonName);

        //打印输出
        _testOutputHelper.WriteLine("Actual");
        foreach (var personResponse in orderByDescending)
        {
            _testOutputHelper.WriteLine(personResponse.ToString());
        }

        for (var i = 0; i < personAddRequestList.Count; i++)
        {
            Assert.Equal(
                orderByDescending.ElementAt(i).PersonName,
                getSortPersonResponses.ElementAt(i).PersonName
            );
        }
    }

    #endregion
}
