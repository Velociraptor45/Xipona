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
        private readonly LocalFixture _local;

        public MakeTemporaryItemPermanentCommandHandlerTests()
        {
            _local = new LocalFixture();
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var handler = _local.CreateCommandHandler();

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
            var handler = _local.CreateCommandHandler();
            _local.SetupCommand();

            _local.ItemRepositoryMock.SetupFindByAsync(_local.Command.PermanentItem.Id, null);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(_local.Command, default);

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
            var handler = _local.CreateCommandHandler();
            _local.SetupCommand();
            _local.SetupStoreItemMock(StoreItemMother.Initial().Create());

            _local.ItemRepositoryMock.SetupFindByAsync(_local.Command.PermanentItem.Id, _local.StoreItemMock.Object);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(_local.Command, default);

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
            var handler = _local.CreateCommandHandler();
            _local.SetupStoreItemMock(StoreItemMother.InitialTemporary().Create());

            List<IStoreItemAvailability> availabilities = _local.StoreItemMock.Object.Availabilities.ToList();
            _local.SetupCommand(availabilities);

            _local.ItemRepositoryMock.SetupFindByAsync(_local.Command.PermanentItem.Id, _local.StoreItemMock.Object);

            // Act
            var result = await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                _local.ItemCategoryValidationServiceMock.VerifyValidateAsyncOnce(_local.Command.PermanentItem.ItemCategoryId);
                _local.ManufacturerValidationServiceMock.VerifyValidateAsyncOnce(_local.Command.PermanentItem.ManufacturerId);
                _local.AvailabilityValidationServiceMock.VerifyValidateOnce(availabilities);
                _local.StoreItemMock.VerifyMakePermanentOnce(_local.Command.PermanentItem, availabilities);
                _local.ItemRepositoryMock.VerifyStoreAsyncOnce(_local.StoreItemMock.Object);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidDataAndManufacturerIdIsNull_ShouldMakeTemporaryItemPermanent()
        {
            // Arrange
            var handler = _local.CreateCommandHandler();
            _local.SetupStoreItemMock(StoreItemMother.InitialTemporary().Create());

            List<IStoreItemAvailability> availabilities = _local.StoreItemMock.Object.Availabilities.ToList();
            _local.SetupCommandWithoutManufacturerId(availabilities);

            _local.ItemRepositoryMock.SetupFindByAsync(_local.Command.PermanentItem.Id, _local.StoreItemMock.Object);

            // Act
            var result = await handler.HandleAsync(_local.Command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                _local.ItemCategoryValidationServiceMock.VerifyValidateAsyncOnce(_local.Command.PermanentItem.ItemCategoryId);
                _local.ManufacturerValidationServiceMock.VerifyValidateAsyncNever();
                _local.AvailabilityValidationServiceMock.VerifyValidateOnce(availabilities);
                _local.StoreItemMock.VerifyMakePermanentOnce(_local.Command.PermanentItem, availabilities);
                _local.ItemRepositoryMock.VerifyStoreAsyncOnce(_local.StoreItemMock.Object);
            }
        }

        private sealed class LocalFixture
        {
            public Fixture Fixture { get; }
            public CommonFixture CommonFixture { get; } = new CommonFixture();
            public StoreItemFixture StoreItemFixture { get; }
            public ItemRepositoryMock ItemRepositoryMock { get; }
            public StoreItemFactoryMock StoreItemFactoryMock { get; }
            public ItemCategoryValidationServiceMock ItemCategoryValidationServiceMock { get; }
            public ManufacturerValidationServiceMock ManufacturerValidationServiceMock { get; }
            public AvailabilityValidationServiceMock AvailabilityValidationServiceMock { get; }
            public StoreItemMock StoreItemMock { get; private set; }
            public MakeTemporaryItemPermanentCommand Command { get; private set; }

            public LocalFixture()
            {
                Fixture = CommonFixture.GetNewFixture();

                StoreItemFixture = new StoreItemFixture(new StoreItemAvailabilityFixture(CommonFixture), CommonFixture);

                ItemRepositoryMock = new ItemRepositoryMock(Fixture);
                StoreItemFactoryMock = new StoreItemFactoryMock(Fixture);
                ItemCategoryValidationServiceMock = new ItemCategoryValidationServiceMock(Fixture);
                ManufacturerValidationServiceMock = new ManufacturerValidationServiceMock(Fixture);
                AvailabilityValidationServiceMock = new AvailabilityValidationServiceMock(Fixture);
            }

            public void SetupCommand()
            {
                Command = Fixture.Create<MakeTemporaryItemPermanentCommand>();
            }

            public void SetupCommand(IEnumerable<IStoreItemAvailability> availabilities)
            {
                Fixture.ConstructorArgumentFor<PermanentItem, IEnumerable<IStoreItemAvailability>>(
                    "availabilities", availabilities);

                Command = Fixture.Create<MakeTemporaryItemPermanentCommand>();
            }

            public void SetupCommandWithoutManufacturerId(
                IEnumerable<IStoreItemAvailability> availabilities)
            {
                Fixture.ConstructorArgumentFor<PermanentItem, ManufacturerId>("manufacturerId", null);
                Fixture.ConstructorArgumentFor<PermanentItem, IEnumerable<IStoreItemAvailability>>(
                    "availabilities", availabilities);

                Command = Fixture.Create<MakeTemporaryItemPermanentCommand>();
            }

            public MakeTemporaryItemPermanentCommandHandler CreateCommandHandler()
            {
                return Fixture.Create<MakeTemporaryItemPermanentCommandHandler>();
            }

            public void SetupStoreItemMock(IStoreItem storeItem)
            {
                StoreItemMock = new StoreItemMock(storeItem);
            }
        }
    }
}