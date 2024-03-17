using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.EventHandlers;
using ProjectHermes.Xipona.Api.Domain.Stores.DomainEvents;
using ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services.Deletions;
using ProjectHermes.Xipona.Api.Domain.Tests.Common;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.ShoppingLists.EventHandlers;

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
            _shoppingListDeletionServiceMock.VerifyHardDeleteForStoreAsync(DomainEvent.StoreId, Times.Once);
        }
    }
}