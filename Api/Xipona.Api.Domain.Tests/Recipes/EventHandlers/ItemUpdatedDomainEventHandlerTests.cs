using Xipona.Api.Domain.Items.DomainEvents;
using Xipona.Api.Domain.Recipes.EventHandlers;
using Xipona.Api.Domain.TestKit.Recipes.Services.Modifications;
using Xipona.Api.Domain.Tests.Common;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.Domain.Tests.Recipes.EventHandlers;

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
                DomainEvent.ItemId, DomainEvent.NewItem);
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);
            _recipeModificationServiceMock.VerifyModifyIngredientsAfterItemUpdateAsync(
                DomainEvent!.ItemId, DomainEvent.NewItem, Times.Once);
        }
    }
}