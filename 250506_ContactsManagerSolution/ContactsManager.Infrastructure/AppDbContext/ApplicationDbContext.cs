using System.Text.Json;
using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactsManager.Infrastructure.AppDbContext;

//DbContext EF Core 的核心基类，提供与数据库交互的基础功能（如查询、保存实体等
//IdentityDbContext<> 继承自 DbContext，是专门用于支持 ASP.NET Core Identity 功能的上下文类
public class ApplicationDbContext(DbContextOptions options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    //一个db对应一个表
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().ToTable("T_Countries");
        modelBuilder
            .Entity<Person>()
            .ToTable(
                "T_Persons",
                tb =>
                {
                    //将检查约束配置移到ToTable的配置委托中，利用HasCheckConstraint扩展方法
                    tb.HasCheckConstraint("CHK_TIN", "LEN([TaxIdentificationNumber]) = 8");
                }
            );

        var countriesJson = File.ReadAllText("countries.json");
        var countryFromJsonList = JsonSerializer.Deserialize<List<Country>>(countriesJson);

        if (countryFromJsonList != null)
            foreach (var country in countryFromJsonList)
                modelBuilder.Entity<Country>().HasData(country);

        var personsJson = File.ReadAllText("persons.json");
        var personFromJsonList = JsonSerializer.Deserialize<List<Person>>(personsJson);
        if (personFromJsonList != null)
            foreach (var person in personFromJsonList)
                modelBuilder.Entity<Person>().HasData(person);

        modelBuilder
            .Entity<Person>()
            .Property(temp => temp.TIN)
            .HasColumnName("TaxIdentificationNumber") //列名
            .HasColumnType("varchar(8)") //列类型
            .HasDefaultValue("ABC00000");

        //定义数据库表的检查约束（Check Constraint），确保字段值满足特定条件。
        //EF Core已弃用直接在实体配置上使用HasCheckConstraint，需通过表配置（ToTable委托）统一管理约束。
        // modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "len([TIN]) = 8");
        // modelBuilder.Entity<Person>().HasIndex(temp => temp.TIN).IsUnique();

        // modelBuilder.Entity<Person>(entity =>
        // {
        //     entity
        //         .HasOne<Country>(p => p.Country)
        //         .WithMany(c => c.Persons)
        //         .HasForeignKey(p => p.CountryId);
        // });
    }
}
