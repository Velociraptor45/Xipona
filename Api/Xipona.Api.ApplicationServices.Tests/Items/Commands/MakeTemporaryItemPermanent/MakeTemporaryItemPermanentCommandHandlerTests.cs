using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.TemporaryItems;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.Items.Commands.MakeTemporaryItemPermanent;

public class MakeTemporaryItemPermanentCommandHandlerTests()
    : CommandHandlerTestsBase<MakeTemporaryItemPermanentCommandHandler, MakeTemporaryItemPermanentCommand, bool>
        (new MakeTemporaryItemPermanentCommandHandlerFixture())
{
    private sealed class MakeTemporaryItemPermanentCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly TemporaryItemServiceMock _serviceMock = new(MockBehavior.Strict);
        public override MakeTemporaryItemPermanentCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;

        public override MakeTemporaryItemPermanentCommandHandler CreateSut()
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
            _serviceMock.VerifyMakePermanentAsync(Command.PermanentItem, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<MakeTemporaryItemPermanentCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.SetupMakePermanentAsync(Command.PermanentItem);
        }
    }
}