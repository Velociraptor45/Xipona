namespace ProjectHermes.Xipona.Api.ArchitectureTests.Contracts;

public class ContractsTests
{
    private readonly ContractsFixture _fixture;

    public ContractsTests()
    {
        _fixture = new ContractsFixture();
    }

    [Fact]
    public void ProjectDependencies()
    {
        var result = Types
            .InAssembly(_fixture.Assembly)
            .Should()
            .OnlyHaveDependenciesOn(
                "ProjectHermes.Xipona.Api.Contracts",
                "System")
            .GetResult()
            .IsSuccessful;

        result.Should().BeTrue();
    }

    private sealed class ContractsFixture
    {
        public Assembly Assembly { get; } = Assembly.Load(new AssemblyName("Xipona.Api.Contracts"));
    }
}