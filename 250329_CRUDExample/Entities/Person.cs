using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public string? TIN { get; set; }

    [ForeignKey("CountryId")]
    public virtual Country? Country { get; set; }

    public override string ToString()
    {
        return $"人员Id {PersonId}, 人员名称 {PersonName}, "
            + $"邮箱 {Email}, 生日 {DateOfBirth}, "
            + $"性别 {Gender}, 国家Id {CountryId}, "
            + $"国家名称 {Country?.CountryName}, 地址 {Address}, "
            + $"收到新闻 {ReceiveNewsLetters}";
    }
}
