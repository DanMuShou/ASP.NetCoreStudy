using ContactsManager.Core.Domain.RepositoryContract;
using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts;

namespace ContactsManager.Core.Services;

public class CountriesGetterService(ICountriesRepository countriesRepository) : ICountriesGetterService
{
    
    public async Task<List<CountryResponse>> GetAllCountries()
    {
        var getAllCountryList = await countriesRepository.GetAllCountries();
        return getAllCountryList.Select(country => country.ToCountryResponse()).ToList();
    }

    public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
    {
        if (countryId == null)
            return null;
        var country = await countriesRepository.GetCountryById(countryId.Value);
        return country?.ToCountryResponse();
    }


}