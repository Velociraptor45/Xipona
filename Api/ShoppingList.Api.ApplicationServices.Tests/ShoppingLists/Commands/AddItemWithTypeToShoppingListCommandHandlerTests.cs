using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.ShoppingLists.Commands;

public class AddItemWithTypeToShoppingListCommandHandlerTests : CommandHandlerTestsBase<AddItemWithTypeToShoppingListCommandHandler,
    AddItemWithTypeToShoppingListCommand, bool>
{
    public AddItemWithTypeToShoppingListCommandHandlerTests() : base(
        new AddItemWithTypeToShoppingListCommandHandlerFixture())
    {
    }

    private sealed class AddItemWithTypeToShoppingListCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly AddItemToShoppingListServiceMock _serviceMock = new(MockBehavior.Strict);
        public override AddItemWithTypeToShoppingListCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;

        public override AddItemWithTypeToShoppingListCommandHandler CreateSut()
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
            _serviceMock.VerifyAddItemWithTypeAsync(Command.ShoppingListId, Command.ItemId, Command.ItemTypeId,
                Command.SectionId, Command.Quantity, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<AddItemWithTypeToShoppingListCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.SetupAddItemWithTypeAsync(Command.ShoppingListId, Command.ItemId, Command.ItemTypeId,
                Command.SectionId, Command.Quantity);
        }
    }
}