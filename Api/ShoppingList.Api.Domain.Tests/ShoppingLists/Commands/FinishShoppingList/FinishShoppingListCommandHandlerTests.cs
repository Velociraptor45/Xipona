using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.FinishShoppingList;
using ShoppingList.Api.Domain.TestKit.Common.Mocks;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.FinishShoppingList;

public class FinishShoppingListCommandHandlerTests
{
    private readonly CommonFixture _commonFixture;
    private readonly ShoppingListMockFixture _shoppingListMockFixture;
    private readonly LocalFixture _fixture;

    public FinishShoppingListCommandHandlerTests()
    {
        _commonFixture = new CommonFixture();
        _shoppingListMockFixture = new ShoppingListMockFixture();
        _fixture = new LocalFixture();
    }

    [Fact]
    public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var fixture = _commonFixture.GetNewFixture();
        var handler = fixture.Create<FinishShoppingListCommandHandler>();

        // Act
        Func<Task> function = async () => await handler.HandleAsync(null, default);

        // Assert
        using (new AssertionScope())
        {
            await function.Should().ThrowAsync<ArgumentNullException>();
        }
    }

    [Fact]
    public async Task HandleAsync_WithInvalidShoppingListId_ShouldThrowDomainException()
    {
        // Arrange
        var fixture = _commonFixture.GetNewFixture();

        var command = fixture.Create<FinishShoppingListCommand>();
        var handler = _fixture.CreateSut();

        _fixture.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, null);

        // Act
        Func<Task> function = async () => await handler.HandleAsync(command, default);

        // Assert
        using (new AssertionScope())
        {
            (await function.Should().ThrowAsync<DomainException>())
                .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ShoppingListNotFound);
        }
    }

    [Fact]
    public async Task HandleAsync_WithValidData_ShouldFinishShoppingList()
    {
        // Arrange
        var fixture = _commonFixture.GetNewFixture();

        Mock<ITransaction> transactionMock = new Mock<ITransaction>();

        ShoppingListMock listMock = _shoppingListMockFixture.Create();
        ShoppingListMock remainingListMock = _shoppingListMockFixture.Create();

        var command = fixture.Create<FinishShoppingListCommand>();
        var handler = _fixture.CreateSut();

        listMock.SetupFinish(command.CompletionDate, remainingListMock.Object);

        _fixture.ShoppingListRepositoryMock.SetupFindByAsync(command.ShoppingListId, listMock.Object);
        _fixture.ShoppingListRepositoryMock.SetupStoreAsync(listMock.Object);
        _fixture.ShoppingListRepositoryMock.SetupStoreAsync(remainingListMock.Object);
        _fixture.TransactionGeneratorMock.SetupGenerateAsync(transactionMock.Object);

        // Act
        bool result = await handler.HandleAsync(command, default);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
            listMock.VerifyFinishOnce(command.CompletionDate);
            _fixture.TransactionGeneratorMock.VerifyGenerateAsyncOnce();
            _fixture.ShoppingListRepositoryMock.VerifyStoreAsyncOnce(listMock.Object);
            _fixture.ShoppingListRepositoryMock.VerifyStoreAsyncOnce(remainingListMock.Object);
            transactionMock.Verify(
                i => i.CommitAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    private class LocalFixture
    {
        public LocalFixture()
        {
            TransactionGeneratorMock = new TransactionGeneratorMock(MockBehavior.Strict);
            ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
        }

        public TransactionGeneratorMock TransactionGeneratorMock { get; private set; }
        public ShoppingListRepositoryMock ShoppingListRepositoryMock { get; private set; }

        public FinishShoppingListCommandHandler CreateSut()
        {
            return new FinishShoppingListCommandHandler(ShoppingListRepositoryMock.Object,
                TransactionGeneratorMock.Object);
        }
    }
}