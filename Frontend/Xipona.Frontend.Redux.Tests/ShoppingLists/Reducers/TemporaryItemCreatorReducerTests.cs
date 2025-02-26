﻿using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.TemporaryItemCreator;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common.Extensions;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ShoppingLists.Reducers;

public class TemporaryItemCreatorReducerTests
{
    public class OnTemporaryItemNameChanged
    {
        private readonly OnTemporaryItemNameChangedFixture _fixture = new();

        [Fact]
        public void OnTemporaryItemNameChanged_WithValidName_ShouldUpdateName()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = TemporaryItemCreatorReducer.OnTemporaryItemNameChanged(_fixture.InitialState,
                _fixture.ChangeAction);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnTemporaryItemNameChangedFixture : TemporaryItemCreatorReducerFixture
        {
            public TemporaryItemNameChangedAction? ChangeAction { get; private set; }

            public void SetupAction()
            {
                ChangeAction = new TemporaryItemNameChangedAction(ExpectedState.TemporaryItemCreator.ItemName);
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        ItemName = new DomainTestBuilder<string>().Create()
                    }
                };
            }
        }
    }

    public class OnTemporaryItemPriceChanged
    {
        private readonly OnTemporaryItemPriceChangedFixture _fixture = new();

        [Fact]
        public void OnTemporaryItemPriceChanged_WithValidPrice_ShouldUpdatePrice()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = TemporaryItemCreatorReducer.OnTemporaryItemPriceChanged(_fixture.InitialState,
                _fixture.ChangeAction);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnTemporaryItemPriceChangedFixture : TemporaryItemCreatorReducerFixture
        {
            public TemporaryItemPriceChangedAction? ChangeAction { get; private set; }

            public void SetupAction()
            {
                ChangeAction = new TemporaryItemPriceChangedAction(ExpectedState.TemporaryItemCreator.Price);
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        Price = new DomainTestBuilder<decimal>().Create()
                    }
                };
            }
        }
    }

    public class OnTemporaryItemSelectedSectionChanged
    {
        private readonly OnTemporaryItemSelectedSectionChangedFixture _fixture = new();

        [Fact]
        public void OnTemporaryItemSelectedSectionChanged_WithValidSection_ShouldUpdateSection()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = TemporaryItemCreatorReducer.OnTemporaryItemSelectedSectionChanged(_fixture.InitialState,
                _fixture.ChangeAction);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnTemporaryItemSelectedSectionChangedFixture : TemporaryItemCreatorReducerFixture
        {
            public TemporaryItemSelectedSectionChangedAction? ChangeAction { get; private set; }

            public void SetupAction()
            {
                ChangeAction = new TemporaryItemSelectedSectionChangedAction(ExpectedState.TemporaryItemCreator.Section!);
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        Section = new DomainTestBuilder<ShoppingListStoreSection>().Create()
                    }
                };
            }
        }
    }

    public class OnTemporaryItemSelectedQuantityTypeChanged
    {
        private readonly OnTemporaryItemSelectedQuantityTypeChangedFixture _fixture = new();

        [Fact]
        public void OnTemporaryItemSelectedQuantityTypeChanged_WithValidQuantityTypeId_ShouldUpdateSelectedQuantityTypeId()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = TemporaryItemCreatorReducer.OnTemporaryItemSelectedQuantityTypeChanged(_fixture.InitialState,
                _fixture.ChangeAction);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnTemporaryItemSelectedQuantityTypeChangedFixture : TemporaryItemCreatorReducerFixture
        {
            public TemporaryItemSelectedQuantityTypeChangedAction? ChangeAction { get; private set; }

            public void SetupAction()
            {
                ChangeAction = new TemporaryItemSelectedQuantityTypeChangedAction(
                    ExpectedState.TemporaryItemCreator.SelectedQuantityTypeId);
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        SelectedQuantityTypeId = new DomainTestBuilder<int>().Create()
                    }
                };
            }
        }
    }

    public class OnOpenTemporaryItemCreator
    {
        private readonly OnOpenTemporaryItemCreatorFixture _fixture = new();

        [Fact]
        public void OnOpenTemporaryItemCreator_WithValidSectionsAndQuantityTypes_ShouldInitializeItemCreator()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = TemporaryItemCreatorReducer.OnOpenTemporaryItemCreator(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnOpenTemporaryItemCreator_WithNoQuantityTypes_ShouldSetSelectedQuantityTypeTo0()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoQuantityTypes();
            _fixture.SetupInitialState();

            // Act
            var result = TemporaryItemCreatorReducer.OnOpenTemporaryItemCreator(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnOpenTemporaryItemCreator_WithNoDefaultSection_ShouldSetFirstSection()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoDefaultSection();
            _fixture.SetupInitialState();

            // Act
            var result = TemporaryItemCreatorReducer.OnOpenTemporaryItemCreator(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OpenTemporaryItemCreator_WithSelectedStoreNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupExpectedStateForSelectedStoreNull();
            _fixture.SetupInitialStateEqualToExpectedState();

            // Act
            var result = TemporaryItemCreatorReducer.OnOpenTemporaryItemCreator(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnOpenTemporaryItemCreatorFixture : TemporaryItemCreatorReducerFixture
        {
            public void SetupInitialStateEqualToExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        ItemName = new DomainTestBuilder<string>().Create(),
                        IsOpen = false,
                        IsSaving = true,
                        Price = new DomainTestBuilder<decimal>().Create(),
                        Section = new DomainTestBuilder<ShoppingListStoreSection>().Create(),
                        SelectedQuantityTypeId = new DomainTestBuilder<int>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                var sections = new List<ShoppingListStoreSection>
                {
                    new DomainTestBuilder<ShoppingListStoreSection>().Create() with { IsDefaultSection = false },
                    new DomainTestBuilder<ShoppingListStoreSection>().Create() with { IsDefaultSection = true },
                };
                var store = new DomainTestBuilder<ShoppingListStore>().Create() with { Sections = sections };
                var quantityTypes = new DomainTestBuilder<QuantityType>().CreateMany(2).ToList();

                ExpectedState = ExpectedState with
                {
                    QuantityTypes = quantityTypes,
                    Stores = ExpectedState.Stores with
                    {
                        Stores = new List<ShoppingListStore>
                        {
                            store
                        }
                    },
                    SelectedStoreId = store.Id,
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        ItemName = ExpectedState.SearchBar.Input,
                        IsOpen = true,
                        IsSaving = false,
                        Price = 1m,
                        Section = sections.Last(),
                        SelectedQuantityTypeId = quantityTypes.First().Id
                    }
                };
            }

            public void SetupExpectedStateWithNoQuantityTypes()
            {
                var sections = new List<ShoppingListStoreSection>
                {
                    new DomainTestBuilder<ShoppingListStoreSection>().Create() with { IsDefaultSection = false },
                    new DomainTestBuilder<ShoppingListStoreSection>().Create() with { IsDefaultSection = true },
                };
                var store = new DomainTestBuilder<ShoppingListStore>().Create() with { Sections = sections };

                ExpectedState = ExpectedState with
                {
                    QuantityTypes = new List<QuantityType>(),
                    Stores = ExpectedState.Stores with
                    {
                        Stores = new List<ShoppingListStore>
                        {
                            store
                        }
                    },
                    SelectedStoreId = store.Id,
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        ItemName = ExpectedState.SearchBar.Input,
                        IsOpen = true,
                        IsSaving = false,
                        Price = 1m,
                        Section = sections.Last(),
                        SelectedQuantityTypeId = 0
                    }
                };
            }

            public void SetupExpectedStateWithNoDefaultSection()
            {
                var sections = new List<ShoppingListStoreSection>
                {
                    new DomainTestBuilder<ShoppingListStoreSection>().Create() with { IsDefaultSection = false },
                    new DomainTestBuilder<ShoppingListStoreSection>().Create() with { IsDefaultSection = false },
                };
                var store = new DomainTestBuilder<ShoppingListStore>().Create() with { Sections = sections };
                var quantityTypes = new DomainTestBuilder<QuantityType>().CreateMany(2).ToList();

                ExpectedState = ExpectedState with
                {
                    QuantityTypes = quantityTypes,
                    Stores = ExpectedState.Stores with
                    {
                        Stores = new List<ShoppingListStore>
                        {
                            store
                        }
                    },
                    SelectedStoreId = store.Id,
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        ItemName = ExpectedState.SearchBar.Input,
                        IsOpen = true,
                        IsSaving = false,
                        Price = 1m,
                        Section = sections.First(),
                        SelectedQuantityTypeId = quantityTypes.First().Id
                    }
                };
            }

            public void SetupExpectedStateForSelectedStoreNull()
            {
                ExpectedState = ExpectedState with
                {
                    SelectedStoreId = Guid.NewGuid()
                };
            }
        }
    }

    public class OnCloseTemporaryItemCreator
    {
        private readonly OnCloseTemporaryItemCreatorFixture _fixture = new();

        [Fact]
        public void OnCloseTemporaryItemCreator_WithValidData_ShouldCloseTemporaryItemCreator()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = TemporaryItemCreatorReducer.OnCloseTemporaryItemCreator(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnCloseTemporaryItemCreatorFixture : TemporaryItemCreatorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        ItemName = new DomainTestBuilder<string>().Create(),
                        IsOpen = true,
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        Input = string.Empty
                    },
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        ItemName = string.Empty,
                        IsOpen = false,
                    }
                };
            }
        }
    }

    public class OnSaveTemporaryItemStarted
    {
        private readonly OnSaveTemporaryItemStartedFixture _fixture = new();

        [Fact]
        public void OnSaveTemporaryItemStarted_WithNotAlreadySaving_ShouldSetTemporaryItemCreatorToSaving()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = TemporaryItemCreatorReducer.OnSaveTemporaryItemStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSaveTemporaryItemStarted_WithAlreadySaving_ShouldSetTemporaryItemCreatorToSaving()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialStateAlreadySaving();

            // Act
            var result = TemporaryItemCreatorReducer.OnSaveTemporaryItemStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSaveTemporaryItemStartedFixture : TemporaryItemCreatorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        IsSaving = false
                    }
                };
            }

            public void SetupInitialStateAlreadySaving()
            {
                InitialState = ExpectedState with
                {
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        IsSaving = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        IsSaving = true
                    }
                };
            }
        }
    }

    public class OnAddTemporaryItem
    {
        private readonly OnAddTemporaryItemFixture _fixture = new();

        [Fact]
        public void OnAddTemporaryItem_WithSectionAlreadyExisting_ShouldAddItemToExistingSection()
        {
            // Arrange
            _fixture.SetupExpectedStateForAlreadyExistingSection();
            _fixture.SetupAction();
            _fixture.SetupInitialStateForAlreadyExistingSection();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);

            // Act
            var result = TemporaryItemCreatorReducer.OnAddTemporaryItem(_fixture.InitialState,
                _fixture.ChangeAction);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnAddTemporaryItem_WithSectionNotAlreadyExisting_ShouldAddItemToNewSection()
        {
            // Arrange
            _fixture.SetupExpectedStateForNotAlreadyExistingSection();
            _fixture.SetupAction();
            _fixture.SetupInitialStateForNotAlreadyExistingSection();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);

            // Act
            var result = TemporaryItemCreatorReducer.OnAddTemporaryItem(_fixture.InitialState,
                _fixture.ChangeAction);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnAddTemporaryItemFixture : TemporaryItemCreatorReducerFixture
        {
            public AddTemporaryItemAction? ChangeAction { get; private set; }

            public void SetupAction()
            {
                var section = ExpectedState.ShoppingList!.Sections.Last();
                var storeSection = new DomainTestBuilder<ShoppingListStoreSection>().Create() with
                {
                    Id = section.Id,
                    Name = section.Name,
                    SortingIndex = section.SortingIndex,
                };

                ChangeAction = new AddTemporaryItemAction(
                    ExpectedState.ShoppingList.Sections.Last().Items.Last(),
                    storeSection);
            }

            public void SetupInitialStateForAlreadyExistingSection()
            {
                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                var section = sections.Last();
                var items = section.Items.ToList();
                items.RemoveAt(items.Count - 1);
                section = section with
                {
                    Items = items
                };
                sections[^1] = section;

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
                    }
                };
            }

            public void SetupInitialStateForNotAlreadyExistingSection()
            {
                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                sections.RemoveAt(sections.Count - 1);

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
                    }
                };
            }

            public void SetupExpectedStateForAlreadyExistingSection()
            {
                ExpectedState = ExpectedState with
                {
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        IsSaving = false
                    }
                };
            }

            public void SetupExpectedStateForNotAlreadyExistingSection()
            {
                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                var section = sections.Last();
                sections[^1] = section with
                {
                    Items = new DomainTestBuilder<ShoppingListItem>().CreateMany(1).ToList(),
                    IsExpanded = true
                };

                ExpectedState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
                    },
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        IsSaving = false
                    }
                };
            }
        }
    }

    public class OnTemporaryItemCreated
    {
        private readonly OnTemporaryItemCreatedFixture _fixture = new();

        [Fact]
        public void OnTemporaryItemCreated_WithValidData_ShouldUpdateItem()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = TemporaryItemCreatorReducer.OnTemporaryItemCreated(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnTemporaryItemCreated_WithInvalidTempId_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupInitialStateEqualToExpectedState();
            _fixture.SetupActionWithInvalidTempId();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = TemporaryItemCreatorReducer.OnTemporaryItemCreated(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.InitialState);
        }

        private sealed class OnTemporaryItemCreatedFixture : TemporaryItemCreatorReducerFixture
        {
            private Guid _tempId;
            private Guid _actualId;
            public TemporaryItemCreatedAction? Action { get; private set; }

            public void SetupInitialStateEqualToExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState()
            {
                var (section, sectionIndex) = ExpectedState.ShoppingList!.Sections.Random();
                var (item, itemIndex) = section.Items.Random();
                _tempId = item.Id.OfflineId!.Value;
                _actualId = item.Id.ActualId!.Value;

                var sections = ExpectedState.ShoppingList.Sections.ToList();
                var items = section.Items.ToList();
                items[itemIndex] = item with
                {
                    Id = new(item.Id.OfflineId, null)
                };
                section = section with
                {
                    Items = items
                };
                sections[sectionIndex] = section;

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
                    }
                };
            }

            public void SetupAction()
            {
                Action = new TemporaryItemCreatedAction(_tempId, _actualId);
            }

            public void SetupActionWithInvalidTempId()
            {
                Action = new TemporaryItemCreatedAction(Guid.NewGuid(), Guid.NewGuid());
            }
        }
    }

    private abstract class TemporaryItemCreatorReducerFixture
    {
        public ShoppingListState InitialState { get; protected set; } =
            new DomainTestBuilder<ShoppingListState>().Create();

        public ShoppingListState ExpectedState { get; protected set; } =
            new DomainTestBuilder<ShoppingListState>().Create();
    }
}