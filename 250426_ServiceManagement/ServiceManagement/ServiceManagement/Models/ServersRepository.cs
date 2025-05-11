using Microsoft.EntityFrameworkCore;
using ServiceManagement.Data;

namespace ServiceManagement.Models;

public class ServersRepository(IDbContextFactory<ServerManagementContext> contextFactory)
    : IServersRepository
{
    #region SaveChange

    public async Task AddServer(Server server)
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        await db.Servers.AddAsync(server);
        await db.SaveChangesAsync();
    }

    public async Task<Server> UpdateServer(Server updatedServer)
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        var server = await db.Servers.FirstOrDefaultAsync(s =>
            s.ServerId == updatedServer.ServerId
        );

        if (server == null)
            return updatedServer;

        server.ServerId = updatedServer.ServerId;
        server.Name = updatedServer.Name;
        server.City = updatedServer.City;
        server.IsOnline = updatedServer.IsOnline;
        await db.SaveChangesAsync();

        return server;
    }

    public async Task<bool> DeleteServerByServerId(int serverId)
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        db.Servers.RemoveRange(db.Servers.Where(s => s.ServerId == serverId));
        return await db.SaveChangesAsync() > 0;
    }

    #endregion

    #region GetAndNotSave

    public async Task<List<Server>> GetAllServers()
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        var servers = await db.Servers.ToListAsync();
        return servers;
    }

    public async Task<List<Server>> GetServersByCityName(string cityName)
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        var servers = await db
            .Servers.Where(s => s.City != null && s.City.ToLower().Equals(cityName.ToLower()))
            .ToListAsync();
        return servers;
    }

    public async Task<Server?> GetServerByServerId(int id)
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        var server = await db.Servers.FirstOrDefaultAsync(s => s.ServerId == id);
        return server;
    }

    public async Task<List<Server>> GetSearchServers(string serverFilter)
    {
        await using var db = await contextFactory.CreateDbContextAsync();
        var servers = await db
            .Servers.Where(s => s.Name != null && s.Name.ToLower().Contains(serverFilter.ToLower()))
            .ToListAsync();
        return servers;
    }

    #endregion
}
