using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.CreateItem
{
    public class CreateItemCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreItemFixture storeItemFixture;
        private readonly CreateItemCommandFixture createItemCommandFixture;

        public CreateItemCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
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

            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = fixture.Create<CreateItemCommand>();

            itemCategoryRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<ItemCategoryId>(id => id == command.ItemCreation.ItemCategoryId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IItemCategory>(null));

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

            Mock<IManufacturerRepository> manufacturerRepositoryMock = fixture.Freeze<Mock<IManufacturerRepository>>();
            ManufacturerId manufacturerId = new ManufacturerId(commonFixture.NextInt());

            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = createItemCommandFixture.GetCreateItemCommand(manufacturerId);

            manufacturerRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<ManufacturerId>(id => id == manufacturerId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IManufacturer>(null));

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
        public async Task HandleAsync_WithValidDataAndFilledManufacturerId_ShouldCreateItem()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IStoreItemFactory> storeItemFactoryMock = fixture.Freeze<Mock<IStoreItemFactory>>();
            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            Mock<IManufacturerRepository> manufacturerRepositoryMock = fixture.Freeze<Mock<IManufacturerRepository>>();

            IManufacturer manufacturer = fixture.Create<Manufacturer>();
            IItemCategory itemCategory = fixture.Create<ItemCategory>();
            IStoreItem storeItem = storeItemFixture.GetStoreItem();

            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = createItemCommandFixture.GetCreateItemCommand(itemCategory.Id, manufacturer.Id);

            storeItemFactoryMock
                .Setup(i => i.Create(
                    It.Is<ItemCreation>(c => c == command.ItemCreation),
                    It.Is<IItemCategory>(cat => cat == itemCategory),
                    It.Is<IManufacturer>(man => man == manufacturer)))
                .Returns(storeItem);

            itemCategoryRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<ItemCategoryId>(id => id == itemCategory.Id),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(itemCategory));

            manufacturerRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<ManufacturerId>(id => id == manufacturer.Id),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(manufacturer));

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IStoreItem>(item => item == storeItem),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidDataAndManufacturerIdNull_ShouldCreateItem()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IStoreItemFactory> storeItemFactoryMock = fixture.Freeze<Mock<IStoreItemFactory>>();
            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IItemCategoryRepository> itemCategoryRepositoryMock = fixture.Freeze<Mock<IItemCategoryRepository>>();
            Mock<IManufacturerRepository> manufacturerRepositoryMock = fixture.Freeze<Mock<IManufacturerRepository>>();

            IItemCategory itemCategory = fixture.Create<ItemCategory>();
            IStoreItem storeItem = storeItemFixture.GetStoreItem();

            var handler = fixture.Create<CreateItemCommandHandler>();
            var command = createItemCommandFixture.GetCreateItemCommand(itemCategory.Id, null);

            storeItemFactoryMock
                .Setup(i => i.Create(
                    It.Is<ItemCreation>(c => c == command.ItemCreation),
                    It.Is<IItemCategory>(cat => cat == itemCategory),
                    It.Is<IManufacturer>(man => man == null)))
                .Returns(storeItem);

            itemCategoryRepositoryMock
                .Setup(i => i.FindByAsync(
                    It.Is<ItemCategoryId>(id => id == itemCategory.Id),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(itemCategory));

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                manufacturerRepositoryMock.Verify(
                    i => i.FindByAsync(
                        It.IsAny<ManufacturerId>(),
                        It.IsAny<CancellationToken>()),
                    Times.Never);
                itemRepositoryMock.Verify(
                    i => i.StoreAsync(
                        It.Is<IStoreItem>(item => item == storeItem),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}