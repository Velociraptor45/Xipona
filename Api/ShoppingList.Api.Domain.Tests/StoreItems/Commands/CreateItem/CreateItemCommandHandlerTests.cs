using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Fixtures;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Services;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.CreateItem
{
    public class CreateItemCommandHandlerTests
    {
        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidDataAndManufacturerId_ShouldCreateItem()
        {
            // Arrange
            var local = new LocalFixture();

            IStoreItem storeItem = local.StoreItemFixture.CreateValid();
            List<IStoreItemAvailability> availabilities = storeItem.Availabilities.ToList();

            var handler = local.CreateCommandHandler();
            var command = local.CreateCommand(availabilities);

            local.StoreItemFactoryMock.SetupCreate(command.ItemCreation, storeItem);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                local.ItemRepositoryMock.VerifyStoreAsyncOnce(storeItem);
                local.ItemCategoryValidationServiceMock.VerifyValidateOnce(command.ItemCreation.ItemCategoryId);
                local.ManufacturerValidationServiceMock.VerifyValidateOnce(command.ItemCreation.ManufacturerId);
                local.AvailabilityValidationServiceMock.VerifyValidateOnce(availabilities);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidDataAndManufacturerIdNull_ShouldCreateItem()
        {
            // Arrange
            var local = new LocalFixture();

            IStoreItem storeItem = local.StoreItemFixture.CreateValid();
            List<IStoreItemAvailability> availabilities = storeItem.Availabilities.ToList();

            var handler = local.CreateCommandHandler();
            var command = local.CreateCommandWithoutManufacturerId(availabilities);

            local.StoreItemFactoryMock.SetupCreate(command.ItemCreation, storeItem);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                local.ItemRepositoryMock.VerifyStoreAsyncOnce(storeItem);
                local.ItemCategoryValidationServiceMock.VerifyValidateOnce(command.ItemCreation.ItemCategoryId);
                local.ManufacturerValidationServiceMock.VerifyValidateNever();
                local.AvailabilityValidationServiceMock.VerifyValidateOnce(availabilities);
            }
        }

        private sealed class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public StoreItemFixture StoreItemFixture { get; }
            public ShoppingListMockFixture ShoppingListMockFixture { get; }
            public ItemCategoryFixture ItemCategoryFixture { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }
            public StoreItemFactoryMock StoreItemFactoryMock { get; }
            public ItemCategoryValidationServiceMock ItemCategoryValidationServiceMock { get; }
            public ManufacturerValidationServiceMock ManufacturerValidationServiceMock { get; }
            public AvailabilityValidationServiceMock AvailabilityValidationServiceMock { get; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                StoreItemFixture = new StoreItemFixture(new StoreItemAvailabilityFixture(CommonFixture), CommonFixture);
                ShoppingListMockFixture = new ShoppingListMockFixture(CommonFixture, new ShoppingListFixture(CommonFixture));
                ItemCategoryFixture = new ItemCategoryFixture(CommonFixture);

                ItemRepositoryMock = new ItemRepositoryMock(Fixture);
                StoreItemFactoryMock = new StoreItemFactoryMock(Fixture);
                ItemCategoryValidationServiceMock = new ItemCategoryValidationServiceMock(Fixture);
                ManufacturerValidationServiceMock = new ManufacturerValidationServiceMock(Fixture);
                AvailabilityValidationServiceMock = new AvailabilityValidationServiceMock(Fixture);
            }

            public CreateItemCommand CreateCommand()
            {
                return Fixture.Create<CreateItemCommand>();
            }

            public CreateItemCommand CreateCommand(IEnumerable<IStoreItemAvailability> availabilities)
            {
                Fixture.ConstructorArgumentFor<ItemCreation, IEnumerable<IStoreItemAvailability>>(
                    "availabilities", availabilities);

                return Fixture.Create<CreateItemCommand>();
            }

            public CreateItemCommand CreateCommandWithoutManufacturerId(
                IEnumerable<IStoreItemAvailability> availabilities)
            {
                Fixture.ConstructorArgumentFor<ItemCreation, ManufacturerId>("manufacturerId", null);
                Fixture.ConstructorArgumentFor<ItemCreation, IEnumerable<IStoreItemAvailability>>(
                    "availabilities", availabilities);

                return Fixture.Create<CreateItemCommand>();
            }

            public CreateItemCommandHandler CreateCommandHandler()
            {
                return Fixture.Create<CreateItemCommandHandler>();
            }
        }
    }
}