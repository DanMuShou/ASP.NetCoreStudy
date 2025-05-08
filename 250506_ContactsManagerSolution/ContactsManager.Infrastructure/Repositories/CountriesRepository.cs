using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContract;
using ContactsManager.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace ContactsManager.Infrastructure.Repositories;

public class CountriesRepository(ApplicationDbContext db) : ICountriesRepository
{
    public async Task<Country> AddCountry(Country country)
    {
        await db.Countries.AddAsync(country);
        await db.SaveChangesAsync();
        return country;
    }

    public async Task<List<Country>> GetAllCountries() => await db.Countries.ToListAsync();

    public async Task<Country?> GetCountryById(Guid countryId) =>
        await db.Countries.FirstOrDefaultAsync(c => c.CountryId == countryId);

    public async Task<Country?> GetCountryByCountryName(string countryName) =>
        await db.Countries.FirstOrDefaultAsync(c => c.CountryName == countryName);
}
