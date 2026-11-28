using Microsoft.Extensions.Logging;
using Xipona.Api.Domain.Items.DomainEvents;
using Xipona.Api.Domain.Recipes.EventHandlers;
using Xipona.Api.Domain.TestKit.Recipes.Services.Modifications;
using Xipona.Api.Domain.Tests.Common;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.Domain.Tests.Recipes.EventHandlers;

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