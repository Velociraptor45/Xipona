using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Items;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common.Extensions;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ShoppingLists.Reducers;

public class ShoppingListItemReducerTests
{
    public class OnLoadingPriceUpdaterPricesFinished
    {
        private readonly OnLoadingPriceUpdaterPricesFinishedFixture _fixture = new();

        [Fact]
        public void OnLoadingPriceUpdaterPricesFinished_WithPriceThatMatchesSelectedItem_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupActionWithPriceThatMatchesSelectedItem();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnLoadingPriceUpdaterPricesFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnLoadingPriceUpdaterPricesFinished_WithoutPriceThatMatchesSelectedItem_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupActionWithoutPriceThatMatchesSelectedItem();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnLoadingPriceUpdaterPricesFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadingPriceUpdaterPricesFinishedFixture : ShoppingListItemReducerFixture
        {
            public LoadingPriceUpdaterPricesFinishedAction? Action { get; set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        OtherItemTypePrices = InitialState.PriceUpdate.OtherItemTypePrices
                    }
                };
            }

            public void SetupActionWithPriceThatMatchesSelectedItem()
            {
                var prices = ExpectedState.PriceUpdate.OtherItemTypePrices.ToList();
                var additionalPrice = new DomainTestBuilder<ItemTypePrice>().Create() with
                {
                    ItemTypeId = ExpectedState.PriceUpdate.Item!.TypeId!.Value
                };
                prices.Add(additionalPrice);
                Action = new LoadingPriceUpdaterPricesFinishedAction(prices);
            }

