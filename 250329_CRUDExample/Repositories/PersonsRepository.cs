using System.Linq.Expressions;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Repositories;

public class PersonsRepository : IPersonsRepository
{
    private readonly ApplicationDbContext _db;

    public PersonsRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Person> AddPerson(Person person)
    {
        await _db.Persons.AddAsync(person);
        await _db.SaveChangesAsync();
        return person;
    }

    public async Task<List<Person>> GetAllPersons() =>
        await _db.Persons.Include(p => p.Country).ToListAsync();

    public async Task<Person?> GetPersonByPersonId(Guid personId) =>
        await _db.Persons.Include(p => p.Country).FirstOrDefaultAsync(p => p.PersonId == personId);

    //Expression - 解析表达式树，不直接执行  数据库查询、动态分析  低内存消耗（数据库层过滤）
    public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate) =>
        await _db.Persons.Include(p => p.Country).Where(predicate).ToListAsync();

    public async Task<bool> DeletePersonByPersonId(Guid personId)
    {
        _db.Persons.RemoveRange(_db.Persons.Where(p => p.PersonId == personId));
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<Person> UpdatePerson(Person person)
    {
        var matchingPerson = await _db.Persons.FirstOrDefaultAsync(p =>
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

        var countUpdated = await _db.SaveChangesAsync();
        return matchingPerson;
    }
}
