using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemDiscount;
using ProjectHermes.Xipona.Api.ApplicationServices.Tests.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Tests.ShoppingLists.Commands.AddDiscount;

public class AddItemDiscountCommandHandlerTests()
    : CommandHandlerTestsBase<AddItemDiscountCommandHandler, AddItemDiscountCommand, bool>(new AddDiscountCommandHandlerFixture())
{
    private sealed class AddDiscountCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ShoppingListModificationServiceMock _serviceMock = new(MockBehavior.Strict);

        public override AddItemDiscountCommand? Command { get; protected set; }
        public override bool ExpectedResult { get; protected set; } = true;
        public override AddItemDiscountCommandHandler CreateSut()
        {
            return new AddItemDiscountCommandHandler(TransactionGeneratorMock.Object, _ => _serviceMock.Object);
        }

        public override void Setup()
        {
            SetupCommand();
            SetupCallingService();
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.VerifyAddDiscountAsync(Command.ShoppingListId, Command.Discount, Times.Once);
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<AddItemDiscountCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);
            _serviceMock.SetupAddDiscountAsync(Command.ShoppingListId, Command.Discount);
        }
    }
}
