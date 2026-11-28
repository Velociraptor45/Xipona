using Xipona.Api.ApplicationServices.ShoppingLists.Commands.PutItemInBasket;
using Xipona.Api.ApplicationServices.Tests.Common;
using Xipona.Api.Domain.TestKit.Common;
using Xipona.Api.Domain.TestKit.ShoppingLists.Services.Modifications;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.ApplicationServices.Tests.ShoppingLists.Commands.PutItemInBasket;

public class PutItemInBasketCommandHandlerTests()
    : CommandHandlerTestsBase<PutItemInBasketCommandHandler, PutItemInBasketCommand, bool>
        (new PutItemInBasketCommandHandlerFixture())
{
    private sealed class PutItemInBasketCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ShoppingListModificationServiceMock _serviceMock = new(MockBehavior.Strict);
        public override PutItemInBasketCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;

        public override PutItemInBasketCommandHandler CreateSut()
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
            _serviceMock.VerifyPutItemInBasketAsync(Command.ShoppingListId, Command.OfflineTolerantItemId,
                Command.ItemTypeId, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<PutItemInBasketCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.SetupPutItemInBasketAsync(Command.ShoppingListId, Command.OfflineTolerantItemId,
                Command.ItemTypeId);
        }
    }
}