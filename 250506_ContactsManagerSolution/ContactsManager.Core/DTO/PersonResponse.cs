using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Enums;

namespace ContactsManager.Core.DTO;

/// <summary>
///DTO 作为大多数 PersonService 的返回类型
/// </summary>
public class PersonResponse
{
    public Guid PersonId { get; set; }
    public string? PersonName { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public Guid? CountryId { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
    public string? CountryName { get; set; }
    public double? Age { get; set; }

    /// <summary>
    /// 比较两个 PersonResponse 是否相等
    /// </summary>
    /// <param name="obj">要比较的PersonResponse对象</param>
    /// <returns>>表示所有人员详细信息是否与指定的参数对象匹配</returns>
    public override bool Equals(object? obj)
    {
        if (obj is PersonResponse personResponse)
        {
            return PersonId == personResponse.PersonId
                && PersonName == personResponse.PersonName
                && Email == personResponse.Email
                && DateOfBirth == personResponse.DateOfBirth
                && Gender == personResponse.Gender
                && CountryId == personResponse.CountryId
                && Address == personResponse.Address
                && ReceiveNewsLetters == personResponse.ReceiveNewsLetters
                && CountryName == personResponse.CountryName
                && Age == personResponse.Age;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"PersonId:{PersonId},"
            + $"PersonName:{PersonName},"
            + $"Email:{Email},"
            + $"DateOfBirth:{DateOfBirth},"
            + $"Gender:{Gender},"
            + $"CountryId:{CountryId},"
            + $"Address:{Address},"
            + $"ReceiveNewsLetters:{ReceiveNewsLetters},"
            + $"CountryName:{CountryName},"
            + $"Age:{Age}";
    }
}

/// <summary>
/// 转换Person类到PersonResponse类
/// </summary>
public static class PersonResponseExtensions
{
    public static PersonResponse ToPersonResponse(this Person person)
    {
        return new PersonResponse
        {
            PersonId = person.PersonId,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Gender = person.Gender,
            CountryId = person.CountryId,
            Address = person.Address,
            Age =
                (person.DateOfBirth == null)
                    ? null
                    : Math.Round((DateTime.Now - person.DateOfBirth).Value.TotalDays / 365.25),
            ReceiveNewsLetters = person.ReceiveNewsLetters,
            CountryName = person.Country?.CountryName,
        };
    }

    public static PersonUpdateRequest ToPersonUpdateRequest(this PersonResponse personResponse)
    {
        return new PersonUpdateRequest
        {
            PersonId = personResponse.PersonId,
            PersonName = personResponse.PersonName,
            Email = personResponse.Email,
            DateOfBirth = personResponse.DateOfBirth,
            Gender = (GenderOptions)
                Enum.Parse(typeof(GenderOptions), personResponse.Gender ?? string.Empty, true),
            CountryId = personResponse.CountryId,
            Address = personResponse.Address,
            ReceiveNewsLetters = personResponse.ReceiveNewsLetters,
        };
    }
}
