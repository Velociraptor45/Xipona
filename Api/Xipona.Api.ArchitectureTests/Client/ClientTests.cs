namespace ProjectHermes.Xipona.Api.ArchitectureTests.Client;

public class ClientTests
{
    private readonly ClientFixture _fixture;

    public ClientTests()
    {
        _fixture = new ClientFixture();
    }

    [Fact]
    public void ProjectDependencies()
    {
        var result = Types
            .InAssembly(_fixture.Assembly)
            .Should()
            .OnlyHaveDependenciesOn(
                "ProjectHermes.Xipona.Api.Client",
                "ProjectHermes.Xipona.Api.Contracts",
                "System",
                "Newtonsoft",
                "RestEase")
            .GetResult()
            .IsSuccessful;

        result.Should().BeTrue();
    }

    private sealed class ClientFixture
    {
        public Assembly Assembly { get; } = Assembly.Load(new AssemblyName("Xipona.Api.Client"));
    }
}