using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Mocks;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Mocks;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Mocks;
using ShoppingList.Api.Domain.TestKit.Stores.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.CreateItem
{
    public class CreateItemCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreFixture storeFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemSectionFixture storeItemSectionFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly CreateItemCommandFixture createItemCommandFixture;

        public CreateItemCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            storeFixture = new StoreFixture(commonFixture);
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemSectionFixture = new StoreItemSectionFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(new StoreItemAvailabilityFixture(commonFixture), commonFixture);
            createItemCommandFixture = new CreateItemCommandFixture(commonFixture);
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            var handler = fixture.Create<CreateItemCommandHandler>();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidItemCategoryId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            ItemCategoryRepositoryMock itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(fixture);
            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = fixture.Create<CreateItemCommand>();

            itemCategoryRepositoryMock.SetupFindByAsync(command.ItemCreation.ItemCategoryId, null);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidManufacturerId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            ManufacturerRepositoryMock manufacturerRepositoryMock = new ManufacturerRepositoryMock(fixture);
            ItemCategoryRepositoryMock itemCategoryRepositoryMock = new ItemCategoryRepositoryMock(fixture);

            ManufacturerId manufacturerId = new ManufacturerId(commonFixture.NextInt());
            IItemCategory itemCategory = fixture.Create<ItemCategory>();

            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = createItemCommandFixture.GetCreateItemCommand(itemCategory.Id, manufacturerId);

            manufacturerRepositoryMock.SetupFindByAsync(manufacturerId, null);
            itemCategoryRepositoryMock.SetupFindByAsync(itemCategory.Id, itemCategory);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ManufacturerNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidStoreStoreIdFromAvailability_ShouldThrowDomainException()
        {
            // todo implement
        }

        [Fact]
        public async Task HandleAsync_WithInvalidStoreItemSection_ShouldThrowDomainException()
        {
            // todo implement
        }

        [Fact]
        public async Task HandleAsync_WithValidDataAndFilledManufacturerId_ShouldCreateItem()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            StoreItemFactoryMock storeItemFactoryMock = new StoreItemFactoryMock(fixture);
            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);

            ItemCategoryId itemCategoryId = new ItemCategoryId(commonFixture.NextInt());
            ManufacturerId manufacturerId = new ManufacturerId(commonFixture.NextInt());
            IStoreItem storeItem = storeItemFixture.CreateValid();
            List<IStoreItemAvailability> availabilities = storeItem.Availabilities.ToList();

            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = createItemCommandFixture.GetCreateItemCommand(itemCategoryId, manufacturerId, availabilities);

            storeItemFactoryMock.SetupCreate(command.ItemCreation, storeItem);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemRepositoryMock.VerifyStoreAsyncOnce(storeItem);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidDataAndManufacturerIdNull_ShouldCreateItem()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            StoreItemFactoryMock storeItemFactoryMock = new StoreItemFactoryMock(fixture);
            ItemRepositoryMock itemRepositoryMock = new ItemRepositoryMock(fixture);

            ItemCategoryId itemCategoryId = new ItemCategoryId(commonFixture.NextInt());
            IStoreItem storeItem = storeItemFixture.CreateValid();
            List<IStoreItemAvailability> availabilities = storeItem.Availabilities.ToList();

            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = createItemCommandFixture.GetCreateItemCommand(itemCategoryId, null, availabilities);

            storeItemFactoryMock.SetupCreate(command.ItemCreation, storeItem);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemRepositoryMock.VerifyStoreAsyncOnce(storeItem);
            }
        }
    }
}