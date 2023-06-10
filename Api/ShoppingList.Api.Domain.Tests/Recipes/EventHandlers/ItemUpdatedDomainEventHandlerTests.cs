using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.EventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.EventHandlers;

public class ItemUpdatedDomainEventHandlerTests
    : DomainEventHandlerTestsBase<ItemUpdatedDomainEvent, ItemUpdatedDomainEventHandler>
{
    public ItemUpdatedDomainEventHandlerTests() : base(new ItemUpdatedDomainEventHandlerFixture())
    {
    }

    private sealed class ItemUpdatedDomainEventHandlerFixture : DomainEventHandlerBaseFixture
    {
        private readonly RecipeModificationServiceMock _recipeModificationServiceMock = new(MockBehavior.Strict);

        public override ItemUpdatedDomainEventHandler CreateSut()
        {
            return new ItemUpdatedDomainEventHandler(_ => _recipeModificationServiceMock.Object);
        }

        public override void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);

            _recipeModificationServiceMock.SetupModifyIngredientsAfterItemUpdateAsync(
                DomainEvent.OldItemId, DomainEvent.NewItem);
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);
            _recipeModificationServiceMock.VerifyModifyIngredientsAfterItemUpdateAsync(
                DomainEvent!.OldItemId, DomainEvent.NewItem, Times.Once);
        }
    }
}