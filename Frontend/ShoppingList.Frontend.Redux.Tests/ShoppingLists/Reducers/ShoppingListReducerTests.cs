using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ShoppingLists.Reducers;

public class ShoppingListReducerTests
{
    public class OnShoppingListEntered
    {
        private readonly OnShoppingListEnteredFixture _fixture = new();

        [Fact]
        public void OnShoppingListEntered_WithValidData_ShouldResetState()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ShoppingListReducer.OnShoppingListEntered(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnShoppingListEnteredFixture : ShoppingListReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    QuantityTypes = new DomainTestBuilder<QuantityType>().CreateMany(2).ToList(),
                    QuantityTypesInPacket = new DomainTestBuilder<QuantityTypeInPacket>().CreateMany(2).ToList(),
                    Stores = new AllActiveStores(new DomainTestBuilder<ShoppingListStore>().CreateMany(2).ToList()),
                    SelectedStoreId = Guid.NewGuid(),
                    ItemsInBasketVisible = false,
                    EditModeActive = true,
                    ShoppingList = new DomainTestBuilder<ShoppingListModel>().Create(),
                    SearchBar = new DomainTestBuilder<SearchBar>().Create()
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    SelectedStoreId = Guid.Empty,
                    ItemsInBasketVisible = true,
                    EditModeActive = false,
                    ShoppingList = null,
                    SearchBar = ExpectedState.SearchBar with
                    {
                        Input = string.Empty,
                        Results = new List<SearchItemForShoppingListResult>()
                    }
                };
            }
        }
    }

    public class OnLoadQuantityTypesFinished
    {
        private readonly OnLoadQuantityTypesFinishedFixture _fixture = new();

        [Fact]
        public void OnLoadQuantityTypesFinished_WithValidData_ShouldSetQuantityTypes()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListReducer.OnLoadQuantityTypesFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadQuantityTypesFinishedFixture : ShoppingListReducerFixture
        {
            public LoadQuantityTypesFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    QuantityTypes = new DomainTestBuilder<QuantityType>().CreateMany(2).ToList()
                };
            }

            public void SetupAction()
            {
                Action = new LoadQuantityTypesFinishedAction(ExpectedState.QuantityTypes);
            }
        }
    }

    public class OnLoadQuantityTypesInPacketFinished
    {
        private readonly OnLoadQuantityTypesInPacketFinishedFixture _fixture = new();

        [Fact]
        public void OnLoadQuantityTypesInPacketFinished_WithValidData_ShouldSetQuantityTypesInPacket()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListReducer.OnLoadQuantityTypesInPacketFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadQuantityTypesInPacketFinishedFixture : ShoppingListReducerFixture
        {
            public LoadQuantityTypesInPacketFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    QuantityTypesInPacket = new DomainTestBuilder<QuantityTypeInPacket>().CreateMany(2).ToList()
                };
            }

            public void SetupAction()
            {
                Action = new LoadQuantityTypesInPacketFinishedAction(ExpectedState.QuantityTypesInPacket);
            }
        }
    }

    public class OnLoadAllActiveStoresFinished
    {
        private readonly OnLoadAllActiveStoresFinishedFixture _fixture = new();

        [Fact]
        public void OnLoadAllActiveStoresFinished_WithValidData_ShouldSetAndOrderStores()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListReducer.OnLoadAllActiveStoresFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        private sealed class OnLoadAllActiveStoresFinishedFixture : ShoppingListReducerFixture
        {
            public LoadAllActiveStoresFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Stores = new DomainTestBuilder<AllActiveStores>().Create()
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = InitialState with
                {
                    Stores = new AllActiveStores(new List<ShoppingListStore>
                    {
                        new DomainTestBuilder<ShoppingListStore>().Create() with
                        {
                            Name = $"A{new DomainTestBuilder<string>().Create()}"
                        },
                        new DomainTestBuilder<ShoppingListStore>().Create() with
                        {
                            Name = $"B{new DomainTestBuilder<string>().Create()}"
                        },
                        new DomainTestBuilder<ShoppingListStore>().Create() with
                        {
                            Name = $"Z{new DomainTestBuilder<string>().Create()}"
                        }
                    })
                };
            }

            public void SetupAction()
            {
                Action = new LoadAllActiveStoresFinishedAction(ExpectedState.Stores.Stores.Reverse().ToList());
            }
        }
    }

    public class OnSelectedStoreChanged
    {
        private readonly OnSelectedStoreChangedFixture _fixture = new();

        [Fact]
        public void OnSelectedStoreChanged_WithValidData_ShouldSetSelectedStoreId()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();
            _fixture.SetupExpectedState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListReducer.OnSelectedStoreChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSelectedStoreChangedFixture : ShoppingListReducerFixture
        {
            public SelectedStoreChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    SelectedStoreId = Guid.NewGuid(),
                    SearchBar = ExpectedState.SearchBar with
                    {
                        Input = new DomainTestBuilder<string>().Create(),
                        Results = new DomainTestBuilder<SearchItemForShoppingListResult>().CreateMany(2).ToList()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        Input = string.Empty,
                        Results = new List<SearchItemForShoppingListResult>()
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SelectedStoreChangedAction(ExpectedState.SelectedStoreId);
            }
        }
    }

    public class OnLoadShoppingListFinished
    {
        private readonly OnLoadShoppingListFinishedFixture _fixture = new();

        [Fact]
        public void OnLoadShoppingListFinished_WithEditModeActive_WithItemsInBasketNotVisible_ShouldResetToDefaultsAndSetShoppingList()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListReducer.OnLoadShoppingListFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadShoppingListFinishedFixture : ShoppingListReducerFixture
        {
            public LoadShoppingListFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    EditModeActive = true,
                    ItemsInBasketVisible = false,
                    ShoppingList = new DomainTestBuilder<ShoppingListModel>().Create()
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    EditModeActive = false,
                    ItemsInBasketVisible = true
                };
            }

            public void SetupAction()
            {
                Action = new LoadShoppingListFinishedAction(ExpectedState.ShoppingList!);
            }
        }
    }

    public class OnToggleItemsInBasketVisible
    {
        private readonly OnToggleItemsInBasketVisibleFixture _fixture = new();

        [Fact]
        public void OnToggleItemsInBasketVisible_WithItemsInBasketVisible_ShouldSetToNotVisible()
        {
            // Arrange
            _fixture.SetupInitialStateWithItemsInBasketVisible();
            _fixture.SetupExpectedStateWithItemsInBasketNotVisible();

            // Act
            var result = ShoppingListReducer.OnToggleItemsInBasketVisible(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnToggleItemsInBasketVisible_WithItemsInBasketNotVisible_ShouldSetToVisible()
        {
            // Arrange
            _fixture.SetupInitialStateWithItemsInBasketNotVisible();
            _fixture.SetupExpectedStateWithItemsInBasketVisible();

            // Act
            var result = ShoppingListReducer.OnToggleItemsInBasketVisible(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnToggleItemsInBasketVisibleFixture : ShoppingListReducerFixture
        {
            public void SetupInitialStateWithItemsInBasketVisible()
            {
                InitialState = ExpectedState with
                {
                    ItemsInBasketVisible = true
                };
            }

            public void SetupInitialStateWithItemsInBasketNotVisible()
            {
                InitialState = ExpectedState with
                {
                    ItemsInBasketVisible = false
                };
            }

            public void SetupExpectedStateWithItemsInBasketVisible()
            {
                ExpectedState = ExpectedState with
                {
                    ItemsInBasketVisible = true
                };
            }

            public void SetupExpectedStateWithItemsInBasketNotVisible()
            {
                ExpectedState = ExpectedState with
                {
                    ItemsInBasketVisible = false
                };
            }
        }
    }

    public class OnToggleEditMode
    {
        private readonly OnToggleEditModeFixture _fixture = new();

        [Fact]
        public void OnToggleEditMode_WithEditModeActive_ShouldSetToInactive()
        {
            // Arrange
            _fixture.SetupInitialStateWithEditModeActive();
            _fixture.SetupExpectedStateWithEditModeInactive();

            // Act
            var result = ShoppingListReducer.OnToggleEditMode(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnToggleEditMode_WithEditModeInactive_ShouldSetToActive()
        {
            // Arrange
            _fixture.SetupInitialStateWithEditModeInactive();
            _fixture.SetupExpectedStateWithEditModeActive();

            // Act
            var result = ShoppingListReducer.OnToggleEditMode(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnToggleEditModeFixture : ShoppingListReducerFixture
        {
            public void SetupInitialStateWithEditModeActive()
            {
                InitialState = ExpectedState with
                {
                    EditModeActive = true
                };
            }

            public void SetupInitialStateWithEditModeInactive()
            {
                InitialState = ExpectedState with
                {
                    EditModeActive = false
                };
            }

            public void SetupExpectedStateWithEditModeActive()
            {
                ExpectedState = ExpectedState with
                {
                    EditModeActive = true
                };
            }

            public void SetupExpectedStateWithEditModeInactive()
            {
                ExpectedState = ExpectedState with
                {
                    EditModeActive = false
                };
            }
        }
    }

    public class OnToggleShoppingListSectionExpansion
    {
        private readonly OnToggleShoppingListSectionExpansionFixture _fixture = new();

        [Fact]
        public void OnToggleShoppingListSectionExpansion_WithSectionExpanded_ShouldCollapseSection()
        {
            // Arrange
            _fixture.SetupInitialStateWithSectionExpanded();
            _fixture.SetupExpectedStateWithSectionCollapsed();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListReducer.OnToggleShoppingListSectionExpansion(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnToggleShoppingListSectionExpansion_WithSectionCollapsed_ShouldExpandSection()
        {
            // Arrange
            _fixture.SetupInitialStateWithSectionCollapsed();
            _fixture.SetupExpectedStateWithSectionExpanded();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListReducer.OnToggleShoppingListSectionExpansion(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnToggleShoppingListSectionExpansion_WithShoppingListNull_ShouldNotModifyState()
        {
            // Arrange
            _fixture.SetupExpectedStateForShoppingListNull();
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionForShoppingListNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListReducer.OnToggleShoppingListSectionExpansion(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnToggleShoppingListSectionExpansion_WithInvalidSectionId_ShouldNotModifyState2()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionForInvalidSection();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListReducer.OnToggleShoppingListSectionExpansion(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnToggleShoppingListSectionExpansionFixture : ShoppingListReducerFixture
        {
            public ToggleShoppingListSectionExpansionAction? Action { get; private set; }

            public void SetupInitialStateEqualExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialStateWithSectionExpanded()
            {
                SetupInitialState(true);
            }

            public void SetupInitialStateWithSectionCollapsed()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isExpanded)
            {
                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                sections[0] = sections[0] with { IsExpanded = isExpanded };

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
                    }
                };
            }

            public void SetupExpectedStateForShoppingListNull()
            {
                ExpectedState = ExpectedState with
                {
                    ShoppingList = null
                };
            }

            public void SetupExpectedStateWithSectionExpanded()
            {
                SetupExpectedState(true);
            }

            public void SetupExpectedStateWithSectionCollapsed()
            {
                SetupExpectedState(false);
            }

            private void SetupExpectedState(bool isExpanded)
            {
                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                sections[0] = sections[0] with { IsExpanded = isExpanded };
                ExpectedState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
                    }
                };
            }

            public void SetupAction()
            {
                Action = new ToggleShoppingListSectionExpansionAction(ExpectedState.ShoppingList!.Sections.First().Id);
            }

            public void SetupActionForShoppingListNull()
            {
                SetupRandomAction();
            }

            public void SetupActionForInvalidSection()
            {
                SetupRandomAction();
            }

            public void SetupRandomAction()
            {
                Action = new ToggleShoppingListSectionExpansionAction(Guid.NewGuid());
            }
        }
    }

    public class OnSaveStoreFinished
    {
        private readonly OnSaveStoreFinishedFixture _fixture = new();

        [Fact]
        public void OnSaveStoreFinished_WithValidData_ShouldClearStores()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ShoppingListReducer.OnSaveStoreFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSaveStoreFinishedFixture : ShoppingListReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Stores = ExpectedState.Stores with
                    {
                        Stores = new DomainTestBuilder<ShoppingListStore>().CreateMany(2).ToList()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Stores = new AllActiveStores(new List<ShoppingListStore>(0))
                };
            }
        }
    }

    public class OnDeleteStoreFinished
    {
        private readonly OnDeleteStoreFinishedFixture _fixture = new();

        [Fact]
        public void OnDeleteStoreFinished_WithValidData_ShouldClearStores()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ShoppingListReducer.OnDeleteStoreFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDeleteStoreFinishedFixture : ShoppingListReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Stores = ExpectedState.Stores with
                    {
                        Stores = new DomainTestBuilder<ShoppingListStore>().CreateMany(2).ToList()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Stores = new AllActiveStores(new List<ShoppingListStore>(0))
                };
            }
        }
    }

    private abstract class ShoppingListReducerFixture
    {
        public ShoppingListState ExpectedState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
        public ShoppingListState InitialState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
    }
}