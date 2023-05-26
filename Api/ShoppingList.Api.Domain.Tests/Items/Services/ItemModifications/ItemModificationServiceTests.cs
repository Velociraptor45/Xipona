using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Shared;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.AutoFixture;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Items.Services.ItemModifications;

public class ItemModificationServiceTests
{
    private readonly LocalFixture _localFixture;

    public ItemModificationServiceTests()
    {
        _localFixture = new LocalFixture();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithNotFindingItem_ShouldThrowDomainException()
    {
        // Arrange
        _localFixture.SetupNotFindingItem();
        _localFixture.SetupModification();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        Func<Task> func = async () => await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
    }

    #region WithModifiedItemTypesEqualToExisting

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesEqualToExisting_ShouldRemoveNoItemFromAnyShoppingList()
    {
        // Arrange
        _localFixture.SetupForWithModifiedItemTypesEqualToExisting();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyRemovingNoItemTypesFromAnyShoppingList();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesEqualToExisting_ShouldStoreNoShoppingList()
    {
        // Arrange
        _localFixture.SetupForWithModifiedItemTypesEqualToExisting();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyStoringNoShoppingList();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesEqualToExisting_ShouldModifyItem()
    {
        // Arrange
        _localFixture.SetupForWithModifiedItemTypesEqualToExisting();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyModifyingItem();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesEqualToExisting_ShouldStoreItem()
    {
        // Arrange
        _localFixture.SetupForWithModifiedItemTypesEqualToExisting();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyStoringItem();
    }

    #endregion WithModifiedItemTypesEqualToExisting

    #region WithModifiedItemTypesNotContainingAllExisting

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesNotContainingAllExisting_ShouldRemoveNotExistingItemTypesFromShoppingList()
    {
        // Arrange
        _localFixture.SetupForWithModifiedItemTypesNotContainingAllExisting();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyRemovingNotExistingItemTypesFromAllShoppingLists();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesNotContainingAllExisting_ShouldNotRemoveStillExistingItemTypesFromShoppingLists()
    {
        // Arrange
        _localFixture.SetupForWithModifiedItemTypesNotContainingAllExisting();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyRemovingNoStillExistingItemTypesFromAnyShoppingList();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesNotContainingAllExisting_ShouldStoreShoppingListsOfNotExistingItemTypes()
    {
        // Arrange
        _localFixture.SetupForWithModifiedItemTypesNotContainingAllExisting();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyStoringShoppingListsOfNotExistingItemTypes();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesNotContainingAllExisting_ShouldModifyItem()
    {
        // Arrange
        _localFixture.SetupForWithModifiedItemTypesNotContainingAllExisting();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyModifyingItem();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesNotContainingAllExisting_ShouldStoreItem()
    {
        // Arrange
        _localFixture.SetupForWithModifiedItemTypesNotContainingAllExisting();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyStoringItem();
    }

    #endregion WithModifiedItemTypesNotContainingAllExisting

    #region WithModifiedItemTypesContainingNew

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesContainingNew_ShouldRemoveNoItemFromAnyShoppingList()
    {
        // Arrange
        _localFixture.SetupForWithModifiedItemTypesContainingNew();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyRemovingNoItemTypesFromAnyShoppingList();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesContainingNew_ShouldStoreNoShoppingList()
    {
        // Arrange
        _localFixture.SetupForWithModifiedItemTypesContainingNew();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyStoringNoShoppingList();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesContainingNew_ShouldModifyItem()
    {
        // Arrange
        _localFixture.SetupForWithModifiedItemTypesContainingNew();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyModifyingItem();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesContainingNew_ShouldStoreItem()
    {
        // Arrange
        _localFixture.SetupForWithModifiedItemTypesContainingNew();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyStoringItem();
    }

    #endregion WithModifiedItemTypesContainingNew

    #region WithSameItemsButStoresChanged

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithSameItemsButStoresChanged_ShouldRemoveItemTypeWithChangedStoreFromShoppingLists()
    {
        // Arrange
        _localFixture.SetupForWithSameItemsButStoresChanged();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyRemovingItemTypeWithChangedStoreFromAllShoppingLists();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithSameItemsButStoresChanged_ShouldNotRemoveStillExistingItemTypesFromShoppingLists()
    {
        // Arrange
        _localFixture.SetupForWithSameItemsButStoresChanged();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyRemovingNoStillExistingItemTypesFromAnyShoppingList();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithSameItemsButStoresChanged_ShouldStoreShoppingListOfItemTypeWithChangedStore()
    {
        // Arrange
        _localFixture.SetupForWithSameItemsButStoresChanged();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyStoringShoppingListsOfItemTypeWithChangedStore();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithSameItemsButStoresChanged_ShouldModifyItem()
    {
        // Arrange
        _localFixture.SetupForWithSameItemsButStoresChanged();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyModifyingItem();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithSameItemsButStoresChanged_ShouldStoreItem()
    {
        // Arrange
        _localFixture.SetupForWithSameItemsButStoresChanged();
        var sut = _localFixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_localFixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_localFixture.Modification);

        // Assert
        _localFixture.VerifyStoringItem();
    }

    #endregion WithSameItemsButStoresChanged

    private class LocalFixture : ItemModificationServiceFixture
    {
        private readonly Fixture _fixture;
        private readonly CommonFixture _commonFixture = new();

        private Dictionary<ItemTypeId, List<ShoppingListMock>>? _shoppingListDict;
        private List<IItemType>? _notExistingItemTypes;
        private Tuple<ItemTypeId, StoreId>? _removedStoreByTypeId;
        private ItemMock? _itemMock;
        private List<ItemTypeModification>? _modifiedItemTypes;

        public LocalFixture()
        {
            _fixture = _commonFixture.GetNewFixture();
        }

        public ItemWithTypesModification? Modification { get; private set; }

        public void SetupModification()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            if (_modifiedItemTypes != null)
            {
                _fixture.ConstructorArgumentFor<ItemWithTypesModification, IEnumerable<ItemTypeModification>>("itemTypes",
                    _modifiedItemTypes);
            }
            _fixture.ConstructorArgumentFor<ItemWithTypesModification, ItemId>("id", _itemMock.Object.Id);
            Modification = _fixture.Create<ItemWithTypesModification>();
        }

        private void SetupFindingItem()
        {
            _itemMock = new ItemMock(ItemMother.InitialWithTypes().Create(), MockBehavior.Strict);
            ItemRepositoryMock.SetupFindActiveByAsync(_itemMock.Object.Id, _itemMock.Object);
        }

        public void SetupNotFindingItem()
        {
            _itemMock = new ItemMock(ItemMother.InitialWithTypes().Create(), MockBehavior.Strict);
            ItemRepositoryMock.SetupFindActiveByAsync(_itemMock.Object.Id, null);
        }

        private void SetupItemReturningTypes()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            TestPropertyNotSetException.ThrowIfNull(Modification);
            var originalItemTypes = _itemMock.Object.ItemTypes;
            _itemMock.SetupItemTypes()
                .Returns(() =>
                    _itemMock.ModifyWithTypeCalled ?
                        new List<IItemType>(originalItemTypes
                            .Select(t => new ItemType(
                                t.Id,
                                t.Name,
                                t.Availabilities,
                                null,
                                Modification.ItemTypes.All(it => it.Id != t.Id))))
                            .AsReadOnly() :
                        originalItemTypes);
        }

        private void SetupModifyingItem()
        {
            TestPropertyNotSetException.ThrowIfNull(Modification);
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            _itemMock.SetupModifyAsync(Modification, ValidatorMock.Object);
        }

        private void SetupModifiedItemTypesEqualToExisting()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            _modifiedItemTypes = _itemMock.Object.ItemTypes
                .Select(t => new ItemTypeModification(t.Id, t.Name, t.Availabilities))
                .ToList();
        }

        private void SetupModifiedItemTypesContainingNew()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            _modifiedItemTypes = _itemMock.Object.ItemTypes
                .Select(t => new ItemTypeModification(t.Id, t.Name, t.Availabilities))
                .Union(_fixture.CreateMany<ItemTypeModification>(1))
                .ToList();
        }

        private void SetupModifiedItemTypesNotContainingAllExisting()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            var choseItem = _commonFixture.ChooseRandom(_itemMock.Object.ItemTypes);
            _modifiedItemTypes = new ItemTypeModification(choseItem.Id, choseItem.Name, choseItem.Availabilities)
                .ToMonoList();
            _notExistingItemTypes = _itemMock.Object.ItemTypes.Except(choseItem.ToMonoList()).ToList();
        }

        private void SetupModifiedItemTypesEqualToExistingButOneWithDifferentStores()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            var removedOne = false;
            _modifiedItemTypes = _itemMock.Object.ItemTypes
                .Select(t =>
                {
                    if (removedOne)
                        return new ItemTypeModification(t.Id, t.Name, t.Availabilities);

                    var av = _commonFixture.RemoveRandom(t.Availabilities, 1).ToList();
                    var removedAvailability = t.Availabilities.Except(av).Single();
                    _removedStoreByTypeId = new Tuple<ItemTypeId, StoreId>(t.Id, removedAvailability.StoreId);
                    removedOne = true;
                    return new ItemTypeModification(t.Id, t.Name, av);
                })
                .ToList();
        }

        private void SetupFindingShoppingListsWithoutStoreChanges()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            var dict = new Dictionary<ItemTypeId, List<ShoppingListMock>>();
            foreach (var type in _itemMock.Object.ItemTypes)
            {
                var storeId = StoreIdMother.OneFrom(type.Availabilities).Create();
                var shoppingLists = new ShoppingListBuilder()
                    .WithStoreId(storeId)
                    .CreateMany(1)
                    .Select(s => new ShoppingListMock(s))
                    .ToList();
                ShoppingListRepositoryMock.SetupFindByAsync(type.Id, shoppingLists.Select(s => s.Object));
                dict.Add(type.Id, shoppingLists);
            }

            _shoppingListDict = dict;
        }

        private void SetupFindingShoppingListsWithStoreChanges()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            TestPropertyNotSetException.ThrowIfNull(_removedStoreByTypeId);
            var dict = new Dictionary<ItemTypeId, List<ShoppingListMock>>();
            foreach (var type in _itemMock.Object.ItemTypes)
            {
                var storeId =
                    type.Id == _removedStoreByTypeId.Item1
                        ? new StoreIdBuilder().Create()
                        : StoreIdMother.OneFrom(type.Availabilities).Create();
                var shoppingListMocks = new ShoppingListBuilder()
                    .WithStoreId(storeId)
                    .CreateMany(1)
                    .Select(s => new ShoppingListMock(s))
                    .ToList();

                ShoppingListRepositoryMock.SetupFindByAsync(type.Id, shoppingListMocks.Select(m => m.Object));
                dict.Add(type.Id, shoppingListMocks);
            }

            _shoppingListDict = dict;
        }

        private void SetupStoringShoppingListsOfItemTypeWithChangedStore()
        {
            TestPropertyNotSetException.ThrowIfNull(_shoppingListDict);
            TestPropertyNotSetException.ThrowIfNull(_removedStoreByTypeId);
            var lists = _shoppingListDict.Where(kv => kv.Key == _removedStoreByTypeId.Item1);
            foreach (var (_, shoppingListMocks) in lists)
            {
                foreach (var mock in shoppingListMocks)
                {
                    ShoppingListRepositoryMock.SetupStoreAsync(mock.Object);
                }
            }
        }

        private void SetupStoringItem()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            ItemRepositoryMock.SetupStoreAsync(_itemMock.Object, _itemMock.Object);
        }

        private void SetupStoringShoppingListsOfNotExistingItemTypes()
        {
            TestPropertyNotSetException.ThrowIfNull(_shoppingListDict);
            TestPropertyNotSetException.ThrowIfNull(_notExistingItemTypes);
            var lists = _shoppingListDict.Where(kv => _notExistingItemTypes.Any(t => t.Id == kv.Key));
            foreach (var (_, shoppingListMocks) in lists)
            {
                foreach (var mock in shoppingListMocks)
                {
                    ShoppingListRepositoryMock.SetupStoreAsync(mock.Object);
                }
            }
        }

        public void SetupStoringShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(_shoppingListDict);
            foreach (var listMock in _shoppingListDict.Values.SelectMany(l => l))
            {
                ShoppingListRepositoryMock.SetupStoreAsync(listMock.Object);
            }
        }

        #region Verification

        public void VerifyRemovingNoItemTypesFromAnyShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(_shoppingListDict);
            TestPropertyNotSetException.ThrowIfNull(Modification);
            foreach (var (itemTypeId, shoppingListMocks) in _shoppingListDict)
            {
                foreach (var mock in shoppingListMocks)
                {
                    mock.VerifyRemoveItemNever(Modification.Id, itemTypeId);
                }
            }
        }

        public void VerifyRemovingNoStillExistingItemTypesFromAnyShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(_shoppingListDict);
            TestPropertyNotSetException.ThrowIfNull(_modifiedItemTypes);
            TestPropertyNotSetException.ThrowIfNull(Modification);
            var lists = _shoppingListDict.Where(kv => _modifiedItemTypes.Any(t => t.Id == kv.Key)
                                                      && (_removedStoreByTypeId == null || kv.Key != _removedStoreByTypeId.Item1));
            foreach (var (itemTypeId, shoppingListMocks) in lists)
            {
                foreach (var mock in shoppingListMocks)
                {
                    mock.VerifyRemoveItemNever(Modification.Id, itemTypeId);
                }
            }
        }

        public void VerifyRemovingNotExistingItemTypesFromAllShoppingLists()
        {
            TestPropertyNotSetException.ThrowIfNull(_shoppingListDict);
            TestPropertyNotSetException.ThrowIfNull(_notExistingItemTypes);
            TestPropertyNotSetException.ThrowIfNull(Modification);
            var lists = _shoppingListDict.Where(kv => _notExistingItemTypes.Any(t => t.Id == kv.Key));
            foreach (var (itemTypeId, shoppingListMocks) in lists)
            {
                foreach (var mock in shoppingListMocks)
                {
                    mock.VerifyRemoveItem(Modification.Id, itemTypeId, Times.Once);
                }
            }
        }

        public void VerifyRemovingItemTypeWithChangedStoreFromAllShoppingLists()
        {
            TestPropertyNotSetException.ThrowIfNull(_shoppingListDict);
            TestPropertyNotSetException.ThrowIfNull(_removedStoreByTypeId);
            TestPropertyNotSetException.ThrowIfNull(Modification);
            var lists = _shoppingListDict.Where(kv => kv.Key == _removedStoreByTypeId.Item1);
            foreach (var (itemTypeId, shoppingListMocks) in lists)
            {
                foreach (var mock in shoppingListMocks)
                {
                    mock.VerifyRemoveItem(Modification.Id, itemTypeId, Times.Once);
                }
            }
        }

        public void VerifyStoringShoppingListsOfNotExistingItemTypes()
        {
            TestPropertyNotSetException.ThrowIfNull(_shoppingListDict);
            TestPropertyNotSetException.ThrowIfNull(_notExistingItemTypes);
            var lists = _shoppingListDict.Where(kv => _notExistingItemTypes.Any(t => t.Id == kv.Key));
            foreach (var (_, shoppingListMocks) in lists)
            {
                foreach (var mock in shoppingListMocks)
                {
                    ShoppingListRepositoryMock.VerifyStoreAsync(mock.Object, Times.Once);
                }
            }
        }

        public void VerifyStoringShoppingListsOfItemTypeWithChangedStore()
        {
            TestPropertyNotSetException.ThrowIfNull(_shoppingListDict);
            TestPropertyNotSetException.ThrowIfNull(_removedStoreByTypeId);
            var lists = _shoppingListDict.Where(kv => kv.Key == _removedStoreByTypeId.Item1);
            foreach (var (_, shoppingListMocks) in lists)
            {
                foreach (var mock in shoppingListMocks)
                {
                    ShoppingListRepositoryMock.VerifyStoreAsync(mock.Object, Times.Once);
                }
            }
        }

        public void VerifyStoringNoShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(_shoppingListDict);
            foreach (var (_, shoppingListMocks) in _shoppingListDict)
            {
                foreach (var mock in shoppingListMocks)
                {
                    ShoppingListRepositoryMock.VerifyStoreAsync(mock.Object, Times.Never);
                }
            }
        }

        public void VerifyModifyingItem()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            TestPropertyNotSetException.ThrowIfNull(Modification);
            _itemMock.VerifyModifyAsync(Modification, ValidatorMock.Object, Times.Once);
        }

        public void VerifyStoringItem()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            ItemRepositoryMock.VerifyStoreAsync(_itemMock.Object, Times.Once);
        }

