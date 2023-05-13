using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor.AddToShoppingListModal;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Recipes.Reducers;

public class AddToShoppingListReducerTests
{
    public class OnAddToShoppingListModalClosed
    {
        private readonly OnAddToShoppingListModalClosedFixture _fixture = new();

        [Fact]
        public void OnAddToShoppingListModalClosed_WithOpenModal_ShouldCloseAndRemoveModal()
        {
            // Arrange
            _fixture.SetupInitialState(true);
            _fixture.SetupExpectedState();

            // Act
            var result = AddToShoppingListReducer.OnAddToShoppingListModalClosed(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnAddToShoppingListModalClosed_WithClosedModal_ShouldRemoveModal()
        {
            // Arrange
            _fixture.SetupInitialState(false);
            _fixture.SetupExpectedState();

            // Act
            var result = AddToShoppingListReducer.OnAddToShoppingListModalClosed(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnAddToShoppingListModalClosedFixture : AddToShoppingListReducerFixture
        {
            public void SetupInitialState(bool isOpen)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsAddToShoppingListOpen = isOpen,
                        AddToShoppingList = new DomainTestBuilder<AddToShoppingList>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsAddToShoppingListOpen = false,
                        AddToShoppingList = null
                    }
                };
            }
        }
    }

    public class OnLoadAddToShoppingListFinished
    {
        private readonly OnLoadAddToShoppingListFinishedFixture _fixture = new();

        [Fact]
        public void OnLoadAddToShoppingListFinished_WithValidData_ShouldReturnExpectedState()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = AddToShoppingListReducer.OnLoadAddToShoppingListFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnLoadAddToShoppingListFinished_WithRecipeNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupInitialStateForRecipeNull();
            _fixture.SetupExpectedStateEqualsInitialState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = AddToShoppingListReducer.OnLoadAddToShoppingListFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadAddToShoppingListFinishedFixture : AddToShoppingListReducerFixture
        {
            public LoadAddToShoppingListFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsAddToShoppingListOpen = false,
                        AddToShoppingList = new DomainTestBuilder<AddToShoppingList>().Create()
                    }
                };
            }

            public void SetupInitialStateForRecipeNull()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupExpectedStateEqualsInitialState()
            {
                ExpectedState = InitialState;
            }

