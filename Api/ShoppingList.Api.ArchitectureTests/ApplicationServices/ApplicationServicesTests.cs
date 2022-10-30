using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ArchitectureTests.ApplicationServices.CustomRules;

namespace ProjectHermes.ShoppingList.Api.ArchitectureTests.ApplicationServices;

public class ApplicationServicesTests
{
    private readonly ApplicationServicesFixture _fixture;

    public ApplicationServicesTests()
    {
        _fixture = new ApplicationServicesFixture();
    }

    [Fact]
    public void CommandHandlerNames()
    {
        var result = Types
            .InAssembly(_fixture.Assembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .HaveNameEndingWith("CommandHandler")
            .GetResult()
            .IsSuccessful;

        result.Should().BeTrue();
    }

    [Fact]
    public void CommandNames()
    {
        var result = Types
            .InAssembly(_fixture.Assembly)
            .That()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult()
            .IsSuccessful;

        result.Should().BeTrue();
    }

    [Fact]
    public void CommandHandlerForCommand()
    {
        var rule = new MatchingCommandHandlerForCommandRule();

        var result = Types
            .InAssembly(_fixture.Assembly)
            .That()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .MeetCustomRule(rule)
            .GetResult()
            .IsSuccessful;

        result.Should().BeTrue();
    }

    [Fact]
    public void CommandForCommandHandler()
    {
        var rule = new MatchingCommandForCommandHandlerRule();

        var result = Types
            .InAssembly(_fixture.Assembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .Should()
            .MeetCustomRule(rule)
            .GetResult()
            .IsSuccessful;

        result.Should().BeTrue();
    }

    private sealed class ApplicationServicesFixture
    {
        public Assembly Assembly { get; } = Assembly.Load(new AssemblyName("ShoppingList.Api.ApplicationServices"));
    }
}