using Microsoft.EntityFrameworkCore;
using ServiceManagement.Models;

namespace ServiceManagement.Data;

public class ServerManagementContext(DbContextOptions<ServerManagementContext> options)
    : DbContext(options)
{
    public DbSet<Server> Servers { get; set; }

    private readonly List<Server> _hasServerData =
    [
        new()
        {
            ServerId = 2,
            Name = "Server2",
            City = "Toronto",
        },
        new()
        {
            ServerId = 1,
            Name = "Server1",
            City = "Toronto",
        },
        new()
        {
            ServerId = 3,
            Name = "Server3",
            City = "Toronto",
        },
        new()
        {
            ServerId = 4,
            Name = "Server4",
            City = "Toronto",
        },
        new()
        {
            ServerId = 5,
            Name = "Server5",
            City = "Montreal",
        },
        new()
        {
            ServerId = 6,
            Name = "Server6",
            City = "Montreal",
        },
        new()
        {
            ServerId = 7,
            Name = "Server7",
            City = "Montreal",
        },
        new()
        {
            ServerId = 8,
            Name = "Server8",
            City = "Ottawa",
        },
        new()
        {
            ServerId = 9,
            Name = "Server9",
            City = "Ottawa",
        },
        new()
        {
            ServerId = 10,
            Name = "Server10",
            City = "Calgary",
        },
        new()
        {
            ServerId = 11,
            Name = "Server11",
            City = "Calgary",
        },
        new()
        {
            ServerId = 12,
            Name = "Server12",
            City = "Halifax",
        },
        new()
        {
            ServerId = 13,
            Name = "Server13",
            City = "Halifax",
        },
    ];

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Server>().ToTable("T_Servers").HasData(_hasServerData);
    }
}