            public void SetupExpectedState()
            {
                var numberOfServings = ExpectedState.Editor.Recipe!.NumberOfServings;
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsAddToShoppingListOpen = true,
                        AddToShoppingList = ExpectedState.Editor.AddToShoppingList! with
                        {
                            NumberOfServings = numberOfServings,
                            ItemAmountsForOneServing = ExpectedState.Editor.AddToShoppingList.Items
                                .ToDictionary(i => (i.ItemId, i.ItemTypeId), i => i.Quantity / numberOfServings)
                        }
                    }
                };
            }

            public void SetupAction()
            {
                var items = ExpectedState.Editor.AddToShoppingList!.Items
                    .Select(i => i with
                    {
                        Quantity = i.Quantity / ExpectedState.Editor.Recipe!.NumberOfServings
                    })
                    .ToList();

                Action = new LoadAddToShoppingListFinishedAction(items);
            }

            public void SetupActionForRecipeNull()
            {
                Action = new DomainTestBuilder<LoadAddToShoppingListFinishedAction>().Create();
            }
        }
    }

    public class OnAddToShoppingListNumberOfServingsChanged
    {
        private readonly OnAddToShoppingListNumberOfServingsChangedFixture _fixture = new();

        [Fact]
        public void OnAddToShoppingListNumberOfServingsChanged_WithValidData_ShouldReturnExpectedState()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = AddToShoppingListReducer.OnAddToShoppingListNumberOfServingsChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnAddToShoppingListNumberOfServingsChanged_WithAddToShoppingListNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupInitialStateForAddToShoppingListNull();
            _fixture.SetupExpectedStateEqualsInitialState();
            _fixture.SetupActionForAddToShoppingListNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = AddToShoppingListReducer.OnAddToShoppingListNumberOfServingsChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnAddToShoppingListNumberOfServingsChangedFixture : AddToShoppingListReducerFixture
        {
            public AddToShoppingListNumberOfServingsChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                var numberOfServings = ExpectedState.Editor.Recipe!.NumberOfServings;
                var amountForOneServing = ExpectedState.Editor.AddToShoppingList!.Items
                    .ToDictionary(i => (i.ItemId, i.ItemTypeId), i => i.Quantity / numberOfServings);

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        AddToShoppingList = ExpectedState.Editor.AddToShoppingList with
                        {
                            NumberOfServings = numberOfServings,
                            ItemAmountsForOneServing = amountForOneServing,
                            Items = ExpectedState.Editor.AddToShoppingList.Items
                                .Select(i => i with
                                {
                                    Quantity = amountForOneServing[(i.ItemId, i.ItemTypeId)] * numberOfServings
                                })
                                .ToList()
                        }
                    }
                };
            }

            public void SetupInitialStateForAddToShoppingListNull()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        AddToShoppingList = null
                    }
                };
            }

            public void SetupExpectedStateEqualsInitialState()
            {
                ExpectedState = InitialState;
            }

            public void SetupExpectedState()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var numberOfServings = ExpectedState.Editor.Recipe!.NumberOfServings;
                var amountForOneServing = ExpectedState.Editor.AddToShoppingList!.Items
                    .ToDictionary(i => (i.ItemId, i.ItemTypeId), i => i.Quantity / numberOfServings);

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        AddToShoppingList = ExpectedState.Editor.AddToShoppingList with
                        {
                            NumberOfServings = Action.NumberOfServings,
                            ItemAmountsForOneServing = amountForOneServing,
                            Items = ExpectedState.Editor.AddToShoppingList.Items
                                .Select(i => i with
                                {
                                    Quantity = (float)Math.Ceiling(
                                        amountForOneServing[(i.ItemId, i.ItemTypeId)] * Action.NumberOfServings)
                                })
                                .ToList()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new DomainTestBuilder<AddToShoppingListNumberOfServingsChangedAction>().Create();
            }

            public void SetupActionForAddToShoppingListNull()
            {
                Action = new DomainTestBuilder<AddToShoppingListNumberOfServingsChangedAction>().Create();
            }
        }
    }

    public class OnAddItemToShoppingListChanged
    {
        private readonly OnAddItemToShoppingListChangedFixture _fixture = new();

        [Fact]
        public void OnAddItemToShoppingListChanged_WithValidData_ShouldReturnExpectedState()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = AddToShoppingListReducer.OnAddItemToShoppingListChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnAddItemToShoppingListChanged_WithAddToShoppingListNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupInitialStateForAddToShoppingListNull();
            _fixture.SetupExpectedStateEqualsInitialState();
            _fixture.SetupActionForAddToShoppingListNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = AddToShoppingListReducer.OnAddItemToShoppingListChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnAddItemToShoppingListChanged_WithNotExistentItemKey_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedStateEqualsInitialState();
            _fixture.SetupActionForWrongItem();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = AddToShoppingListReducer.OnAddItemToShoppingListChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnAddItemToShoppingListChangedFixture : AddToShoppingListReducerFixture
        {
            public AddItemToShoppingListChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                var items = ExpectedState.Editor.AddToShoppingList!.Items.ToList();
                items[^1] = items[^1] with
                {
                    AddToShoppingList = !items[^1].AddToShoppingList
                };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        AddToShoppingList = ExpectedState.Editor.AddToShoppingList with
                        {
                            Items = items
                        }
                    }
                };
            }

            public void SetupInitialStateForAddToShoppingListNull()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        AddToShoppingList = null
                    }
                };
            }

            public void SetupExpectedStateEqualsInitialState()
            {
                ExpectedState = InitialState;
            }

            public void SetupAction()
            {
                var item = ExpectedState.Editor.AddToShoppingList!.Items.Last();
                Action = new AddItemToShoppingListChangedAction(item.Key, item.AddToShoppingList);
            }

            public void SetupActionForAddToShoppingListNull()
            {
                Action = new DomainTestBuilder<AddItemToShoppingListChangedAction>().Create();
            }

            public void SetupActionForWrongItem()
            {
                Action = new DomainTestBuilder<AddItemToShoppingListChangedAction>().Create();
            }
        }
    }

    public class OnAddToShoppingListItemStoreChanged
    {
        private readonly OnAddToShoppingListItemStoreChangedFixture _fixture = new();

        [Fact]
        public void OnAddToShoppingListItemStoreChanged_WithValidData_ShouldReturnExpectedState()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = AddToShoppingListReducer.OnAddToShoppingListItemStoreChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnAddToShoppingListItemStoreChanged_WithAddToShoppingListNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupInitialStateForAddToShoppingListNull();
            _fixture.SetupExpectedStateEqualsInitialState();
            _fixture.SetupActionForAddToShoppingListNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = AddToShoppingListReducer.OnAddToShoppingListItemStoreChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnAddToShoppingListItemStoreChanged_WithNotExistentItemKey_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedStateEqualsInitialState();
            _fixture.SetupActionForWrongItem();
            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = AddToShoppingListReducer.OnAddToShoppingListItemStoreChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnAddToShoppingListItemStoreChangedFixture : AddToShoppingListReducerFixture
        {
            public AddToShoppingListItemStoreChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                var items = ExpectedState.Editor.AddToShoppingList!.Items.ToList();
                items[0] = items[0] with
                {
                    SelectedStoreId = Guid.NewGuid()
                };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        AddToShoppingList = ExpectedState.Editor.AddToShoppingList with
                        {
                            Items = items
                        }
                    }
                };
            }

            public void SetupInitialStateForAddToShoppingListNull()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        AddToShoppingList = null
                    }
                };
            }

            public void SetupExpectedStateEqualsInitialState()
            {
                ExpectedState = InitialState;
            }

            public void SetupAction()
            {
                var item = ExpectedState.Editor.AddToShoppingList!.Items.First();
                Action = new AddToShoppingListItemStoreChangedAction(item.Key, item.SelectedStoreId);
            }

            public void SetupActionForAddToShoppingListNull()
            {
                Action = new DomainTestBuilder<AddToShoppingListItemStoreChangedAction>().Create();
            }

            public void SetupActionForWrongItem()
            {
                Action = new DomainTestBuilder<AddToShoppingListItemStoreChangedAction>().Create();
            }
        }
    }

    public class OnAddItemsToShoppingListStarted
    {
        private readonly OnAddItemsToShoppingListStartedFixture _fixture = new();

        [Fact]
        public void OnAddItemsToShoppingListStarted_WithSaving_ShouldSetSaving()
        {
            // Arrange
            _fixture.SetupInitialState(true);
            _fixture.SetupExpectedState();

            // Act
            var result = AddToShoppingListReducer.OnAddItemsToShoppingListStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnAddItemsToShoppingListStarted_WithNotSaving_ShouldSetSaving()
        {
            // Arrange
            _fixture.SetupInitialState(false);
            _fixture.SetupExpectedState();

            // Act
            var result = AddToShoppingListReducer.OnAddItemsToShoppingListStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnAddItemsToShoppingListStarted_WithAddToShoppingListNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupExpectedStateWithAddToShoppingListNull();
            _fixture.SetupInitialStateEqualsExpectedState();

            // Act
            var result = AddToShoppingListReducer.OnAddItemsToShoppingListStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnAddItemsToShoppingListStartedFixture : AddToShoppingListReducerFixture
        {
            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState(bool isSaving)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        AddToShoppingList = ExpectedState.Editor.AddToShoppingList! with
                        {
                            IsSaving = isSaving
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        AddToShoppingList = ExpectedState.Editor.AddToShoppingList! with
                        {
                            IsSaving = true
                        }
                    }
                };
            }

            public void SetupExpectedStateWithAddToShoppingListNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        AddToShoppingList = null
                    }
                };
            }
        }
    }

    public class OnAddItemsToShoppingListFinished
    {
        private readonly OnAddItemsToShoppingListFinishedFixture _fixture = new();

        [Fact]
        public void OnAddItemsToShoppingListFinished_WithSaving_ShouldSetNotSaving()
        {
            // Arrange
            _fixture.SetupInitialState(true);
            _fixture.SetupExpectedState();

            // Act
            var result = AddToShoppingListReducer.OnAddItemsToShoppingListFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnAddItemsToShoppingListFinished_WithNotSaving_ShouldSetNotSaving()
        {
            // Arrange
            _fixture.SetupInitialState(false);
            _fixture.SetupExpectedState();

            // Act
            var result = AddToShoppingListReducer.OnAddItemsToShoppingListFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnAddItemsToShoppingListFinished_WithAddToShoppingListNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupExpectedStateWithAddToShoppingListNull();
            _fixture.SetupInitialStateEqualsExpectedState();

            // Act
            var result = AddToShoppingListReducer.OnAddItemsToShoppingListFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnAddItemsToShoppingListFinishedFixture : AddToShoppingListReducerFixture
        {
            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState(bool isSaving)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        AddToShoppingList = ExpectedState.Editor.AddToShoppingList! with
                        {
                            IsSaving = isSaving
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        AddToShoppingList = ExpectedState.Editor.AddToShoppingList! with
                        {
                            IsSaving = false
                        }
                    }
                };
            }

            public void SetupExpectedStateWithAddToShoppingListNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        AddToShoppingList = null
                    }
                };
            }
        }
    }

    private abstract class AddToShoppingListReducerFixture
    {
        public RecipeState ExpectedState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
        public RecipeState InitialState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
    }
}