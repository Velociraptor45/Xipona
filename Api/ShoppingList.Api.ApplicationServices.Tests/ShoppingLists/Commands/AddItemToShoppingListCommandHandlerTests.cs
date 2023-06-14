using ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.ShoppingLists.Commands;

public class AddItemToShoppingListCommandHandlerTests : CommandHandlerTestsBase<AddItemToShoppingListCommandHandler,
    AddItemToShoppingListCommand, bool>
{
    public AddItemToShoppingListCommandHandlerTests() : base(new AddItemToShoppingListCommandHandlerFixture())
    {
    }

    private sealed class AddItemToShoppingListCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly AddItemToShoppingListServiceMock _serviceMock = new(MockBehavior.Strict);
        public override AddItemToShoppingListCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;

        public override AddItemToShoppingListCommandHandler CreateSut()
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
            _serviceMock.VerifyAddAsync(Command.ShoppingListId, Command.ItemId, Command.SectionId, Command.Quantity,
                Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<AddItemToShoppingListCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.SetupAddAsync(Command.ShoppingListId, Command.ItemId, Command.SectionId, Command.Quantity);
        }
    }
}