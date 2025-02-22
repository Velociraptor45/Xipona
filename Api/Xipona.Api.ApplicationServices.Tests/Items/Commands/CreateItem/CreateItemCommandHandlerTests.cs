using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.CreateItem;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Creation;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.Items.Commands.CreateItem;

public class CreateItemCommandHandlerTests()
    : CommandHandlerTestsBase<CreateItemCommandHandler, CreateItemCommand, ItemReadModel>
        (new CreateItemCommandHandlerFixture())
{
    private sealed class CreateItemCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ItemCreationServiceMock _itemCreationServiceMock = new(MockBehavior.Strict);
        public override CreateItemCommand? Command { get; protected set; }

        public override ItemReadModel? ExpectedResult { get; protected set; } =
            new DomainTestBuilder<ItemReadModel>().Create();

        public override CreateItemCommandHandler CreateSut()
        {
            return new(_ => _itemCreationServiceMock.Object, TransactionGeneratorMock.Object);
        }

        public override void Setup()
        {
            SetupCommand();
            SetupCallingService();
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _itemCreationServiceMock.VerifyCreateAsync(Command.ItemCreation, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<CreateItemCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
            _itemCreationServiceMock.SetupCreateAsync(Command.ItemCreation, ExpectedResult);
        }
    }
}