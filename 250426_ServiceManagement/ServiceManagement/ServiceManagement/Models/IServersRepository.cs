namespace ServiceManagement.Models;

public interface IServersRepository
{
    public Task AddServer(Server server);
    public Task<Server> UpdateServer(Server updatedServer);
    public Task<bool> DeleteServerByServerId(int serverId);
    public Task<List<Server>> GetAllServers();
    public Task<List<Server>> GetServersByCityName(string cityName);
    public Task<Server?> GetServerByServerId(int id);
    public Task<List<Server>> GetSearchServers(string serverFilter);
}
