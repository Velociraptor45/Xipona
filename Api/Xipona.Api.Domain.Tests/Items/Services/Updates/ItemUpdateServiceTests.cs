using ProjectHermes.Xipona.Api.Core.TestKit.Services;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Updates;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Items.Services.Updates;

public class ItemUpdateServiceTests
{
    public class UpdateAsyncPrice
    {
        private readonly UpdateAsyncPriceFixture _fixture;

        public UpdateAsyncPrice()
        {
            _fixture = new UpdateAsyncPriceFixture();
        }

        public static IEnumerable<object?[]> GetItemTypeIds()
        {
            yield return new object?[] { null };
            yield return new object?[] { ItemTypeId.New };
        }

        [Theory]
        [MemberData(nameof(GetItemTypeIds))]
        public async Task UpdateAsync_WithValidItemId_ShouldUpdateItem(ItemTypeId? itemTypeId)
        {
            // Arrange
            _fixture.SetupWithValidItemId(itemTypeId);
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);
            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);

            // Act
            await sut.UpdateAsync(_fixture.OldItemId.Value, _fixture.ItemTypeId, _fixture.StoreId.Value,
                _fixture.Price.Value);

            // Assert
            _fixture.VerifyUpdatingItem();
        }

        [Theory]
        [MemberData(nameof(GetItemTypeIds))]
        public async Task UpdateAsync_WithValidItemId_ShouldStoreOldItem(ItemTypeId? itemTypeId)
        {
            // Arrange
            _fixture.SetupWithValidItemId(itemTypeId);
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);
            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);

            // Act
            await sut.UpdateAsync(_fixture.OldItemId.Value, _fixture.ItemTypeId, _fixture.StoreId.Value,
                _fixture.Price.Value);

            // Assert
            _fixture.VerifyStoringOldItem();
        }

        [Theory]
        [MemberData(nameof(GetItemTypeIds))]
        public async Task UpdateAsync_WithValidItemId_ShouldStoreNewItem(ItemTypeId? itemTypeId)
        {
            // Arrange
            _fixture.SetupWithValidItemId(itemTypeId);
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);
            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);

            // Act
            await sut.UpdateAsync(_fixture.OldItemId.Value, _fixture.ItemTypeId, _fixture.StoreId.Value,
                _fixture.Price.Value);

            // Assert
            _fixture.VerifyStoringNewItem();
        }

        [Theory]
        [MemberData(nameof(GetItemTypeIds))]
        public async Task UpdateAsync_WithInvalidItemId_ShouldThrow(ItemTypeId? itemTypeId)
        {
            // Arrange
            _fixture.SetupOldItemId();
            _fixture.SetupPrice();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeId(itemTypeId);
            _fixture.SetupNotFindingOldItem();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);
            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);

            // Act
            var func = async () => await sut.UpdateAsync(_fixture.OldItemId.Value, _fixture.ItemTypeId, _fixture.StoreId.Value,
                _fixture.Price.Value);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
        }

        [Theory]
        [MemberData(nameof(GetItemTypeIds))]
        public async Task UpdateAsync_WithItemIsTemporary_ShouldThrow(ItemTypeId? itemTypeId)
        {
            // Arrange
            _fixture.SetupOldItemId();
            _fixture.SetupPrice();
            _fixture.SetupStoreId();
            _fixture.SetupItemTypeId(itemTypeId);
            _fixture.SetupOldItemTemporary();
            _fixture.SetupFindingOldItem();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Price);
            TestPropertyNotSetException.ThrowIfNull(_fixture.StoreId);

            // Act
            var func = async () => await sut.UpdateAsync(_fixture.OldItemId.Value, _fixture.ItemTypeId, _fixture.StoreId.Value,
                _fixture.Price.Value);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.TemporaryItemNotUpdateable);
        }

        private sealed class UpdateAsyncPriceFixture : UpdateAsyncFixture
        {
            public Price? Price { get; private set; }
            public StoreId? StoreId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }

            public void SetupPrice()
            {
                Price = new DomainTestBuilder<Price>().Create();
            }

            public void SetupStoreId()
            {
                StoreId = Domain.Stores.Models.StoreId.New;
            }

            public void SetupItemTypeId(ItemTypeId? itemTypeId)
            {
                ItemTypeId = itemTypeId;
            }

            private void SetupUpdatingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(Price);
                TestPropertyNotSetException.ThrowIfNull(OldItemMock);
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                OldItemMock.SetupUpdate(StoreId.Value, ItemTypeId, Price.Value, DateTimeServiceMock.Object, NewItem);
            }

            public void VerifyUpdatingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(Price);
                TestPropertyNotSetException.ThrowIfNull(OldItemMock);
                TestPropertyNotSetException.ThrowIfNull(StoreId);

                OldItemMock.VerifyUpdate(StoreId.Value, ItemTypeId, Price.Value, DateTimeServiceMock.Object, Times.Once);
            }

            public void SetupWithValidItemId(ItemTypeId? itemTypeId)
            {
                SetupOldItemId();
                SetupOldItem();
                SetupPrice();
                SetupNewItem();
                SetupStoreId();
                SetupItemTypeId(itemTypeId);
                SetupFindingOldItem();
                SetupStoringOldItem();
                SetupStoringNewItem();
                SetupUpdatingItem();
            }
        }
    }

    public class UpdateAsyncWithTypes
    {
        private readonly UpdateAsyncWithTypesFixture _fixture;

        public UpdateAsyncWithTypes()
        {
            _fixture = new UpdateAsyncWithTypesFixture();
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidItemId_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupOldItemId();
            _fixture.SetupNotFindingOldItem();
            _fixture.SetupItemUpdate();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemWithTypesUpdate);

            // Act
            var func = async () => await sut.UpdateAsync(_fixture.ItemWithTypesUpdate);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemId_ShouldUpdateItem()
        {
            // Arrange
            _fixture.SetupWithValidItemId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemWithTypesUpdate);

            // Act
            await sut.UpdateAsync(_fixture.ItemWithTypesUpdate);

            // Assert
            _fixture.VerifyUpdatingItem();
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemId_ShouldStoreOldItem()
        {
            // Arrange
            _fixture.SetupWithValidItemId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemWithTypesUpdate);

            // Act
            await sut.UpdateAsync(_fixture.ItemWithTypesUpdate);

            // Assert
            _fixture.VerifyStoringOldItem();
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemId_ShouldStoreNewItem()
        {
            // Arrange
            _fixture.SetupWithValidItemId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemWithTypesUpdate);

            // Act
            await sut.UpdateAsync(_fixture.ItemWithTypesUpdate);

            // Assert
            _fixture.VerifyStoringNewItem();
        }

        private sealed class UpdateAsyncWithTypesFixture : UpdateAsyncFixture
        {
            public ItemWithTypesUpdate? ItemWithTypesUpdate { get; private set; }

            public void SetupItemUpdate()
            {
                TestPropertyNotSetException.ThrowIfNull(OldItemId);

                ItemWithTypesUpdate = new DomainTestBuilder<ItemWithTypesUpdate>()
                    .FillConstructorWith("oldId", OldItemId.Value).Create();
            }

            private void SetupUpdatingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemWithTypesUpdate);
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                TestPropertyNotSetException.ThrowIfNull(OldItemMock);

                OldItemMock.SetupUpdateAsync(ItemWithTypesUpdate, ValidatorMock.Object, DateTimeServiceMock.Object, NewItem);
            }

            public void VerifyUpdatingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemWithTypesUpdate);
                TestPropertyNotSetException.ThrowIfNull(OldItemMock);

                OldItemMock.VerifyUpdateAsync(ItemWithTypesUpdate, ValidatorMock.Object, DateTimeServiceMock.Object, Times.Once);
            }

            public void SetupWithValidItemId()
            {
                SetupOldItemId();
                SetupOldItem();
                SetupNewItem();
                SetupItemUpdate();
                SetupFindingOldItem();
                SetupUpdatingItem();
                SetupStoringOldItem();
                SetupStoringNewItem();
            }
        }
    }

    public class UpdateAsyncWithoutTypes
    {
        private readonly UpdateAsyncWithoutTypesFixture _fixture;

        public UpdateAsyncWithoutTypes()
        {
            _fixture = new UpdateAsyncWithoutTypesFixture();
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidItemId_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupOldItemId();
            _fixture.SetupNotFindingOldItem();
            _fixture.SetupItemUpdate();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemUpdate);

            // Act
            var func = async () => await sut.UpdateAsync(_fixture.ItemUpdate);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemId_ShouldUpdateItem()
        {
            // Arrange
            _fixture.SetupWithValidItemId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemUpdate);

            // Act
            await sut.UpdateAsync(_fixture.ItemUpdate);

            // Assert
            _fixture.VerifyUpdatingItem();
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemId_ShouldStoreOldItem()
        {
            // Arrange
            _fixture.SetupWithValidItemId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemUpdate);

            // Act
            await sut.UpdateAsync(_fixture.ItemUpdate);

            // Assert
            _fixture.VerifyStoringOldItem();
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemId_ShouldStoreNewItem()
        {
            // Arrange
            _fixture.SetupWithValidItemId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemUpdate);

            // Act
            await sut.UpdateAsync(_fixture.ItemUpdate);

            // Assert
            _fixture.VerifyStoringNewItem();
        }

        private sealed class UpdateAsyncWithoutTypesFixture : UpdateAsyncFixture
        {
            public ItemUpdate? ItemUpdate { get; private set; }

            public void SetupItemUpdate()
            {
                TestPropertyNotSetException.ThrowIfNull(OldItemId);

                ItemUpdate = new DomainTestBuilder<ItemUpdate>().FillConstructorWith("oldId", OldItemId.Value).Create();
            }

            private void SetupUpdatingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemUpdate);
                TestPropertyNotSetException.ThrowIfNull(NewItem);
                TestPropertyNotSetException.ThrowIfNull(OldItemMock);

                OldItemMock.SetupUpdateAsync(ItemUpdate, ValidatorMock.Object, DateTimeServiceMock.Object, NewItem);
            }

            public void VerifyUpdatingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemUpdate);
                TestPropertyNotSetException.ThrowIfNull(OldItemMock);

                OldItemMock.VerifyUpdateAsync(ItemUpdate, ValidatorMock.Object, DateTimeServiceMock.Object, Times.Once);
            }

            public void SetupWithValidItemId()
            {
                SetupOldItemId();
                SetupOldItem();
                SetupNewItem();
                SetupItemUpdate();
                SetupFindingOldItem();
                SetupUpdatingItem();
                SetupStoringOldItem();
                SetupStoringNewItem();
            }
        }
    }

    private abstract class UpdateAsyncFixture : ItemUpdateServiceFixture
    {
        protected ItemMock? OldItemMock;
        protected IItem? NewItem;
        public ItemId? OldItemId { get; private set; }

        public void SetupOldItemId()
        {
            OldItemId = ItemId.New;
        }

        protected void SetupOldItem(bool isTemporary = false)
        {
            TestPropertyNotSetException.ThrowIfNull(OldItemId);

            OldItemMock = new ItemMock(
                ItemMother.Initial()
                    .WithId(OldItemId.Value)
                    .WithIsTemporary(isTemporary)
                    .Create(),
                MockBehavior.Strict);
        }

        public void SetupOldItemTemporary()
        {
            SetupOldItem(true);
        }

        protected void SetupNewItem()
        {
            NewItem = ItemMother.Initial().Create();
        }

        public void SetupFindingOldItem()
        {
            TestPropertyNotSetException.ThrowIfNull(OldItemMock);
            TestPropertyNotSetException.ThrowIfNull(OldItemId);

            SetupFindingItem(OldItemId.Value, OldItemMock.Object);
        }

        public void SetupNotFindingOldItem()
        {
            TestPropertyNotSetException.ThrowIfNull(OldItemId);

            SetupFindingItem(OldItemId.Value, null);
        }

        protected void SetupStoringOldItem()
        {
            TestPropertyNotSetException.ThrowIfNull(OldItemMock);

            SetupStoringItem(OldItemMock.Object, OldItemMock.Object);
        }

        protected void SetupStoringNewItem()
        {
            TestPropertyNotSetException.ThrowIfNull(NewItem);

            SetupStoringItem(NewItem, NewItem);
        }

        public void VerifyStoringOldItem()
        {
            TestPropertyNotSetException.ThrowIfNull(OldItemMock);

            VerifyStoringItem(OldItemMock.Object, Times.Once);
        }

        public void VerifyStoringNewItem()
        {
            TestPropertyNotSetException.ThrowIfNull(NewItem);

            VerifyStoringItem(NewItem, Times.Once);
        }
    }

    private abstract class ItemUpdateServiceFixture
    {
        private readonly ItemRepositoryMock _itemRepositoryMock = new(MockBehavior.Strict);
        protected readonly DateTimeServiceMock DateTimeServiceMock = new(MockBehavior.Strict);
        protected readonly ValidatorMock ValidatorMock = new(MockBehavior.Strict);

        public ItemUpdateService CreateSut()
        {
            return new ItemUpdateService(
                _itemRepositoryMock.Object,
                ValidatorMock.Object,
                DateTimeServiceMock.Object);
        }

        protected void SetupFindingItem(ItemId itemId, IItem? item)
        {
            _itemRepositoryMock.SetupFindActiveByAsync(itemId, item);
        }

        protected void SetupStoringItem(IItem item, IItem returnedItem)
        {
            _itemRepositoryMock.SetupStoreAsync(item, returnedItem);
        }

        protected void VerifyStoringItem(IItem item, Func<Times> times)
        {
            _itemRepositoryMock.VerifyStoreAsync(item, times);
        }
    }
}