using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;

    public CountriesServiceTest()
    {
        _countriesService = new CountriesService(
            new PersonsDbContext(new DbContextOptions<PersonsDbContext>())
        );
    }

    #region AddCountries
    //提供countryAddRequest null 抛出异常ArgumentNullException(参数null)
    [Fact]
    public async Task AddCountry_NullCountry()
    {
        //arrange
        CountryAddRequest? request = null;
        //assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            //act
            await _countriesService.AddCountry(request);
        });
    }

    //CountryAddRequest.CountryName = null --> 抛出异常ArgumentException(参数异常)
    [Fact]
    public async Task AddCountry_NullCountryName()
    {
        //arrange
        var request = new CountryAddRequest() { CountryName = null };
        //assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            //act
            await _countriesService.AddCountry(request);
        });
    }

    //CountryAddRequest.CountryName is duplicate --> 抛出异常ArgumentException(参数重复)
    [Fact]
    public void AddCountry_DuplicateCountryName()
    {
        //arrange
        var request1 = new CountryAddRequest() { CountryName = "China" };
        var request2 = new CountryAddRequest() { CountryName = "China" };
        //assert
        Assert.Throws<ArgumentException>(() =>
        {
            //act
            _countriesService.AddCountry(request1);
            _countriesService.AddCountry(request2);
        });
    }

    //CountryAddRequest合格  --> 添加到列表 添加成功
    [Fact]
    public async Task AddCountry_Success()
    {
        //arrange
        var request1 = new CountryAddRequest() { CountryName = "China" };
        var request2 = new CountryAddRequest() { CountryName = "Russia" };
        //act
        var response1 = await _countriesService.AddCountry(request1);
        var response2 = await _countriesService.AddCountry(request2);
        var allResponse = await _countriesService.GetAllCountries();
        //assert
        Assert.True(response1.CountryId != Guid.Empty);
        Assert.True(response2.CountryId != Guid.Empty);
        //调用的Equals方法比较 Equals只比较内容(2 != 1.0) 指针
        Assert.Contains(response1, allResponse);
        Assert.Contains(response2, allResponse);
    }
    #endregion

    #region GetAllCountries
    [Fact]
    public async Task GetAllCountries_EmptyList()
    {
        //arrange
        //null

        //act
        var actualCountryList = await _countriesService.GetAllCountries();
        //assert
        Assert.Empty(actualCountryList);
    }

    [Fact]
    public async Task GetAllCountries_AddFewCountries()
    {
        //arrange
        var actualCountryList = new List<CountryAddRequest>()
        {
            new CountryAddRequest() { CountryName = "China" },
            new CountryAddRequest() { CountryName = "Russia" },
            new CountryAddRequest() { CountryName = "USA" },
            new CountryAddRequest() { CountryName = "UK" },
        };
        //act
        var countriesListFromAddCountry = actualCountryList
            .Select(async country => await _countriesService.AddCountry(country))
            .Select(task => task?.Result)
            .ToList();

        var actualCountryResponseList = await _countriesService.GetAllCountries();
        //assert
        foreach (var expectedCountry in countriesListFromAddCountry)
        {
            Assert.Contains(expectedCountry, actualCountryResponseList);
        }
    }
    #endregion

    #region GetCountryByCountryID
    [Fact]
    //提供null guid 返回null response
    public void GetCountryByCountryID_NullId()
    {
        //arrange
        Guid? id = null;

        //act
        var actualResponse = _countriesService.GetCountryByCountryId(id);
        //assert
        Assert.Null(actualResponse);
    }

    [Fact]
    //提供guid 返回对应的 response
    public async Task GetCountryByCountryID_AddFewCountries()
    {
        //arrange
        var actualCountryList = new List<CountryAddRequest>()
        {
            new CountryAddRequest() { CountryName = "China" },
            new CountryAddRequest() { CountryName = "Russia" },
            new CountryAddRequest() { CountryName = "USA" },
            new CountryAddRequest() { CountryName = "UK" },
        };
        //act
        var countriesListFromAddCountry = actualCountryList
            .Select(async country => await _countriesService.AddCountry(country))
            .Select(task => task?.Result)
            .ToList();

        var countriesIds = countriesListFromAddCountry.Select(country => country?.CountryId);

        foreach (var countryId in countriesIds)
        {
            var actualResponse = await _countriesService.GetCountryByCountryId(countryId);
            Assert.NotNull(actualResponse);
            Assert.Equal(countryId, actualResponse.CountryId);
        }
    }
    #endregion
}
