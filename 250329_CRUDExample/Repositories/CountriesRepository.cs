using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories;

public class CountriesRepository : ICountriesRepository
{
    private readonly ApplicationDbContext _db;

    public CountriesRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Country> AddCountry(Country country)
    {
        await _db.Countries.AddAsync(country);
        await _db.SaveChangesAsync();
        return country;
    }

    public async Task<List<Country>> GetAllCountries() => await _db.Countries.ToListAsync();

    public async Task<Country?> GetCountryById(Guid countryId) =>
        await _db.Countries.FirstOrDefaultAsync(c => c.CountryId == countryId);

    public async Task<Country?> GetCountryByCountryName(string countryName) =>
        await _db.Countries.FirstOrDefaultAsync(c => c.CountryName == countryName);
}
