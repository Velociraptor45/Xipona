using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Domain.Items.EventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.Stores.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.EventHandlers;

public class StoreDeletedDomainEventHandlerTests
    : DomainEventHandlerTestsBase<StoreDeletedDomainEvent, StoreDeletedDomainEventHandler>
{
    public StoreDeletedDomainEventHandlerTests() : base(new StoreDeletedDomainEventHandlerFixture())
    {
    }

    private sealed class StoreDeletedDomainEventHandlerFixture : DomainEventHandlerBaseFixture
    {
        private readonly ItemModificationServiceMock _serviceMock = new(MockBehavior.Strict);

        public override StoreDeletedDomainEventHandler CreateSut()
        {
            return new(_ => _serviceMock.Object,
                new Mock<ILogger<StoreDeletedDomainEventHandler>>(MockBehavior.Loose).Object);
        }

        public override void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);
            _serviceMock.SetupRemoveAvailabilitiesForAsync(DomainEvent.StoreId);
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);
            _serviceMock.VerifyRemoveAvailabilitiesForAsync(DomainEvent.StoreId, Times.Once());
        }
    }
}