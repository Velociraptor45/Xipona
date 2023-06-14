using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.EventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.EventHandlers;

public class ItemAvailabilityDeletedDomainEventHandlerTests
    : DomainEventHandlerTestsBase<ItemAvailabilityDeletedDomainEvent, ItemAvailabilityDeletedDomainEventHandler>
{
    public ItemAvailabilityDeletedDomainEventHandlerTests()
        : base(new ItemAvailabilityDeletedDomainEventHandlerFixture())
    {
    }

    private sealed class ItemAvailabilityDeletedDomainEventHandlerFixture : DomainEventHandlerBaseFixture
    {
        private readonly RecipeModificationServiceMock _serviceMock = new(MockBehavior.Strict);

        public override ItemAvailabilityDeletedDomainEventHandler CreateSut()
        {
            return new(_ => _serviceMock.Object,
                new Mock<ILogger<ItemAvailabilityDeletedDomainEventHandler>>(MockBehavior.Loose).Object);
        }

        public override void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);
            _serviceMock.SetupModifyIngredientsAfterAvailabilityWasDeletedAsync(DomainEvent.ItemId,
                null, DomainEvent.Availability.StoreId);
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);
            _serviceMock.VerifyModifyIngredientsAfterAvailabilityWasDeletedAsync(DomainEvent.ItemId,
                null, DomainEvent.Availability.StoreId, Times.Once);
        }
    }
}