            public void SetupActionWithoutPriceThatMatchesSelectedItem()
            {
                Action = new LoadingPriceUpdaterPricesFinishedAction(ExpectedState.PriceUpdate.OtherItemTypePrices);
            }
        }
    }

    public class OnRemoveItemFromBasket
    {
        private readonly OnRemoveItemFromBasketFixture _fixture = new();

        [Fact]
        public void OnRemoveItemFromBasket_WithActualItemWithoutType_ShouldRemoveItemFromBasket()
        {
            // Arrange
            _fixture.SetupActualItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnRemoveItemFromBasket(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRemoveItemFromBasket_WithActualItemWithType_ShouldRemoveItemFromBasket()
        {
            // Arrange
            _fixture.SetupActualItemId();
            _fixture.SetupItemTypeId();
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnRemoveItemFromBasket(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRemoveItemFromBasket_WithOfflineItem_ShouldRemoveItemFromBasket()
        {
            // Arrange
            _fixture.SetupOfflineItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnRemoveItemFromBasket(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRemoveItemFromBasketWithNoMatchingItem_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupOfflineItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnRemoveItemFromBasket(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRemoveItemFromBasketFixture : ShoppingListItemReducerFixture
        {
            private (int SectionIdx, int ItemIdx)? _chosenItemIndexes;
            private ShoppingListItemId? _itemId;
            private Guid? _itemTypeId;
            public RemoveItemFromBasketAction? Action { get; private set; }

            public void SetupActualItemId()
            {
                _itemId = new ShoppingListItemId(null, Guid.NewGuid());
            }

            public void SetupOfflineItemId()
            {
                _itemId = new ShoppingListItemId(Guid.NewGuid(), null);
            }

            public void SetupItemTypeIdNull()
            {
                _itemTypeId = null;
            }

            public void SetupItemTypeId()
            {
                _itemTypeId = Guid.NewGuid();
            }

            public void SetupInitialState()
            {
                TestPropertyNotSetException.ThrowIfNull(_chosenItemIndexes);

                var sectionIndex = _chosenItemIndexes.Value.SectionIdx;
                var itemIndex = _chosenItemIndexes.Value.ItemIdx;
                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                var section = sections[sectionIndex];
                var items = sections[sectionIndex].Items.ToList();
                var item = items[itemIndex];

                item = item with
                {
                    IsInBasket = true,
                    Hidden = true
                };

                items[itemIndex] = item;
                section = section with { Items = items };
                sections[sectionIndex] = section;

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections)
                    }
                };
            }

            public void SetupExpectedState()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);

                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                var (section, sectionIndex) = sections.Random();

                var items = section.Items.ToList();
                var (item, itemIndex) = items.Random();

                item = item with
                {
                    Id = _itemId,
                    TypeId = _itemTypeId,
                    IsInBasket = false,
                    Hidden = false
                };

                items[itemIndex] = item;
                section = section with { Items = items };
                sections[sectionIndex] = section;

                _chosenItemIndexes = (sectionIndex, itemIndex);

                ExpectedState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections)
                    }
                };
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);

                Action = new RemoveItemFromBasketAction(_itemId, _itemTypeId,
                    new DomainTestBuilder<string>().Create());
            }
        }
    }

    public class OnPutItemInBasket
    {
        private readonly OnPutItemInBasketFixture _fixture = new();

        [Fact]
        public void OnPutItemInBasket_WithActualItemWithoutType_ShouldPutItemInBasket()
        {
            // Arrange
            _fixture.SetupActualItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnPutItemInBasket(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnPutItemInBasket_WithActualItemWithType_ShouldPutItemInBasket()
        {
            // Arrange
            _fixture.SetupActualItemId();
            _fixture.SetupItemTypeId();
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnPutItemInBasket(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnPutItemInBasket_WithOfflineItem_ShouldPutItemInBasket()
        {
            // Arrange
            _fixture.SetupOfflineItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnPutItemInBasket(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnPutItemInBasket_WithNoMatchingItem_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupOfflineItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnPutItemInBasket(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnPutItemInBasketFixture : ShoppingListItemReducerFixture
        {
            private (int SectionIdx, int ItemIdx)? _chosenItemIndexes;
            private ShoppingListItemId? _itemId;
            private Guid? _itemTypeId;
            public PutItemInBasketAction? Action { get; private set; }

            public void SetupActualItemId()
            {
                _itemId = new ShoppingListItemId(null, Guid.NewGuid());
            }

            public void SetupOfflineItemId()
            {
                _itemId = new ShoppingListItemId(Guid.NewGuid(), null);
            }

            public void SetupItemTypeIdNull()
            {
                _itemTypeId = null;
            }

            public void SetupItemTypeId()
            {
                _itemTypeId = Guid.NewGuid();
            }

            public void SetupInitialState()
            {
                TestPropertyNotSetException.ThrowIfNull(_chosenItemIndexes);

                var sectionIndex = _chosenItemIndexes.Value.SectionIdx;
                var itemIndex = _chosenItemIndexes.Value.ItemIdx;
                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                var section = sections[sectionIndex];
                var items = sections[sectionIndex].Items.ToList();
                var item = items[itemIndex];

                item = item with
                {
                    IsInBasket = false,
                    Hidden = true
                };

                items[itemIndex] = item;
                section = section with { Items = items };
                sections[sectionIndex] = section;

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections)
                    }
                };
            }

            public void SetupExpectedState()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);

                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                var (section, sectionIndex) = sections.Random();

                var items = section.Items.ToList();
                var (item, itemIndex) = items.Random();

                item = item with
                {
                    Id = _itemId,
                    TypeId = _itemTypeId,
                    IsInBasket = true,
                    Hidden = false
                };

                items[itemIndex] = item;
                section = section with { Items = items };
                sections[sectionIndex] = section;

                _chosenItemIndexes = (sectionIndex, itemIndex);

                ExpectedState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections)
                    }
                };
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);

                Action = new PutItemInBasketAction(_itemId, _itemTypeId,
                    new DomainTestBuilder<string>().Create());
            }
        }
    }

    public class OnChangeItemQuantityFinished
    {
        private readonly OnChangeItemQuantityFinishedFixture _fixture = new();

        [Fact]
        public void OnChangeItemQuantityFinished_WithActualItemWithoutType_ShouldChangeItemQuantity()
        {
            // Arrange
            _fixture.SetupActualItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnChangeItemQuantityFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnChangeItemQuantityFinished_WithActualItemWithType_ShouldChangeItemQuantity()
        {
            // Arrange
            _fixture.SetupActualItemId();
            _fixture.SetupItemTypeId();
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnChangeItemQuantityFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnChangeItemQuantityFinished_WithOfflineItem_ShouldChangeItemQuantity()
        {
            // Arrange
            _fixture.SetupOfflineItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnChangeItemQuantityFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnChangeItemQuantityFinished_WithNoMatchingItem_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupOfflineItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnChangeItemQuantityFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnChangeItemQuantityFinishedFixture : ShoppingListItemReducerFixture
        {
            private (int SectionIdx, int ItemIdx)? _chosenItemIndexes;
            private ShoppingListItemId? _itemId;
            private Guid? _itemTypeId;
            private readonly float _quantity = new DomainTestBuilder<float>().Create();

            public ChangeItemQuantityFinishedAction? Action { get; private set; }

            public void SetupActualItemId()
            {
                _itemId = new ShoppingListItemId(null, Guid.NewGuid());
            }

            public void SetupOfflineItemId()
            {
                _itemId = new ShoppingListItemId(Guid.NewGuid(), null);
            }

            public void SetupItemTypeIdNull()
            {
                _itemTypeId = null;
            }

            public void SetupItemTypeId()
            {
                _itemTypeId = Guid.NewGuid();
            }

            public void SetupInitialState()
            {
                TestPropertyNotSetException.ThrowIfNull(_chosenItemIndexes);

                var sectionIndex = _chosenItemIndexes.Value.SectionIdx;
                var itemIndex = _chosenItemIndexes.Value.ItemIdx;
                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                var section = sections[sectionIndex];
                var items = sections[sectionIndex].Items.ToList();
                var item = items[itemIndex];

                item = item with
                {
                    Quantity = new DomainTestBuilder<float>().Create()
                };

                items[itemIndex] = item;
                section = section with { Items = items };
                sections[sectionIndex] = section;

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections)
                    }
                };
            }

            public void SetupExpectedState()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);

                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                var (section, sectionIndex) = sections.Random();

                var items = section.Items.ToList();
                var (item, itemIndex) = items.Random();

                item = item with
                {
                    Id = _itemId,
                    TypeId = _itemTypeId,
                    Quantity = _quantity
                };

                items[itemIndex] = item;
                section = section with { Items = items };
                sections[sectionIndex] = section;

                _chosenItemIndexes = (sectionIndex, itemIndex);

                ExpectedState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections)
                    }
                };
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);

                Action = new ChangeItemQuantityFinishedAction(_itemId, _itemTypeId, _quantity);
            }
        }
    }

    public class OnHideItemsInBasket
    {
        private readonly OnHideItemsInBasketFixture _fixture = new();

        [Fact]
        public void OnHideItemsInBasket_WithItemsInBasket_ShouldHideItemsInBasket()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = ShoppingListItemReducer.OnHideItemsInBasket(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnHideItemsInBasket_WithoutItemsInBasket_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupExpectedStateWithoutItemsInBasket();
            _fixture.SetupInitialState();

            // Act
            var result = ShoppingListItemReducer.OnHideItemsInBasket(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnHideItemsInBasketFixture : ShoppingListItemReducerFixture
        {
            public void SetupInitialState()
            {
                var sections = ExpectedState.ShoppingList!.Sections.ToList();

                for (int i = 0; i < sections.Count; i++)
                {
                    var section = sections[i];
                    var items = section.Items.ToList();
                    for (int j = 0; j < section.Items.Count; j++)
                    {
                        var item = items[j];
                        items[j] = item with { Hidden = false };
                    }
                    section = section with { Items = items };
                    sections[i] = section;
                }

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
                    }
                };
            }

            public void SetupExpectedState()
            {
                var section1 = new DomainTestBuilder<ShoppingListSection>().Create() with
                {
                    Items =
                    [
                        new DomainTestBuilder<ShoppingListItem>().Create() with { IsInBasket = true, Hidden = true },
                        new DomainTestBuilder<ShoppingListItem>().Create() with { IsInBasket = false, Hidden = false }
                    ]
                };
                var section2 = new DomainTestBuilder<ShoppingListSection>().Create() with
                {
                    Items =
                    [
                        new DomainTestBuilder<ShoppingListItem>().Create() with { IsInBasket = false, Hidden = false },
                        new DomainTestBuilder<ShoppingListItem>().Create() with { IsInBasket = true, Hidden = true }
                    ]
                };

                ExpectedState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList! with
                    {
                        Sections = new SortedSet<ShoppingListSection>([section1, section2], new SortingIndexComparer())
                    }
                };
            }

            public void SetupExpectedStateWithoutItemsInBasket()
            {
                var section1 = new DomainTestBuilder<ShoppingListSection>().Create() with
                {
                    Items =
                    [
                        new DomainTestBuilder<ShoppingListItem>().Create() with { IsInBasket = false, Hidden = false },
                        new DomainTestBuilder<ShoppingListItem>().Create() with { IsInBasket = false, Hidden = false }
                    ]
                };
                var section2 = new DomainTestBuilder<ShoppingListSection>().Create() with
                {
                    Items =
                    [
                        new DomainTestBuilder<ShoppingListItem>().Create() with { IsInBasket = false, Hidden = false },
                        new DomainTestBuilder<ShoppingListItem>().Create() with { IsInBasket = false, Hidden = false }
                    ]
                };

                ExpectedState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList! with
                    {
                        Sections = new SortedSet<ShoppingListSection>([section1, section2], new SortingIndexComparer())
                    }
                };
            }
        }
    }

    public class OnRemoveItemFromShoppingList
    {
        private readonly OnRemoveItemFromShoppingListFixture _fixture = new();

        [Fact]
        public void OnRemoveItemFromShoppingList_WithActualItemWithoutType_ShouldRemoveItem()
        {
            // Arrange
            _fixture.SetupActualItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnRemoveItemFromShoppingList(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRemoveItemFromShoppingList_WithActualItemWithType_ShouldRemoveItem()
        {
            // Arrange
            _fixture.SetupActualItemId();
            _fixture.SetupItemTypeId();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnRemoveItemFromShoppingList(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRemoveItemFromShoppingList_WithOfflineItem_ShouldRemoveItem()
        {
            // Arrange
            _fixture.SetupOfflineItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnRemoveItemFromShoppingList(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRemoveItemFromShoppingList_WithNoMatchingItem_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupActualItemId();
            _fixture.SetupItemTypeIdNull();
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListItemReducer.OnRemoveItemFromShoppingList(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRemoveItemFromShoppingListFixture : ShoppingListItemReducerFixture
        {
            private ShoppingListItemId? _itemId;
            private Guid? _itemTypeId;

            public RemoveItemFromShoppingListAction? Action { get; private set; }

            public void SetupActualItemId()
            {
                _itemId = new ShoppingListItemId(null, Guid.NewGuid());
            }

            public void SetupOfflineItemId()
            {
                _itemId = new ShoppingListItemId(Guid.NewGuid(), null);
            }

            public void SetupItemTypeIdNull()
            {
                _itemTypeId = null;
            }

            public void SetupItemTypeId()
            {
                _itemTypeId = Guid.NewGuid();
            }

            public void SetupInitialState()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);

                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                var (section, sectionIdx) = sections.Random();

                var items = section.Items.ToList();
                var item = new DomainTestBuilder<ShoppingListItem>().Create() with
                {
                    Id = _itemId,
                    TypeId = _itemTypeId
                };
                items.Add(item);

                section = section with { Items = items };
                sections[sectionIdx] = section;

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList! with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
                    }
                };
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);

                Action = new RemoveItemFromShoppingListAction(_itemId, _itemTypeId,
                    new DomainTestBuilder<string>().Create());
            }
        }
    }

    private abstract class ShoppingListItemReducerFixture
    {
        public ShoppingListState ExpectedState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
        public ShoppingListState InitialState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();

        public void SetupInitialStateEqualExpectedState()
        {
            InitialState = ExpectedState;
        }
    }
}