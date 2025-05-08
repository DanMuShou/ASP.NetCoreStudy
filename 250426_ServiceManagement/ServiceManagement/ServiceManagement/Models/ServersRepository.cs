namespace ServiceManagement.Models;

public class ServersRepository
{
    private static readonly List<Server> Servers =
    [
        new Server
        {
            ServerId = 2,
            Name = "Server2",
            City = "Toronto",
        },
        new Server
        {
            ServerId = 1,
            Name = "Server1",
            City = "Toronto",
        },
        new Server
        {
            ServerId = 3,
            Name = "Server3",
            City = "Toronto",
        },
        new Server
        {
            ServerId = 4,
            Name = "Server4",
            City = "Toronto",
        },
        new Server
        {
            ServerId = 5,
            Name = "Server5",
            City = "Montreal",
        },
        new Server
        {
            ServerId = 6,
            Name = "Server6",
            City = "Montreal",
        },
        new Server
        {
            ServerId = 7,
            Name = "Server7",
            City = "Montreal",
        },
        new Server
        {
            ServerId = 8,
            Name = "Server8",
            City = "Ottawa",
        },
        new Server
        {
            ServerId = 9,
            Name = "Server9",
            City = "Ottawa",
        },
        new Server
        {
            ServerId = 10,
            Name = "Server10",
            City = "Calgary",
        },
        new Server
        {
            ServerId = 11,
            Name = "Server11",
            City = "Calgary",
        },
        new Server
        {
            ServerId = 12,
            Name = "Server12",
            City = "Halifax",
        },
        new Server
        {
            ServerId = 13,
            Name = "Server13",
            City = "Halifax",
        },
    ];

    public static void AddServer(Server context)
    {
        var maxId = Servers.Max(s => s.ServerId);
        context.ServerId = maxId + 1;
        Servers.Add(context);
    }

    public static List<Server> GetServer() => Servers;

    public static List<Server> GetServersByCity(string cityName)
    {
        return Servers
            .Where(s =>
                s.City != null && s.City.Equals(cityName, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();
    }

    public static Server? GetServerById(int id)
    {
        var server = Servers.FirstOrDefault(s => s.ServerId == id);
        if (server != null)
        {
            return new Server
            {
                ServerId = server.ServerId,
                Name = server.Name,
                City = server.City,
                IsOnline = server.IsOnline,
            };
        }

        return null;
    }

    public static void UpdateServer(int serverId, Server context)
    {
        if (serverId != context.ServerId)
            return;

        var serverToUpdate = Servers.FirstOrDefault(s => s.ServerId == serverId);
        if (serverToUpdate != null)
        {
            serverToUpdate.IsOnline = context.IsOnline;
            serverToUpdate.Name = context.Name;
            serverToUpdate.City = context.City;
        }
    }

    public static void DeleteServer(int serverId)
    {
        var server = Servers.FirstOrDefault(s => s.ServerId == serverId);
        if (server != null)
        {
            Servers.Remove(server);
        }
    }

    public static List<Server> SearchServers(string serverFilter)
    {
        return Servers
            .Where(s =>
                s.Name != null && s.Name.Contains(serverFilter, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();
    }
}
