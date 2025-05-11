using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.DTO;

public class RegisterDto
{
    [Required(ErrorMessage = "名称不能为空")]
    public required string PersonName { get; init; }

    [Required(ErrorMessage = "邮箱不能为空")]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; init; }

    [Required(ErrorMessage = "电话不能为空")]
    [Phone(ErrorMessage = "电话格式不正确")]
    [DataType(DataType.PhoneNumber)]
    public required string Phone { get; init; }

    [Required(ErrorMessage = "密码不能为空")]
    [RegularExpression(
        @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$",
        ErrorMessage = "密码必须至少8位，包含字母和数字"
    )]
    [DataType(DataType.Password)]
    public required string Password { get; init; }

    [Required(ErrorMessage = "确认密码不能为空")]
    [Compare("Password", ErrorMessage = "两次输入的密码不一致")]
    [DataType(DataType.Password)]
    public required string ConfirmPassword { get; init; }
}
