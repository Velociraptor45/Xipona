using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ItemCategories.Services.Modifications;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Tests.ItemCategories.Commands.ModifyItemCategory;

public class ModifyItemCategoryCommandHandlerTests : CommandHandlerTestsBase<
    ModifyItemCategoryCommandHandler, ModifyItemCategoryCommand, bool>
{
    public ModifyItemCategoryCommandHandlerTests() : base(new ModifyItemCategoryCommandHandlerFixture())
    {
    }

    private sealed class ModifyItemCategoryCommandHandlerFixture : CommandHandlerBaseFixture
    {
        private readonly ItemCategoryModificationServiceMock _itemCategoryModificationServiceMock =
            new(MockBehavior.Strict);

        public override ModifyItemCategoryCommand? Command { get; protected set; }

        public override bool ExpectedResult { get; protected set; } = true;

        public override ModifyItemCategoryCommandHandler CreateSut()
        {
            return new ModifyItemCategoryCommandHandler(TransactionGeneratorMock.Object,
                _ => _itemCategoryModificationServiceMock.Object);
        }

        public override void Setup()
        {
            SetupCommand();
            SetupCallingService();
        }

        private void SetupCommand()
        {
            Command = new DomainTestBuilder<ModifyItemCategoryCommand>().Create();
        }

        private void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);

            _itemCategoryModificationServiceMock.SetupModifyAsync(Command.Modification);
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(Command);

            _itemCategoryModificationServiceMock.VerifyModifyAsync(Command.Modification, Times.Once);
        }
    }
}