        #endregion Verification

        #region SetupAggregates

        public void SetupForWithModifiedItemTypesEqualToExisting()
        {
            SetupFindingItem();
            SetupModifiedItemTypesEqualToExisting();
            SetupModification();
            SetupModifyingItem();
            SetupItemReturningTypes();
            SetupFindingShoppingListsWithoutStoreChanges();
            SetupStoringItem();
            SetupStoringShoppingList();
        }

        public void SetupForWithModifiedItemTypesNotContainingAllExisting()
        {
            SetupFindingItem();
            SetupModifiedItemTypesNotContainingAllExisting();
            SetupModification();
            SetupModifyingItem();
            SetupItemReturningTypes();
            SetupFindingShoppingListsWithoutStoreChanges();
            SetupStoringShoppingListsOfNotExistingItemTypes();
            SetupStoringItem();
            SetupStoringShoppingList();
        }

        public void SetupForWithModifiedItemTypesContainingNew()
        {
            SetupFindingItem();
            SetupModifiedItemTypesContainingNew();
            SetupModification();
            SetupModifyingItem();
            SetupItemReturningTypes();
            SetupFindingShoppingListsWithoutStoreChanges();
            SetupStoringItem();
            SetupStoringShoppingList();
        }

        public void SetupForWithSameItemsButStoresChanged()
        {
            SetupFindingItem();
            SetupModifiedItemTypesEqualToExistingButOneWithDifferentStores();
            SetupModification();
            SetupModifyingItem();
            SetupItemReturningTypes();
            SetupFindingShoppingListsWithStoreChanges();
            SetupStoringShoppingListsOfItemTypeWithChangedStore();
            SetupStoringItem();
            SetupStoringShoppingList();
        }

