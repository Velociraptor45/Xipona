using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ShoppingList.Api.Infrastructure.Tests.Common.Transactions;
using ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Common;

public abstract class CommandHandlerTestsBase<TCommandHandler, TCommand, TReturnType>
    where TCommandHandler : ICommandHandler<TCommand, TReturnType>
    where TCommand : ICommand<TReturnType>
{
    private readonly CommandHandlerBaseFixture _fixture;

    protected CommandHandlerTestsBase(CommandHandlerBaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldCallService()
    {
        // Arrange
        _fixture.Setup();
        _fixture.SetupTransaction();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Command);
        TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

        // Act
        await sut.HandleAsync(_fixture.Command, default);

        // Assert
        _fixture.VerifyCallingService();
    }

    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldCommitTransaction()
    {
        // Arrange
        _fixture.Setup();
        _fixture.SetupTransaction();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Command);
        TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

        // Act
        await sut.HandleAsync(_fixture.Command, default);

        // Assert
        _fixture.VerifyCommittingTransaction();
    }

    [Fact]
    public async Task HandleAsync_WithValidCommand_ShouldReturnExpectedResult()
    {
        // Arrange
        _fixture.Setup();
        _fixture.SetupTransaction();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Command);
        TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

        // Act
        var result = await sut.HandleAsync(_fixture.Command, default);

        // Assert
        result.Should().BeEquivalentTo(_fixture.ExpectedResult);
    }

    protected abstract class CommandHandlerBaseFixture
    {
        protected TransactionGeneratorMock TransactionGeneratorMock = new(MockBehavior.Strict);
        protected TransactionMock TransactionMock = new(MockBehavior.Strict);
        public abstract TCommand? Command { get; protected set; }
        public abstract TReturnType? ExpectedResult { get; protected set; }

        public abstract TCommandHandler CreateSut();

        public abstract void Setup();

        public void SetupTransaction()
        {
            TransactionGeneratorMock.SetupGenerateAsync(TransactionMock.Object);
            TransactionMock.SetupCommitAsync();
        }

        public void VerifyCommittingTransaction()
        {
            TransactionMock.VerifyCommitAsync(Times.Once);
        }

        public abstract void VerifyCallingService();
    }
}