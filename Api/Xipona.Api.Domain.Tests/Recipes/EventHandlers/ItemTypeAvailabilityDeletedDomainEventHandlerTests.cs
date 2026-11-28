using Microsoft.Extensions.Logging;
using Xipona.Api.Domain.Items.DomainEvents;
using Xipona.Api.Domain.Recipes.EventHandlers;
using Xipona.Api.Domain.TestKit.Recipes.Services.Modifications;
using Xipona.Api.Domain.Tests.Common;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.Domain.Tests.Recipes.EventHandlers;

public class ItemTypeAvailabilityDeletedDomainEventHandlerTests
    : DomainEventHandlerTestsBase<ItemTypeAvailabilityDeletedDomainEvent, ItemTypeAvailabilityDeletedDomainEventHandler>
{
    public ItemTypeAvailabilityDeletedDomainEventHandlerTests()
        : base(new ItemTypeAvailabilityDeletedDomainEventHandlerFixture())
    {
    }

    private sealed class ItemTypeAvailabilityDeletedDomainEventHandlerFixture : DomainEventHandlerBaseFixture
    {
        private readonly RecipeModificationServiceMock _serviceMock = new(MockBehavior.Strict);

        public override ItemTypeAvailabilityDeletedDomainEventHandler CreateSut()
        {
            return new(_ => _serviceMock.Object,
                new Mock<ILogger<ItemTypeAvailabilityDeletedDomainEventHandler>>(MockBehavior.Loose).Object);
        }

        public override void SetupCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);
            _serviceMock.SetupModifyIngredientsAfterAvailabilityWasDeletedAsync(DomainEvent.ItemId,
                DomainEvent.ItemTypeId, DomainEvent.Availability.StoreId);
        }

        public override void VerifyCallingService()
        {
            TestPropertyNotSetException.ThrowIfNull(DomainEvent);
            _serviceMock.VerifyModifyIngredientsAfterAvailabilityWasDeletedAsync(DomainEvent.ItemId,
                DomainEvent.ItemTypeId, DomainEvent.Availability.StoreId, Times.Once);
        }
    }
}