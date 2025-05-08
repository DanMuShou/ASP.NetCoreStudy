using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ServiceManagement.Models;

namespace ServiceManagement.StateStore;

//ProtectedSessionStorage 实例，封装了对浏览器会话存储（session storage）的操作，使上层代码无需直接处理底层存储细节。
public class SessionStorage(ProtectedSessionStorage protectedSessionStorage)
{
    public async Task<Server?> GetServerAsync()
    {
        var result = await protectedSessionStorage.GetAsync<Server>("server");
        return result.Success ? result.Value : null;
    }

    public async Task SetServerAsync(Server? server)
    {
        if (server != null)
            await protectedSessionStorage.SetAsync("server", server);
    }
}
