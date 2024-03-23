namespace ProjectHermes.Xipona.Api.ArchitectureTests.Core;

public class CoreTests
{
    private readonly CoreFixture _fixture;

    public CoreTests()
    {
        _fixture = new CoreFixture();
    }

    [Fact]
    public void ProjectDependencies()
    {
        var result = Types
            .InAssembly(_fixture.Assembly)
            .Should()
            .OnlyHaveDependenciesOn(
                "ProjectHermes.Xipona.Api.Core",
                "System",
                "Microsoft")
            .GetResult()
            .IsSuccessful;

        result.Should().BeTrue();
    }

    private sealed class CoreFixture
    {
        public Assembly Assembly { get; } = Assembly.Load(new AssemblyName("Xipona.Api.Core"));
    }
}