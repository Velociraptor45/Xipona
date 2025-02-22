using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.ModifyManufacturer;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Services.Modifications;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.Manufacturers.Commands.ModifyManufacturer;

public class ModifyManufacturerCommandHandlerTests()
    : CommandHandlerTestsBase<ModifyManufacturerCommandHandler, ModifyManufacturerCommand, bool>
        (new ModifyManufacturerCommandHandlerFixture())
{
    private sealed class ModifyManufacturerCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ManufacturerModificationServiceMock _serviceMock = new(MockBehavior.Strict);
        public override ModifyManufacturerCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;

        public override ModifyManufacturerCommandHandler CreateSut()
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
            _serviceMock.VerifyModifyAsync(Command.Modification, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<ModifyManufacturerCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.SetupModifyAsync(Command.Modification);
        }
    }
}