using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.ShoppingLists.Commands.RemoveItemFromShoppingList;

public class RemoveItemFromShoppingListCommandHandlerTests()
    : CommandHandlerTestsBase<RemoveItemFromShoppingListCommandHandler, RemoveItemFromShoppingListCommand, bool>
        (new RemoveItemFromShoppingListCommandHandlerFixture())
{
    private sealed class RemoveItemFromShoppingListCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ShoppingListModificationServiceMock _serviceMock = new(MockBehavior.Strict);
        public override RemoveItemFromShoppingListCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;

        public override RemoveItemFromShoppingListCommandHandler CreateSut()
        {
            return new(_ => _serviceMock.Object, TransactionGeneratorMock.Object);
        }

        public override void Setup()
        {
            SetupCommand();
            SetupCallingService();
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.VerifyRemoveItemAsync(Command.ShoppingListId, Command.OfflineTolerantItemId, Command.ItemTypeId, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<RemoveItemFromShoppingListCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.SetupRemoveItemAsync(Command.ShoppingListId, Command.OfflineTolerantItemId, Command.ItemTypeId);
        }
    }
}