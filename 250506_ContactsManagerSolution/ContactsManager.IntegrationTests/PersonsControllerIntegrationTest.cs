using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
namespace ContactsManager.IntegrationTests;

// xUnit 的 IClassFixture 特性，确保测试类共享同一个 CustomWebApplicationFactory 实例。
public class PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    //xUnit 自动注入 CustomWebApplicationFactory，确保测试环境与主应用配置一致（如替换数据库、环境变量等）。

    #region Home

    [Fact]
    public async Task H_ToReturnView()
    {
        var httpResponseMessage = await _client.GetAsync("Person/Home");
        httpResponseMessage.Should().Be200Ok();

        var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
        var html = new HtmlDocument();
        html.LoadHtml(responseBody);
        var document = html.DocumentNode;

        document.QuerySelectorAll("table.persons").Should().NotBeNull();
    }

    #endregion
}
