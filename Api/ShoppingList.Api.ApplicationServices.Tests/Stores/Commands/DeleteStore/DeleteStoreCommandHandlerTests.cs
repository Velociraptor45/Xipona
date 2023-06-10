using ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.DeleteStore;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Services.Deletions;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Stores.Commands.DeleteStore;

public class DeleteStoreCommandHandlerTests : CommandHandlerTestsBase<DeleteStoreCommandHandler,
    DeleteStoreCommand, bool>
{
    public DeleteStoreCommandHandlerTests() : base(new DeleteStoreCommandHandlerFixture())
    {
    }

    private sealed class DeleteStoreCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly StoreDeletionServiceMock _serviceMock = new(MockBehavior.Strict);

        public override DeleteStoreCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;

        public override DeleteStoreCommandHandler CreateSut()
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
            _serviceMock.VerifyDeleteAsync(Command.StoreId, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<DeleteStoreCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.SetupDeleteAsync(Command.StoreId);
        }
    }
}