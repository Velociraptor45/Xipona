using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.CreateItemWithTypes;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Creation;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.Items.Commands.CreateItemWithTypes;

public class CreateItemWithTypesCommandHandlerTests
    : CommandHandlerTestsBase<CreateItemWithTypesCommandHandler, CreateItemWithTypesCommand, ItemReadModel>
{
    public CreateItemWithTypesCommandHandlerTests() : base(new CreateItemWithTypesCommandHandlerFixture())
    {
    }

    private sealed class CreateItemWithTypesCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ItemCreationServiceMock _serviceMock = new(MockBehavior.Strict);
        public override CreateItemWithTypesCommand? Command { get; protected set; }

        public override ItemReadModel? ExpectedResult { get; protected set; } =
            new DomainTestBuilder<ItemReadModel>().Create();

        public override CreateItemWithTypesCommandHandler CreateSut()
        {
            return new(TransactionGeneratorMock.Object, _ => _serviceMock.Object);
        }

        public override void Setup()
        {
            SetupCommand();
            SetupCallingService();
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.VerifyCreateItemWithTypesAsync(Command.Item, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<CreateItemWithTypesCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
            _serviceMock.SetupCreateItemWithTypesAsync(Command.Item, ExpectedResult);
        }
    }
}