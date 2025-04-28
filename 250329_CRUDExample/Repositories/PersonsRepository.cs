using System.Linq.Expressions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;

namespace Repositories;

public class PersonsRepository(ApplicationDbContext db, ILogger<PersonsRepository> logger)
    : IPersonsRepository
{
    public async Task<Person> AddPerson(Person person)
    {
        logger.Log(LogLevel.Information, "库调用: PersonsRepository -> AddPerson");

        await db.Persons.AddAsync(person);
        await db.SaveChangesAsync();
        return person;
    }

    public async Task<List<Person>> GetAllPersons()
    {
        logger.Log(LogLevel.Information, "库调用: PersonsRepository -> GetAllPersons");
        return await db.Persons.Include(p => p.Country).ToListAsync();
    }

    public async Task<Person?> GetPersonByPersonId(Guid personId)
    {
        logger.Log(LogLevel.Information, "库调用: PersonsRepository -> GetPersonByPersonId");
        return await db
            .Persons.Include(p => p.Country)
            .FirstOrDefaultAsync(p => p.PersonId == personId);
    }

    //Expression - 解析表达式树，不直接执行  数据库查询、动态分析  低内存消耗（数据库层过滤）
    public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
    {
        logger.Log(LogLevel.Information, "库调用: PersonsRepository -> GetFilteredPersons");
        return await db.Persons.Include(p => p.Country).Where(predicate).ToListAsync();
    }

    public async Task<bool> DeletePersonByPersonId(Guid personId)
    {
        logger.Log(LogLevel.Information, "库调用: PersonsRepository -> DeletePersonByPersonId");

        db.Persons.RemoveRange(db.Persons.Where(p => p.PersonId == personId));
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<Person> UpdatePerson(Person person)
    {
        logger.Log(LogLevel.Information, "库调用: PersonsRepository -> UpdatePerson");

        var matchingPerson = await db.Persons.FirstOrDefaultAsync(p =>
            p.PersonId == person.PersonId
        );
        if (matchingPerson == null)
        {
            return person;
        }

        matchingPerson.PersonName = person.PersonName;
        matchingPerson.Email = person.Email;
        matchingPerson.DateOfBirth = person.DateOfBirth;
        matchingPerson.Gender = person.Gender;
        matchingPerson.CountryId = person.CountryId;
        matchingPerson.Address = person.Address;
        matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

        await db.SaveChangesAsync();
        return matchingPerson;
    }
}
