using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.ItemCategorySelectors;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Items.Reducers;

public class ItemCategorySelectorReducerTests
{
    public class OnLoadInitialItemCategoryFinished
    {
        private readonly OnLoadInitialItemCategoryFinishedFixture _fixture;

        public OnLoadInitialItemCategoryFinished()
        {
            _fixture = new OnLoadInitialItemCategoryFinishedFixture();
        }

        [Fact]
        public void OnLoadInitialItemCategoryFinished_WithValidData_ShouldSetItemCategory()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnLoadInitialItemCategoryFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnLoadInitialItemCategoryFinishedFixture : ItemCategorySelectorReducerFixture
        {
            public LoadInitialItemCategoryFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ItemCategorySelector = ExpectedState.Editor.ItemCategorySelector with
                        {
                            ItemCategories = new DomainTestBuilder<ItemCategorySearchResult>().CreateMany(2).ToList()
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
                        ItemCategorySelector = ExpectedState.Editor.ItemCategorySelector with
                        {
                            ItemCategories = new DomainTestBuilder<ItemCategorySearchResult>().CreateMany(1).ToList()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new LoadInitialItemCategoryFinishedAction(
                    ExpectedState.Editor.ItemCategorySelector.ItemCategories.First());
            }
        }
    }

    public class OnCreateNewItemCategoryFinished
    {
        private readonly OnCreateNewItemCategoryFinishedFixture _fixture;

        public OnCreateNewItemCategoryFinished()
        {
            _fixture = new OnCreateNewItemCategoryFinishedFixture();
        }

        [Fact]
        public void OnCreateNewItemCategoryFinished_WithValidData_ShouldSetItemCategoryAndItemCategoryId()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnCreateNewItemCategoryFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnCreateNewItemCategoryFinishedFixture : ItemCategorySelectorReducerFixture
        {
            public CreateNewItemCategoryFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ItemCategoryId = Guid.NewGuid()
                        },
                        ItemCategorySelector = ExpectedState.Editor.ItemCategorySelector with
                        {
                            ItemCategories = new DomainTestBuilder<ItemCategorySearchResult>().CreateMany(2).ToList()
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                var itemCategories = new DomainTestBuilder<ItemCategorySearchResult>().CreateMany(1).ToList();

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ItemCategoryId = itemCategories.First().Id
                        },
                        ItemCategorySelector = ExpectedState.Editor.ItemCategorySelector with
                        {
                            ItemCategories = itemCategories
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new CreateNewItemCategoryFinishedAction(
                    ExpectedState.Editor.ItemCategorySelector.ItemCategories.First());
            }
        }
    }

    public class OnItemCategoryInputChanged
    {
        private readonly OnItemCategoryInputChangedFixture _fixture;

        public OnItemCategoryInputChanged()
        {
            _fixture = new OnItemCategoryInputChangedFixture();
        }

        [Fact]
        public void OnItemCategoryInputChanged_WithValidInput_ShouldSetInput()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnItemCategoryInputChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnItemCategoryInputChangedFixture : ItemCategorySelectorReducerFixture
        {
            public ItemCategoryInputChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ItemCategorySelector = ExpectedState.Editor.ItemCategorySelector with
                        {
                            Input = new DomainTestBuilder<string>().Create()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new ItemCategoryInputChangedAction(ExpectedState.Editor.ItemCategorySelector.Input);
            }
        }
    }

    public class OnSearchItemCategoryFinished
    {
        private readonly OnSearchItemCategoryFinishedFixture _fixture;

        public OnSearchItemCategoryFinished()
        {
            _fixture = new OnSearchItemCategoryFinishedFixture();
        }

