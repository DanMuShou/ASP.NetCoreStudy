using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.DTO.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// Acts as a DTO for 插入新人
/// </summary>
//更擅长数据库请求格式
public class PersonAddRequest
{
    [Required(ErrorMessage = "名称不能为空")]
    public string? PersonName { get; set; }

    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "出生日期不能为空")]
    public DateTime? DateOfBirth { get; set; }
    public GenderOptions? Gender { get; set; }
    public Guid? CountryId { get; set; }
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
}

/// <summary>
/// 转换PersonAddRequest为Person类型
/// </summary>
public static class PersonAddRequestExtensions
{
    public static Person ToPerson(this PersonAddRequest personAddRequest)
    {
        return new Person
        {
            PersonName = personAddRequest.PersonName,
            Email = personAddRequest.Email,
            DateOfBirth = personAddRequest.DateOfBirth,
            Gender = personAddRequest.Gender?.ToString(),
            CountryId = personAddRequest.CountryId,
            Address = personAddRequest.Address,
            ReceiveNewsLetters = personAddRequest.ReceiveNewsLetters,
        };
    }
}