        #endregion SetupAggregates
    }

    public class TransferToSectionAsync
    {
        private readonly TransferToSectionAsyncFixture _fixture;

        public TransferToSectionAsync()
        {
            _fixture = new TransferToSectionAsyncFixture();
        }

        [Fact]
        public async Task TransferToSectionAsync_WithNotFindingStore_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupSectionIds();
            _fixture.SetupStoreContainingNewSection();
            _fixture.SetupNotFindingStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);

            // Act
            var func = async () =>
                await sut.TransferToSectionAsync(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.StoreNotFound);
        }

        [Fact]
        public async Task TransferToSectionAsync_WithStoreNotContainingNewSection_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupSectionIds();
            _fixture.SetupStoreNotContainingNewSection();
            _fixture.SetupFindingStore();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);

            // Act
            var func = async () =>
                await sut.TransferToSectionAsync(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.OldAndNewSectionNotInSameStore);
        }

        [Fact]
        public async Task TransferToSectionAsync_WithValidData_ShouldTransferItem()
        {
            // Arrange
            _fixture.SetupSectionIds();
            _fixture.SetupStoreContainingNewSection();
            _fixture.SetupFindingStore();
            _fixture.SetupItem();
            _fixture.SetupFindingItem();
            _fixture.SetupStoringItem();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);

            // Act
            await sut.TransferToSectionAsync(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            _fixture.VerifyTransferringItem();
        }

        [Fact]
        public async Task TransferToSectionAsync_WithValidData_ShouldStoreItem()
        {
            // Arrange
            _fixture.SetupSectionIds();
            _fixture.SetupStoreContainingNewSection();
            _fixture.SetupFindingStore();
            _fixture.SetupItem();
            _fixture.SetupFindingItem();
            _fixture.SetupStoringItem();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);

            // Act
            await sut.TransferToSectionAsync(_fixture.OldSectionId.Value, _fixture.NewSectionId.Value);

            // Assert
            _fixture.VerifyStoringItem();
        }

        private sealed class TransferToSectionAsyncFixture : ItemModificationServiceFixture
        {
            private StoreMock? _storeMock;
            private ItemMock? _itemMock;

            public SectionId? NewSectionId { get; set; }
            public SectionId? OldSectionId { get; set; }

            public void SetupSectionIds()
            {
                OldSectionId = SectionId.New;
                NewSectionId = SectionId.New;
            }

            public void SetupStoreContainingNewSection()
            {
                TestPropertyNotSetException.ThrowIfNull(NewSectionId);

                _storeMock = new StoreMock(MockBehavior.Strict, new StoreBuilder().Create());
                _storeMock.SetupContainsSection(NewSectionId.Value, true);
            }

            public void SetupStoreNotContainingNewSection()
            {
                TestPropertyNotSetException.ThrowIfNull(NewSectionId);

                _storeMock = new StoreMock(MockBehavior.Strict, new StoreBuilder().Create());
                _storeMock.SetupContainsSection(NewSectionId.Value, false);
            }

            public void SetupFindingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                StoreRepositoryMock.SetupFindActiveByAsync(OldSectionId.Value, _storeMock.Object);
            }

            public void SetupNotFindingStore()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);
                TestPropertyNotSetException.ThrowIfNull(_storeMock);

                StoreRepositoryMock.SetupFindActiveByAsync(OldSectionId.Value, null);
            }

            public void SetupItem()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);
                TestPropertyNotSetException.ThrowIfNull(NewSectionId);

                _itemMock = new ItemMock(ItemMother.Initial().Create(), MockBehavior.Strict);
                _itemMock.SetupTransferToDefaultSection(OldSectionId.Value, NewSectionId.Value);
            }

            public void SetupFindingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);
                TestPropertyNotSetException.ThrowIfNull(_itemMock);

                ItemRepositoryMock.SetupFindActiveByAsync(OldSectionId.Value, _itemMock.Object.ToMonoList());
            }

            public void SetupStoringItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMock);

                ItemRepositoryMock.SetupStoreAsync(_itemMock.Object, _itemMock.Object);
            }

            public void VerifyTransferringItem()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);
                TestPropertyNotSetException.ThrowIfNull(NewSectionId);
                TestPropertyNotSetException.ThrowIfNull(_itemMock);

                _itemMock.VerifyTransferToDefaultSection(OldSectionId.Value, NewSectionId.Value, Times.Once);
            }

            public void VerifyStoringItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemMock);

                ItemRepositoryMock.VerifyStoreAsync(_itemMock.Object, Times.Once);
            }
        }
    }

    public abstract class ItemModificationServiceFixture
    {
        protected readonly ItemRepositoryMock ItemRepositoryMock;
        protected readonly ValidatorMock ValidatorMock;
        protected readonly StoreRepositoryMock StoreRepositoryMock;
        protected readonly ShoppingListRepositoryMock ShoppingListRepositoryMock;

        protected ItemModificationServiceFixture()
        {
            ItemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
            ValidatorMock = new ValidatorMock(MockBehavior.Strict);
            StoreRepositoryMock = new StoreRepositoryMock(MockBehavior.Strict);
            ShoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
        }

        public ItemModificationService CreateSut()
        {
            return new ItemModificationService(
                ItemRepositoryMock.Object,
                _ => ValidatorMock.Object,
                ShoppingListRepositoryMock.Object,
                _ => StoreRepositoryMock.Object,
                default);
        }
    }
}