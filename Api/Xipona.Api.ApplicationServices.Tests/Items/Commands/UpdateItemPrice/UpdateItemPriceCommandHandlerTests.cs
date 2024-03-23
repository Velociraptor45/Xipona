using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Updates;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.Items.Commands.UpdateItemPrice;

public class UpdateItemPriceCommandHandlerTests : CommandHandlerTestsBase<
    UpdateItemPriceCommandHandler, UpdateItemPriceCommand, bool>
{
    public UpdateItemPriceCommandHandlerTests() : base(new UpdateItemPriceCommandHandlerFixture())
    {
    }

    private sealed class UpdateItemPriceCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ItemUpdateServiceMock _itemUpdateServiceMock = new(MockBehavior.Strict);

        public override UpdateItemPriceCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;

        public override UpdateItemPriceCommandHandler CreateSut()
        {
            return new UpdateItemPriceCommandHandler(_ => _itemUpdateServiceMock.Object,
                TransactionGeneratorMock.Object);
        }

        public override void Setup()
        {
            SetupCommand();
            SetupCallingService();
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<UpdateItemPriceCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);

            _itemUpdateServiceMock.SetupUpdateAsync(Command.ItemId, Command.ItemTypeId, Command.StoreId, Command.Price);
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);

            _itemUpdateServiceMock.VerifyUpdateAsync(Command.ItemId, Command.ItemTypeId, Command.StoreId, Command.Price,
                Times.Once);
        }
    }
}