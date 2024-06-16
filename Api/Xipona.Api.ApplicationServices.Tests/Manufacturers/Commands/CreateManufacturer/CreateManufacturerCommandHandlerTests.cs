using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Manufacturers.Services.Creations;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommandHandlerTests()
    : CommandHandlerTestsBase<CreateManufacturerCommandHandler, CreateManufacturerCommand, IManufacturer>
        (new CreateManufacturerCommandHandlerFixture())
{
    private sealed class CreateManufacturerCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ManufacturerCreationServiceMock _serviceMock = new(MockBehavior.Strict);
        public override CreateManufacturerCommand? Command { get; protected set; }
        public override IManufacturer? ExpectedResult { get; protected set; } = ManufacturerMother.NotDeleted().Create();

        public override CreateManufacturerCommandHandler CreateSut()
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
            _serviceMock.VerifyCreateAsync(Command.Name, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<CreateManufacturerCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
            _serviceMock.SetupCreateAsync(Command.Name, ExpectedResult);
        }
    }
}