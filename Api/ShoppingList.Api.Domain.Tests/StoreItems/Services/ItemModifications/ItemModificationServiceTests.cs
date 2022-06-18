using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.Shared;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Ports;
using ShoppingList.Api.Domain.TestKit.StoreItems.Services.Validation;
using ShoppingList.Api.Domain.TestKit.Stores.Models;
using ShoppingList.Api.TestTools.AutoFixture;
using ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Services.ItemModifications;

public class ItemModificationServiceTests
{
    private readonly LocalFixture _fixture;

    public ItemModificationServiceTests()
    {
        _fixture = new LocalFixture();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithNotFindingItem_ShouldThrowDomainException()
    {
        // Arrange
        _fixture.SetupNotFindingItem();
        _fixture.SetupModification();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        Func<Task> func = async () => await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
    }

    #region WithModifiedItemTypesEqualToExisting

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesEqualToExisting_ShouldRemoveNoItemFromAnyShoppingList()
    {
        // Arrange
        _fixture.SetupForWithModifiedItemTypesEqualToExisting();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyRemovingNoItemTypesFromAnyShoppingList();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesEqualToExisting_ShouldStoreNoShoppingList()
    {
        // Arrange
        _fixture.SetupForWithModifiedItemTypesEqualToExisting();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyStoringNoShoppingList();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesEqualToExisting_ShouldModifyItem()
    {
        // Arrange
        _fixture.SetupForWithModifiedItemTypesEqualToExisting();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyModifyingItem();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesEqualToExisting_ShouldStoreItem()
    {
        // Arrange
        _fixture.SetupForWithModifiedItemTypesEqualToExisting();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyStoringItem();
    }

    #endregion WithModifiedItemTypesEqualToExisting

    #region WithModifiedItemTypesNotContainingAllExisting

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesNotContainingAllExisting_ShouldRemoveNotExistingItemTypesFromShoppingList()
    {
        // Arrange
        _fixture.SetupForWithModifiedItemTypesNotContainingAllExisting();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyRemovingNotExistingItemTypesFromAllShoppingLists();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesNotContainingAllExisting_ShouldNotRemoveStillExistingItemTypesFromShoppingLists()
    {
        // Arrange
        _fixture.SetupForWithModifiedItemTypesNotContainingAllExisting();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyRemovingNoStillExistingItemTypesFromAnyShoppingList();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesNotContainingAllExisting_ShouldStoreShoppingListsOfNotExistingItemTypes()
    {
        // Arrange
        _fixture.SetupForWithModifiedItemTypesNotContainingAllExisting();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyStoringShoppingListsOfNotExistingItemTypes();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesNotContainingAllExisting_ShouldModifyItem()
    {
        // Arrange
        _fixture.SetupForWithModifiedItemTypesNotContainingAllExisting();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyModifyingItem();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesNotContainingAllExisting_ShouldStoreItem()
    {
        // Arrange
        _fixture.SetupForWithModifiedItemTypesNotContainingAllExisting();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyStoringItem();
    }

    #endregion WithModifiedItemTypesNotContainingAllExisting

    #region WithModifiedItemTypesContainingNew

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesContainingNew_ShouldRemoveNoItemFromAnyShoppingList()
    {
        // Arrange
        _fixture.SetupForWithModifiedItemTypesContainingNew();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyRemovingNoItemTypesFromAnyShoppingList();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesContainingNew_ShouldStoreNoShoppingList()
    {
        // Arrange
        _fixture.SetupForWithModifiedItemTypesContainingNew();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyStoringNoShoppingList();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesContainingNew_ShouldModifyItem()
    {
        // Arrange
        _fixture.SetupForWithModifiedItemTypesContainingNew();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyModifyingItem();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithModifiedItemTypesContainingNew_ShouldStoreItem()
    {
        // Arrange
        _fixture.SetupForWithModifiedItemTypesContainingNew();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyStoringItem();
    }

    #endregion WithModifiedItemTypesContainingNew

    #region WithSameItemsButStoresChanged

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithSameItemsButStoresChanged_ShouldRemoveItemTypeWithChangedStoreFromShoppingLists()
    {
        // Arrange
        _fixture.SetupForWithSameItemsButStoresChanged();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyRemovingItemTypeWithChangedStoreFromAllShoppingLists();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithSameItemsButStoresChanged_ShouldNotRemoveStillExistingItemTypesFromShoppingLists()
    {
        // Arrange
        _fixture.SetupForWithSameItemsButStoresChanged();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyRemovingNoStillExistingItemTypesFromAnyShoppingList();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithSameItemsButStoresChanged_ShouldStoreShoppingListOfItemTypeWithChangedStore()
    {
        // Arrange
        _fixture.SetupForWithSameItemsButStoresChanged();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyStoringShoppingListsOfItemTypeWithChangedStore();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithSameItemsButStoresChanged_ShouldModifyItem()
    {
        // Arrange
        _fixture.SetupForWithSameItemsButStoresChanged();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyModifyingItem();
    }

    [Fact]
    public async Task ModifyItemWithTypesAsync_WithSameItemsButStoresChanged_ShouldStoreItem()
    {
        // Arrange
        _fixture.SetupForWithSameItemsButStoresChanged();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

        // Act
        await sut.ModifyItemWithTypesAsync(_fixture.Modification);

        // Assert
        _fixture.VerifyStoringItem();
    }

    #endregion WithSameItemsButStoresChanged

    private class LocalFixture
    {
        private readonly Fixture _fixture;
        private readonly ItemRepositoryMock _itemRepositoryMock;
        private readonly ValidatorMock _validatorMock;
        private readonly CommonFixture _commonFixture = new();
        private readonly ShoppingListRepositoryMock _shoppingListRepositoryMock;

        private Dictionary<ItemTypeId, List<ShoppingListMock>>? _shoppingListDict;
        private List<IItemType>? _notExistingItemTypes;
        private Tuple<ItemTypeId, StoreId>? _removedStoreByTypeId;
        private StoreItemMock? _itemMock;
        private List<ItemTypeModification>? _modifiedItemTypes;

        public LocalFixture()
        {
            _fixture = _commonFixture.GetNewFixture();
            _itemRepositoryMock = new ItemRepositoryMock(MockBehavior.Strict);
            _validatorMock = new ValidatorMock(MockBehavior.Strict);
            _shoppingListRepositoryMock = new ShoppingListRepositoryMock(MockBehavior.Strict);
        }

        public ItemWithTypesModification? Modification { get; private set; }

        public ItemModificationService CreateSut()
        {
            return new ItemModificationService(
                _itemRepositoryMock.Object,
                _ => _validatorMock.Object,
                _shoppingListRepositoryMock.Object,
                default);
        }

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
            _itemMock = new StoreItemMock(StoreItemMother.InitialWithTypes().Create(), MockBehavior.Strict);
            _itemRepositoryMock.SetupFindByAsync(_itemMock.Object.Id, _itemMock.Object);
        }

        public void SetupNotFindingItem()
        {
            _itemMock = new StoreItemMock(StoreItemMother.InitialWithTypes().Create(), MockBehavior.Strict);
            _itemRepositoryMock.SetupFindByAsync(_itemMock.Object.Id, null);
        }

        private void SetupItemReturningTypes()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            TestPropertyNotSetException.ThrowIfNull(Modification);
            var originalItemTypes = _itemMock.Object.ItemTypes;
            _itemMock.SetupItemTypes()
                .Returns(() =>
                    _itemMock.ModifyWithTypeCalled ?
                        new List<IItemType>(Modification.ItemTypes
                            .Select(t => new ItemType(t.Id!.Value, t.Name, t.Availabilities))).AsReadOnly() :
                        originalItemTypes);
        }

        private void SetupModifyingItem()
        {
            TestPropertyNotSetException.ThrowIfNull(Modification);
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            _itemMock.SetupModifyAsync(Modification, _validatorMock.Object);
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
                _shoppingListRepositoryMock.SetupFindByAsync(type.Id, shoppingLists.Select(s => s.Object));
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

                _shoppingListRepositoryMock.SetupFindByAsync(type.Id, shoppingListMocks.Select(m => m.Object));
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
                    _shoppingListRepositoryMock.SetupStoreAsync(mock.Object);
                }
            }
        }

        private void SetupStoringItem()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            _itemRepositoryMock.SetupStoreAsync(_itemMock.Object, _itemMock.Object);
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
                    _shoppingListRepositoryMock.SetupStoreAsync(mock.Object);
                }
            }
        }

        public void SetupStoringShoppingList()
        {
            TestPropertyNotSetException.ThrowIfNull(_shoppingListDict);
            foreach (var listMock in _shoppingListDict.Values.SelectMany(l => l))
            {
                _shoppingListRepositoryMock.SetupStoreAsync(listMock.Object);
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
                    _shoppingListRepositoryMock.VerifyStoreAsync(mock.Object, Times.Once);
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
                    _shoppingListRepositoryMock.VerifyStoreAsync(mock.Object, Times.Once);
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
                    _shoppingListRepositoryMock.VerifyStoreAsync(mock.Object, Times.Never);
                }
            }
        }

        public void VerifyModifyingItem()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            TestPropertyNotSetException.ThrowIfNull(Modification);
            _itemMock.VerifyModifyAsync(Modification, _validatorMock.Object, Times.Once);
        }

        public void VerifyStoringItem()
        {
            TestPropertyNotSetException.ThrowIfNull(_itemMock);
            _itemRepositoryMock.VerifyStoreAsync(_itemMock.Object, Times.Once);
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
}