using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Mocks;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;
using ShoppingList.Api.Domain.TestKit.Stores.Mocks;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemSectionFixture storeItemSectionFixture;
        private readonly StoreFixture storeFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly CreateTemporaryItemCommandFixture createTemporaryItemCommandFixture;

        public CreateTemporaryItemCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemSectionFixture = new StoreItemSectionFixture(commonFixture);
            storeFixture = new StoreFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(new StoreItemAvailabilityFixture(commonFixture), commonFixture);
            createTemporaryItemCommandFixture = new CreateTemporaryItemCommandFixture(commonFixture);
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            var handler = fixture.Create<CreateTemporaryItemCommandHandler>();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithDeletedStore_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            StoreRepositoryMock storeRepositoryMock = new StoreRepositoryMock(fixture);

            var handler = fixture.Create<CreateTemporaryItemCommandHandler>();
            var command = fixture.Create<CreateTemporaryItemCommand>();

            IStore store = storeFixture.GetStore(command.TemporaryItemCreation.Availability.StoreId.AsStoreId(), isDeleted: true);

            storeRepositoryMock.SetupFindByAsync(command.TemporaryItemCreation.Availability.StoreId.AsStoreId(), store);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.StoreNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithStoreIsNull_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            StoreRepositoryMock storeRepositoryMock = new StoreRepositoryMock(fixture);

            var handler = fixture.Create<CreateTemporaryItemCommandHandler>();
            var command = fixture.Create<CreateTemporaryItemCommand>();

            storeRepositoryMock.SetupFindByAsync(command.TemporaryItemCreation.Availability.StoreId.AsStoreId(), null);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.StoreNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidStoreItemSectionId_ShouldThrowDomainException()
        {
            // todo implement
        }

        [Fact]
        public async Task HandleAsync_WithValidCommand_ShouldStoreItem()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            StoreItemFactoryMock storeItemFactoryMock = new StoreItemFactoryMock(fixture);
            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);
            StoreRepositoryMock storeRepositoryMock = new StoreRepositoryMock(fixture);
            StoreItemAvailabilityFactoryMock availabilityFactoryMock = new StoreItemAvailabilityFactoryMock(fixture);

            IStoreItem storeItem = storeItemFixture.CreateValid();
            IStoreItemAvailability availability = commonFixture.ChooseRandom(storeItem.Availabilities);
            StoreId storeId = availability.StoreId.Id.AsStoreId();

            var handler = fixture.Create<CreateTemporaryItemCommandHandler>();
            var command = createTemporaryItemCommandFixture.Create(availability);

            // setup store
            StoreDefinition baseStoreDefinition = StoreDefinition.FromId(storeId);
            IStore store = storeFixture.CreateValid(baseStoreDefinition, 4);
            storeRepositoryMock.SetupFindByAsync(storeId, store);

            // setup availabilities
            IStoreSection defaultSection = store.Sections.Single(s => s.IsDefaultSection);

            availabilityFactoryMock.SetupCreate(store, availability.Price, defaultSection.Id, availability);
            storeItemFactoryMock.SetupCreate(command.TemporaryItemCreation, availability, storeItem);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                storeRepositoryMock.VerifyFindByAsyncOnce(storeId);
                availabilityFactoryMock.VerifyCreateOnce(store, availability.Price, defaultSection.Id);
                itemRepositoryMock.VerifyStoreAsyncOnce(storeItem);
            }
        }

        //todo further tests
    }
}