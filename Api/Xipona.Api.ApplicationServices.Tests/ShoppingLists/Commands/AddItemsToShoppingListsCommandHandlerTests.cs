using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.ShoppingLists.Commands;

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