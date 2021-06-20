using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Services;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Services;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.StoreItems.Fixtures;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.MakeTemporaryItemPermanent
{
    public class MakeTemporaryItemPermanentCommandHandlerTests
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
        public async Task HandleAsync_WithInvalidItemId_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommand();

            local.ItemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, null);

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
        public async Task HandleAsync_WithNonTemporaryItem_ShouldThrowDomainException()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();
            var command = local.CreateCommand();

            var baseDefinition = StoreItemDefinition.FromTemporary(false);
            IStoreItem storeItem = local.StoreItemFixture.CreateValid(baseDefinition);

            local.ItemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, storeItem);

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
        public async Task HandleAsync_WithValidDataAndManufacturerId_ShouldMakeTemporaryItemPermanent()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();

            StoreItemMock storeItemMock = local.StoreItemMockFixture.Create(StoreItemDefinition.FromTemporary(true));
            List<IStoreItemAvailability> availabilities = storeItemMock.Object.Availabilities.ToList();

            var command = local.CreateCommand(availabilities);

            local.ItemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, storeItemMock.Object);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                local.ItemCategoryValidationServiceMock.VerifyValidateAsyncOnce(command.PermanentItem.ItemCategoryId);
                local.ManufacturerValidationServiceMock.VerifyValidateAsyncOnce(command.PermanentItem.ManufacturerId);
                local.AvailabilityValidationServiceMock.VerifyValidateOnce(availabilities);
                storeItemMock.VerifyMakePermanentOnce(command.PermanentItem, availabilities);
                local.ItemRepositoryMock.VerifyStoreAsyncOnce(storeItemMock.Object);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidDataAndManufacturerIdIsNull_ShouldMakeTemporaryItemPermanent()
        {
            // Arrange
            var local = new LocalFixture();
            var handler = local.CreateCommandHandler();

            StoreItemMock storeItemMock = local.StoreItemMockFixture.Create(StoreItemDefinition.FromTemporary(true));
            List<IStoreItemAvailability> availabilities = storeItemMock.Object.Availabilities.ToList();

            var command = local.CreateCommandWithoutManufacturerId(availabilities);

            local.ItemRepositoryMock.SetupFindByAsync(command.PermanentItem.Id, storeItemMock.Object);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                local.ItemCategoryValidationServiceMock.VerifyValidateAsyncOnce(command.PermanentItem.ItemCategoryId);
                local.ManufacturerValidationServiceMock.VerifyValidateAsyncNever();
                local.AvailabilityValidationServiceMock.VerifyValidateOnce(availabilities);
                storeItemMock.VerifyMakePermanentOnce(command.PermanentItem, availabilities);
                local.ItemRepositoryMock.VerifyStoreAsyncOnce(storeItemMock.Object);
            }
        }

        private sealed class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public StoreItemFixture StoreItemFixture { get; }
            public StoreItemMockFixture StoreItemMockFixture { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }
            public StoreItemFactoryMock StoreItemFactoryMock { get; }
            public ItemCategoryValidationServiceMock ItemCategoryValidationServiceMock { get; }
            public ManufacturerValidationServiceMock ManufacturerValidationServiceMock { get; }
            public AvailabilityValidationServiceMock AvailabilityValidationServiceMock { get; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                StoreItemFixture = new StoreItemFixture(new StoreItemAvailabilityFixture(CommonFixture), CommonFixture);
                StoreItemMockFixture = new StoreItemMockFixture(CommonFixture, StoreItemFixture);

                ItemRepositoryMock = new ItemRepositoryMock(Fixture);
                StoreItemFactoryMock = new StoreItemFactoryMock(Fixture);
                ItemCategoryValidationServiceMock = new ItemCategoryValidationServiceMock(Fixture);
                ManufacturerValidationServiceMock = new ManufacturerValidationServiceMock(Fixture);
                AvailabilityValidationServiceMock = new AvailabilityValidationServiceMock(Fixture);
            }

            public MakeTemporaryItemPermanentCommand CreateCommand()
            {
                return Fixture.Create<MakeTemporaryItemPermanentCommand>();
            }

            public MakeTemporaryItemPermanentCommand CreateCommand(IEnumerable<IStoreItemAvailability> availabilities)
            {
                Fixture.ConstructorArgumentFor<PermanentItem, IEnumerable<IStoreItemAvailability>>(
                    "availabilities", availabilities);

                return Fixture.Create<MakeTemporaryItemPermanentCommand>();
            }

            public MakeTemporaryItemPermanentCommand CreateCommandWithoutManufacturerId(
                IEnumerable<IStoreItemAvailability> availabilities)
            {
                Fixture.ConstructorArgumentFor<PermanentItem, ManufacturerId>("manufacturerId", null);
                Fixture.ConstructorArgumentFor<PermanentItem, IEnumerable<IStoreItemAvailability>>(
                    "availabilities", availabilities);

                return Fixture.Create<MakeTemporaryItemPermanentCommand>();
            }

            public MakeTemporaryItemPermanentCommandHandler CreateCommandHandler()
            {
                return Fixture.Create<MakeTemporaryItemPermanentCommandHandler>();
            }
        }
    }
}