using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.ShoppingLists.Commands;

public class AddTemporaryItemToShoppingListCommandHandlerTests : CommandHandlerTestsBase<AddTemporaryItemToShoppingListCommandHandler,
    AddTemporaryItemToShoppingListCommand, bool>
{
    public AddTemporaryItemToShoppingListCommandHandlerTests() : base(new AddTemporaryItemToShoppingListCommandHandlerFixture())
    {
    }

    private sealed class AddTemporaryItemToShoppingListCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ShoppingListModificationServiceMock _serviceMock = new(MockBehavior.Strict);
        public override AddTemporaryItemToShoppingListCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;

        public override AddTemporaryItemToShoppingListCommandHandler CreateSut()
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
            _serviceMock.VerifyAddTemporaryItemAsync(Command.ShoppingListId, Command.ItemName, Command.QuantityType,
                Command.Quantity, Command.Price, Command.SectionId, Command.TemporaryItemId,
                Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<AddTemporaryItemToShoppingListCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.SetupAddTemporaryItemAsync(Command.ShoppingListId, Command.ItemName, Command.QuantityType,
                Command.Quantity, Command.Price, Command.SectionId, Command.TemporaryItemId);
        }
    }
}