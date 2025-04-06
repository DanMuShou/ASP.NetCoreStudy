using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
/// Person 原 模型
/// </summary>
public class Person
{
    [Key]
    public Guid PersonId { get; set; }

    //nvarchar      使用 varchar 更节省空间    使用 nvarchar 可以避免字符编码问题
    [StringLength(40)]
    public string? PersonName { get; set; }

    [StringLength(40)]
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }

    [StringLength(10)]
    public string? Gender { get; set; }

    //uniqueidentifier
    public Guid? CountryId { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }
    public bool ReceiveNewsLetters { get; set; }
}
