using ServiceManagement.Models;

namespace ServiceManagement.StateStore;

public class ContainerStorage
{
    private Server _server = new Server();

    public Server GetServer() => _server;

    public void SetServer(Server server) => _server = server;
}
