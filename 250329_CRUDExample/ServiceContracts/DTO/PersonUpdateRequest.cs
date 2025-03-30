using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.DTO.Enums;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO 更新人员信息请求
/// </summary>
public class PersonUpdateRequest
{
    [Required(ErrorMessage = "Person Id不能为空")]
    public Guid PersonId { get; set; }

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
/// 转换PersonUpdateRequest为Person类型
/// </summary>
public static class PersonUpdateRequestExtensions
{
    public static Person ToPerson(this PersonUpdateRequest personUpdateRequest)
    {
        return new Person
        {
            PersonId = personUpdateRequest.PersonId,
            PersonName = personUpdateRequest.PersonName,
            Email = personUpdateRequest.Email,
            DateOfBirth = personUpdateRequest.DateOfBirth,
            Gender = personUpdateRequest.Gender?.ToString(),
            CountryId = personUpdateRequest.CountryId,
            Address = personUpdateRequest.Address,
            ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters,
        };
    }
}
