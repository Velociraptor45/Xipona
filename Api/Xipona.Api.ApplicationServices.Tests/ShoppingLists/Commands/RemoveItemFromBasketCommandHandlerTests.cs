using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.ShoppingLists.Commands;

public class RemoveItemFromBasketCommandHandlerTests
    : CommandHandlerTestsBase<RemoveItemFromBasketCommandHandler, RemoveItemFromBasketCommand, bool>
{
    public RemoveItemFromBasketCommandHandlerTests() : base(new RemoveItemFromBasketCommandHandlerFixture())
    {
    }

    private sealed class RemoveItemFromBasketCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ShoppingListModificationServiceMock _serviceMock = new(MockBehavior.Strict);
        public override RemoveItemFromBasketCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;

        public override RemoveItemFromBasketCommandHandler CreateSut()
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
            _serviceMock.VerifyRemoveItemFromBasketAsync(Command.ShoppingListId, Command.OfflineTolerantItemId,
                Command.ItemTypeId, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<RemoveItemFromBasketCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.SetupRemoveItemFromBasketAsync(Command.ShoppingListId, Command.OfflineTolerantItemId, Command.ItemTypeId);
        }
    }
}