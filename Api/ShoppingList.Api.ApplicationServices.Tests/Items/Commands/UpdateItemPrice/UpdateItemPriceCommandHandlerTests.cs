using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Items.Commands.UpdateItemPrice;

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