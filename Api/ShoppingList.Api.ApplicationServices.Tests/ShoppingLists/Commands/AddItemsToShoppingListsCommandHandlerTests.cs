using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.ShoppingLists.Commands;

public class AddItemsToShoppingListsCommandHandlerTests : CommandHandlerTestsBase<AddItemsToShoppingListsCommandHandler,
    AddItemsToShoppingListsCommand, bool>
{
    public AddItemsToShoppingListsCommandHandlerTests() : base(new AddItemsToShoppingListsCommandHandlerFixture())
    {
    }

    private sealed class AddItemsToShoppingListsCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly AddItemToShoppingListServiceMock _serviceMock = new(MockBehavior.Strict);

        public override AddItemsToShoppingListsCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;

        public override AddItemsToShoppingListsCommandHandler CreateSut()
        {
            return new(_serviceMock.Object, TransactionGeneratorMock.Object);
        }

        public override void Setup()
        {
            SetupCommand();
            SetupCallingService();
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.VerifyAddAsync(Command.ItemToShoppingListAdditions, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<AddItemsToShoppingListsCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);

            _serviceMock.SetupAddAsync(Command.ItemToShoppingListAdditions);
        }
    }
}