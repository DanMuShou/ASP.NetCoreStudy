using System.ComponentModel.DataAnnotations;

namespace Entities;

/// <summary>
/// 文档注释 --
/// 国家域模型
/// </summary>
public class Country
{
    [Key]
    public Guid CountryId { get; set; }
    public string? CountryName { get; set; }
    public virtual ICollection<Person>? Persons { get; set; } = [];
}
