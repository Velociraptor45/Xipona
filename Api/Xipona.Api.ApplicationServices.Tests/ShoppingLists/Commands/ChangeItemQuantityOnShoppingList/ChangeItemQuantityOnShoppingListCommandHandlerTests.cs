using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;

public class ChangeItemQuantityOnShoppingListCommandHandlerTests()
    : CommandHandlerTestsBase<ChangeItemQuantityOnShoppingListCommandHandler, ChangeItemQuantityOnShoppingListCommand,
        bool>(new ChangeItemQuantityOnShoppingListCommandHandlerFixture())
{
    private sealed class ChangeItemQuantityOnShoppingListCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ShoppingListModificationServiceMock _serviceMock = new(MockBehavior.Strict);
        public override ChangeItemQuantityOnShoppingListCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;

        public override ChangeItemQuantityOnShoppingListCommandHandler CreateSut()
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
            _serviceMock.VerifyChangeItemQuantityAsync(Command.ShoppingListId, Command.OfflineTolerantItemId,
                Command.ItemTypeId, Command.Quantity, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<ChangeItemQuantityOnShoppingListCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.SetupChangeItemQuantityAsync(Command.ShoppingListId, Command.OfflineTolerantItemId,
                Command.ItemTypeId, Command.Quantity);
        }
    }
}