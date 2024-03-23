namespace ProjectHermes.Xipona.Api.ArchitectureTests.Domain;

public class DomainTests
{
    private readonly DomainFixture _fixture;

    public DomainTests()
    {
        _fixture = new DomainFixture();
    }

    [Fact]
    public void ProjectDependencies()
    {
        var result = Types
            .InAssembly(_fixture.Assembly)
            .Should()
            .OnlyHaveDependenciesOn(
                "ProjectHermes.Xipona.Api.Core",
                "ProjectHermes.Xipona.Api.Domain",
                "System",
                "Microsoft")
            .GetResult()
            .IsSuccessful;

        result.Should().BeTrue();
    }

    private sealed class DomainFixture
    {
        public Assembly Assembly { get; } = Assembly.Load(new AssemblyName("Xipona.Api.Domain"));
    }
}