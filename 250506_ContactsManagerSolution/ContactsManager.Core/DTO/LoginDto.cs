using System.ComponentModel.DataAnnotations;

namespace ContactsManager.Core.DTO;

public class LoginDto
{
    [Required(ErrorMessage = "邮箱地址不能为空")]
    [EmailAddress(ErrorMessage = "邮箱地址格式不正确")]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [Required(ErrorMessage = "密码不能为空")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}
