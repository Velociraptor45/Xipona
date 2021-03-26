using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Tests;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Mocks;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Mocks;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Mocks;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.MakeTemporaryItemPermanent
{
    public class MakeTemporaryItemPermanentCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreFixture storeFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemSectionFixture storeItemSectionFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemMockFixture storeItemMockFixture;
        private readonly MakeTemporaryItemPermanentCommandFixture makeTemporaryItemPermanentCommandFixture;

        public MakeTemporaryItemPermanentCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            storeFixture = new StoreFixture(commonFixture);
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemSectionFixture = new StoreItemSectionFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
            shoppingListFixture = new ShoppingListFixture(commonFixture);
            storeItemMockFixture = new StoreItemMockFixture(commonFixture, storeItemFixture);
            makeTemporaryItemPermanentCommandFixture = new MakeTemporaryItemPermanentCommandFixture(commonFixture);
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidItemId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);

            var command = fixture.Create<MakeTemporaryItemPermanentCommand>();
            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            itemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, null);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidItemCategoryId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);
            ItemCategoryRepositoryMock itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(fixture);

            var baseDefinition = StoreItemDefinition.FromTemporary(true);
            IStoreItem storeItem = storeItemFixture.CreateValid(baseDefinition);

            var command = fixture.Create<MakeTemporaryItemPermanentCommand>();
            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            itemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, storeItem);
            itemCategoryRepositoryMock.SetupFindByAsync(command.PermanentItem.ItemCategoryId, null);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithNonTemporaryItem_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);

            var baseDefinition = StoreItemDefinition.FromTemporary(false);
            IStoreItem storeItem = storeItemFixture.CreateValid(baseDefinition);

            var command = fixture.Create<MakeTemporaryItemPermanentCommand>();
            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            itemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, storeItem);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ItemNotTemporary);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidManufacturerId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);
            ItemCategoryRepositoryMock itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(fixture);
            ManufacturerRepositoryMock manufacturerRepositoryMock = new ManufacturerRepositoryMock(fixture);

            var baseDefinition = StoreItemDefinition.FromTemporary(true);
            IStoreItem storeItem = storeItemFixture.CreateValid(baseDefinition);
            IItemCategory itemCategory = fixture.Create<IItemCategory>();
            var manufacturerId = new ManufacturerId(commonFixture.NextInt());

            var command = makeTemporaryItemPermanentCommandFixture.Create(manufacturerId);
            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            itemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, storeItem);
            itemCategoryRepositoryMock.SetupFindByAsync(command.PermanentItem.ItemCategoryId, itemCategory);
            manufacturerRepositoryMock.SetupFindByAsync(manufacturerId, null);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.ManufacturerNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidStoreId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);

            var manufacturerId = new ManufacturerId(commonFixture.NextInt());
            var allStores = storeFixture.CreateManyValid().ToList();
            MakeTemporaryItemPermanentCommand command = null; //CreateCommandWithAvailabilities(manufacturerId, allStores);
            allStores.Shuffle();

            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            var baseDefinition = StoreItemDefinition.FromTemporary(true);
            IStoreItem storeItem = storeItemFixture.CreateValid(baseDefinition);

            itemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, storeItem);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(ex => ex.Reason.ErrorCode == ErrorReasonCode.StoreNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidStoreItemSection_ShouldThrowDomainException()
        {
            // todo implement
        }

        [Fact]
        public async Task HandleAsync_WithInvalidStoreStoreIdFromAvailability_ShouldThrowDomainException()
        {
            // todo implement
        }

        [Fact]
        public async Task HandleAsync_WithValidData_ShouldMakeTemporaryItemPermanent()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);

            var manufacturerId = new ManufacturerId(commonFixture.NextInt());

            var handler = fixture.Create<MakeTemporaryItemPermanentCommandHandler>();

            StoreItemMock storeItemMock = storeItemMockFixture.Create(StoreItemDefinition.FromTemporary(true));
            List<IStoreItemAvailability> availabilities = storeItemMock.Object.Availabilities.ToList();

            var command = makeTemporaryItemPermanentCommandFixture.Create(manufacturerId, availabilities);

            itemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, storeItemMock.Object);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                storeItemMock.VerifyMakePermanentOnce(command.PermanentItem, availabilities);
                itemRepositoryMock.VerifyStoreAsyncOnce(storeItemMock.Object);
            }
        }
    }
}