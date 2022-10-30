namespace ProjectHermes.ShoppingList.Api.ArchitectureTests.Client;

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
                "ProjectHermes.ShoppingList.Api.Client",
                "ProjectHermes.ShoppingList.Api.Contracts",
                "System",
                "Newtonsoft",
                "RestEase")
            .GetResult()
            .IsSuccessful;

        result.Should().BeTrue();
    }

    private sealed class ClientFixture
    {
        public Assembly Assembly { get; } = Assembly.Load(new AssemblyName("ShoppingList.Api.Client"));
    }
}