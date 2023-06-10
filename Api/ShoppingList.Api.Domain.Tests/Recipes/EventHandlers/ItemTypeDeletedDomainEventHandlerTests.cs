using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.EventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.EventHandlers;

public class ItemTypeDeletedDomainEventHandlerTests
    : DomainEventHandlerTestsBase<ItemTypeDeletedDomainEvent, ItemTypeDeletedDomainEventHandler>
{
    public ItemTypeDeletedDomainEventHandlerTests()
        : base(new ItemAvailabilityDeletedDomainEventHandlerFixture())
    {
    }

    private sealed class ItemAvailabilityDeletedDomainEventHandlerFixture : DomainEventHandlerBaseFixture
    {
        private readonly RecipeModificationServiceMock _serviceMock = new(MockBehavior.Strict);

        public override ItemTypeDeletedDomainEventHandler CreateSut()
        {
            return new(_ => _serviceMock.Object,
                new Mock<ILogger<ItemTypeDeletedDomainEventHandler>>(MockBehavior.Loose).Object);
        }

        public override void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);
            _serviceMock.SetupRemoveDefaultItemAsync(DomainEvent.ItemId, DomainEvent.ItemTypeId);
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);
            _serviceMock.VerifyRemoveDefaultItemAsync(DomainEvent.ItemId, DomainEvent.ItemTypeId, Times.Once);
        }
    }
}