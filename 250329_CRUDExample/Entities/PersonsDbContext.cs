using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class PersonsDbContext : DbContext
{
    public PersonsDbContext(DbContextOptions options)
        : base(options) { }

    //一个db对应一个表
    public DbSet<Country> Countries { get; set; }
    public DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().ToTable("T_Countries");
        modelBuilder.Entity<Person>().ToTable("T_Persons");

        // var countriesJson = File.ReadAllText("countries.json");
        // var countryFromJsonList = JsonSerializer.Deserialize<List<Country>>(countriesJson);
        //
        // if (countryFromJsonList != null)
        //     foreach (var country in countryFromJsonList)
        //         modelBuilder.Entity<Country>().HasData(country);
        //
        // var personsJson = File.ReadAllText("persons.json");
        // var personFromJsonList = JsonSerializer.Deserialize<List<Person>>(personsJson);
        // if (personFromJsonList != null)
        //     foreach (var person in personFromJsonList)
        //         modelBuilder.Entity<Person>().HasData(person);
    }
}
