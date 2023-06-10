using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.EventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.Stores.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Services.Deletions;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.EventHandlers;

public class StoreDeletedDomainEventHandlerTests
    : DomainEventHandlerTestsBase<StoreDeletedDomainEvent, StoreDeletedDomainEventHandler>
{
    public StoreDeletedDomainEventHandlerTests() : base(new StoreDeletedDomainEventHandlerFixture())
    {
    }

    private sealed class StoreDeletedDomainEventHandlerFixture : DomainEventHandlerBaseFixture
    {
        private readonly ShoppingListDeletionServiceMock _shoppingListDeletionServiceMock = new(MockBehavior.Strict);

        public override StoreDeletedDomainEventHandler CreateSut()
        {
            return new(_ => _shoppingListDeletionServiceMock.Object,
                new Mock<ILogger<StoreDeletedDomainEventHandler>>(MockBehavior.Loose).Object);
        }

        public override void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);
            _shoppingListDeletionServiceMock.SetupHardDeleteForStoreAsync(DomainEvent.StoreId);
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);
            _shoppingListDeletionServiceMock.VerifyHardDeleteForStoreAsync(DomainEvent.StoreId, Times.Once());
        }
    }
}