        [Fact]
        public void OnSearchItemCategoryFinished_WithItemCategorySelected_ShouldSetItemCategoriesAndKeepSelected()
        {
            // Arrange
            _fixture.SetupExpectedStateWithSelectedItemCategory();
            _fixture.SetupInitialState();
            _fixture.SetupActionWithSelectedItemCategory();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnSearchItemCategoryFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void OnSearchItemCategoryFinished_WithNoItemCategorySelected_ShouldSetItemCategories()
        {
            // Arrange
            _fixture.SetupExpectedStateWithoutSelectedItemCategory();
            _fixture.SetupInitialState();
            _fixture.SetupActionWithoutSelectedItemCategory();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnSearchItemCategoryFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnSearchItemCategoryFinishedFixture : ItemCategorySelectorReducerFixture
        {
            public SearchItemCategoryFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ItemCategorySelector = ExpectedState.Editor.ItemCategorySelector with
                        {
                            ItemCategories = new List<ItemCategorySearchResult>
                            {
                                new DomainTestBuilder<ItemCategorySearchResult>().Create(),
                                ExpectedState.Editor.ItemCategorySelector.ItemCategories.First()
                            }
                        }
                    }
                };
            }

            public void SetupExpectedStateWithSelectedItemCategory()
            {
                var itemCategory = ExpectedState.Editor.ItemCategorySelector.ItemCategories.Last();

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ItemCategoryId = itemCategory.Id
                        },
                        ItemCategorySelector = ExpectedState.Editor.ItemCategorySelector with
                        {
                            ItemCategories = new List<ItemCategorySearchResult>
                            {
                                itemCategory,
                                new DomainTestBuilder<ItemCategorySearchResult>()
                                    .FillPropertyWith(r => r.Name, $"A{new DomainTestBuilder<string>().Create()}")
                                    .Create(),
                                new DomainTestBuilder<ItemCategorySearchResult>()
                                    .FillPropertyWith(r => r.Name, $"B{new DomainTestBuilder<string>().Create()}")
                                    .Create(),
                                new DomainTestBuilder<ItemCategorySearchResult>()
                                    .FillPropertyWith(r => r.Name, $"Z{new DomainTestBuilder<string>().Create()}")
                                    .Create()
                            }
                        }
                    }
                };
            }

            public void SetupExpectedStateWithoutSelectedItemCategory()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ItemCategoryId = null
                        },
                        ItemCategorySelector = ExpectedState.Editor.ItemCategorySelector with
                        {
                            ItemCategories = new List<ItemCategorySearchResult>
                            {
                                new DomainTestBuilder<ItemCategorySearchResult>()
                                    .FillPropertyWith(r => r.Name, $"A{new DomainTestBuilder<string>().Create()}")
                                    .Create(),
                                new DomainTestBuilder<ItemCategorySearchResult>()
                                    .FillPropertyWith(r => r.Name, $"B{new DomainTestBuilder<string>().Create()}")
                                    .Create(),
                                new DomainTestBuilder<ItemCategorySearchResult>()
                                    .FillPropertyWith(r => r.Name, $"Z{new DomainTestBuilder<string>().Create()}")
                                    .Create()
                            }
                        }
                    }
                };
            }

            public void SetupActionWithSelectedItemCategory()
            {
                Action = new SearchItemCategoryFinishedAction(
                    ExpectedState.Editor.ItemCategorySelector.ItemCategories.Skip(1).Reverse().ToList());
            }

            public void SetupActionWithoutSelectedItemCategory()
            {
                Action = new SearchItemCategoryFinishedAction(
                    ExpectedState.Editor.ItemCategorySelector.ItemCategories.Reverse().ToList());
            }
        }
    }

    public class OnSelectedItemCategoryChanged
    {
        private readonly OnSelectedItemCategoryChangedFixture _fixture;

        public OnSelectedItemCategoryChanged()
        {
            _fixture = new OnSelectedItemCategoryChangedFixture();
        }

        [Fact]
        public void OnSelectedItemCategoryChanged_WithValidData_ShouldSetItemCategoryId()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategorySelectorReducer.OnSelectedItemCategoryChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnSelectedItemCategoryChangedFixture : ItemCategorySelectorReducerFixture
        {
            public SelectedItemCategoryChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ItemCategoryId = Guid.NewGuid()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SelectedItemCategoryChangedAction(ExpectedState.Editor.Item!.ItemCategoryId!.Value);
            }
        }
    }

    private abstract class ItemCategorySelectorReducerFixture
    {
        public ItemState ExpectedState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
        public ItemState InitialState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
    }
}