using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using System;
using System.IO;
using Xipona.Api.Endpoint.IntegrationTests;
using Xunit;

[assembly: AssemblyFixture(typeof(DockerFixture))]
namespace Xipona.Api.Endpoint.IntegrationTests;

public sealed class DockerFixture : IDisposable
{
    private readonly ICompositeService _container;

    public DockerFixture()
    {
        var fileDir = Path.Combine(Directory.GetCurrentDirectory(), "docker-compose.yml");
        _container = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(fileDir)
            .RemoveOrphans()
            .Build()
            .Start();

        // wait for DB to initialize
        Task.Delay(3000).GetAwaiter().GetResult();
    }

    public const string ConnectionStringWithoutDb =
        "server=127.0.0.1;port=15906;userid=postgres;password=123root";

    public const string ConnectionString =
        "server=127.0.0.1;port=15906;database={DatabaseName};userid=postgres;password=123root";

    public const string DatabaseName = "test-shoppinglist";

    public void Dispose()
    {
        _container.Dispose();
    }
}