using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.SearchBar;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ShoppingLists.Reducers;

public class ShoppingListSearchBarReducerTests
{
    public class OnItemForShoppingListSearchInputChanged
    {
        private readonly OnItemForShoppingListSearchInputChangedFixture _fixture;

        public OnItemForShoppingListSearchInputChanged()
        {
            _fixture = new OnItemForShoppingListSearchInputChangedFixture();
        }

        [Fact]
        public void OnItemForShoppingListSearchInputChanged_WithValidInput_WithButtonDisabled_ShouldSetInputAndEnableButton()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupInitialButtonDisabled();
            _fixture.SetupExpectedButtonEnabled();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListSearchBarReducer.OnItemForShoppingListSearchInputChanged(_fixture.InitialState,
                _fixture.Action!);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemForShoppingListSearchInputChanged_WithValidInput_WithButtonEnabled_ShouldSetInputAndEnableButton()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupInitialButtonEnabled();
            _fixture.SetupExpectedButtonEnabled();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListSearchBarReducer.OnItemForShoppingListSearchInputChanged(_fixture.InitialState,
                _fixture.Action!);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void OnItemForShoppingListSearchInputChanged_WithInvalidInput_WithButtonDisabled_ShouldSetInputAndDisableButton(
            string input)
        {
            // Arrange
            _fixture.SetupInput(input);
            _fixture.SetupInitialButtonDisabled();
            _fixture.SetupExpectedButtonDisabled();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListSearchBarReducer.OnItemForShoppingListSearchInputChanged(_fixture.InitialState,
                _fixture.Action!);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void OnItemForShoppingListSearchInputChanged_WithInvalidInput_WithButtonEnabled_ShouldSetInputAndDisableButton(
                string input)
        {
            // Arrange
            _fixture.SetupInput(input);
            _fixture.SetupInitialButtonEnabled();
            _fixture.SetupExpectedButtonDisabled();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListSearchBarReducer.OnItemForShoppingListSearchInputChanged(_fixture.InitialState,
                _fixture.Action!);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnItemForShoppingListSearchInputChangedFixture : ShoppingListSearchBarReducerFixture
        {
            private string? _input;

            public ItemForShoppingListSearchInputChangedAction? Action { get; private set; }

            public void SetupInput(string? input = null)
            {
                _input = input ?? new DomainTestBuilder<string>().Create();
            }

            public void SetupInitialButtonEnabled()
            {
                SetupInitialState(true);
            }

            public void SetupInitialButtonDisabled()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isButtonEnabled)
            {
                InitialState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        Input = new DomainTestBuilder<string>().Create()
                    },
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        IsButtonEnabled = isButtonEnabled
                    }
                };
            }

            public void SetupExpectedButtonEnabled()
            {
                SetupExpectedState(true);
            }

            public void SetupExpectedButtonDisabled()
            {
                SetupExpectedState(false);
            }

            private void SetupExpectedState(bool isButtonEnabled)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);

                ExpectedState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        Input = _input.Trim(),
                        Results = isButtonEnabled
                            ? ExpectedState.SearchBar.Results
                            : new List<SearchItemForShoppingListResult>()
                    },
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        IsButtonEnabled = isButtonEnabled
                    }
                };
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);

                Action = new ItemForShoppingListSearchInputChangedAction(_input);
            }
        }
    }

    public class OnSearchItemForShoppingList
    {
        private readonly OnSearchItemForShoppingListFixture _fixture;

        public OnSearchItemForShoppingList()
        {
            _fixture = new OnSearchItemForShoppingListFixture();
        }

        [Fact]
        public void OnSearchItemForShoppingList_WithValidData_ShouldClearSearchResults()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ShoppingListSearchBarReducer.OnSearchItemForShoppingList(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSearchItemForShoppingListFixture : ShoppingListSearchBarReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
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
                        Results = new List<SearchItemForShoppingListResult>()
                    },
                };
            }
        }
    }

    public class OnSearchItemForShoppingListFinished
    {
        private readonly OnSearchItemForShoppingListFinishedFixture _fixture;

        public OnSearchItemForShoppingListFinished()
        {
            _fixture = new OnSearchItemForShoppingListFinishedFixture();
        }

        [Fact]
        public void OnSearchItemForShoppingListFinished_WithValidData_ShouldSetAndOrderSearchResults()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListSearchBarReducer.OnSearchItemForShoppingListFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        private sealed class OnSearchItemForShoppingListFinishedFixture : ShoppingListSearchBarReducerFixture
        {
            public SearchItemForShoppingListFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
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
                        Results = new List<SearchItemForShoppingListResult>
                        {
                            new DomainTestBuilder<SearchItemForShoppingListResult>()
                                .FillConstructorWith("name", $"A{new DomainTestBuilder<string>().Create()}")
                                .Create(),
                            new DomainTestBuilder<SearchItemForShoppingListResult>()
                                .FillConstructorWith("name", $"B{new DomainTestBuilder<string>().Create()}")
                                .Create(),
                            new DomainTestBuilder<SearchItemForShoppingListResult>()
                                .FillConstructorWith("name", $"Z{new DomainTestBuilder<string>().Create()}")
                                .Create()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SearchItemForShoppingListFinishedAction(ExpectedState.SearchBar.Results.Reverse().ToList());
            }
        }
    }

    public class OnItemForShoppingListSearchResultSelected
    {
        private readonly OnItemForShoppingListSearchResultSelectedFixture _fixture;

        public OnItemForShoppingListSearchResultSelected()
        {
            _fixture = new OnItemForShoppingListSearchResultSelectedFixture();
        }

        [Fact]
        public void OnItemForShoppingListSearchResultSelected_WithValidData_ShouldClearInputAndSearchResults()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ShoppingListSearchBarReducer.OnItemForShoppingListSearchResultSelected(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnItemForShoppingListSearchResultSelectedFixture : ShoppingListSearchBarReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        Input = new DomainTestBuilder<string>().Create(),
                        Results = new DomainTestBuilder<SearchItemForShoppingListResult>().CreateMany(2).ToList()
                    },
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        ItemName = new DomainTestBuilder<string>().Create()
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
                    },
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        ItemName = string.Empty
                    }
                };
            }
        }
    }

    private abstract class ShoppingListSearchBarReducerFixture
    {
        public ShoppingListState ExpectedState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
        public ShoppingListState InitialState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
    }
}