using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Configurations;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.SearchBar;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ShoppingLists.Effects;

public class ShoppingListSearchBarEffectsTests
{
    public class HandleItemForShoppingListSearchInputChangedAction
    {
        private readonly HandleItemForShoppingListSearchInputChangedActionFixture _fixture;

        public HandleItemForShoppingListSearchInputChangedAction()
        {
            _fixture = new HandleItemForShoppingListSearchInputChangedActionFixture();
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        public async Task HandleItemForShoppingListSearchInputChangedAction_WithEmptyInput_ShouldNotDispatchAction(
            string input)
        {
            // Arrange
            _fixture.SetupAction(input);
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleItemForShoppingListSearchInputChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);
            await Task.Delay(10);

            // Assert
            _fixture.VerifyNotDispatchingSearchAction();
        }

        [Fact]
        public async Task HandleItemForShoppingListSearchInputChangedAction_WithValidInput_ShouldDispatchAction()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupDispatchingSearchAction();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleItemForShoppingListSearchInputChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);
            await Task.Delay(10);

            // Assert
            _fixture.VerifyDispatchingSearchAction();
        }

        [Fact]
        public async Task HandleItemForShoppingListSearchInputChangedAction_WithRepeatedInvokation_ShouldDispatchAction()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupDispatchingSearchAction();
            _fixture.SetupLongSearchDelay();

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleItemForShoppingListSearchInputChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);
            await sut.HandleItemForShoppingListSearchInputChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);
            await Task.Delay(250);

            // Assert
            _fixture.VerifyDispatchingSearchAction();
        }

        private sealed class HandleItemForShoppingListSearchInputChangedActionFixture :
            ShoppingListSearchBarEffectsFixture
        {
            public ItemForShoppingListSearchInputChangedAction? Action { get; private set; }

            public void SetupAction()
            {
                SetupAction(new DomainTestBuilder<string>().Create());
            }

            public void SetupAction(string input)
            {
                Action = new ItemForShoppingListSearchInputChangedAction(input);
            }

            public void SetupLongSearchDelay()
            {
                SearchDelayInMilliseconds = 100;
            }

            public void SetupDispatchingSearchAction()
            {
                SetupDispatchingAction<SearchItemForShoppingListAction>();
            }

            public void VerifyDispatchingSearchAction()
            {
                VerifyDispatchingAction<SearchItemForShoppingListAction>();
            }

            public void VerifyNotDispatchingSearchAction()
            {
                VerifyNotDispatchingAction<SearchItemForShoppingListAction>();
            }
        }
    }

    public class HandleSearchItemForShoppingListAction
    {
        private readonly HandleSearchItemForShoppingListActionFixture _fixture;

        public HandleSearchItemForShoppingListAction()
        {
            _fixture = new HandleSearchItemForShoppingListActionFixture();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task HandleSearchItemForShoppingListAction_WithEmptyInput_ShouldDoNothing(string input)
        {
            // Arrange
            _fixture.SetupInput(input);
            _fixture.SetupAction();

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchItemForShoppingListAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyNotDispatchingFinishAction();
        }

        [Fact]
        public async Task HandleSearchItemForShoppingListAction_WithValidInput_ShouldDispatchExpectedAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupInput();
                _fixture.SetupAction();
                _fixture.SetupSearchResult();
                _fixture.SetupSearchingForItems();
                _fixture.SetupDispatchingFinishAction();
            });

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchItemForShoppingListAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSearchItemForShoppingListActionFixture : ShoppingListSearchBarEffectsFixture
        {
            private string? _input;
            private List<SearchItemForShoppingListResult>? _searchResult;

            public SearchItemForShoppingListAction? Action { get; private set; }
            public SearchItemForShoppingListFinishedAction? ExpectedAction { get; private set; }

            public void SetupInput()
            {
                SetupInput(new DomainTestBuilder<string>().Create());
            }

            public void SetupInput(string input)
            {
                _input = input;
                State = State with
                {
                    SearchBar = State.SearchBar with
                    {
                        Input = _input
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SearchItemForShoppingListAction();
            }

            public void SetupSearchResult()
            {
                _searchResult = new DomainTestBuilder<SearchItemForShoppingListResult>().CreateMany(2).ToList();
            }

            public void SetupSearchingForItems()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                ApiClientMock.SetupSearchItemsForShoppingListAsync(_input, State.SelectedStoreId, _searchResult);
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                ExpectedAction = new SearchItemForShoppingListFinishedAction(_searchResult);
                SetupDispatchingAction(ExpectedAction);
            }

            public void VerifyNotDispatchingFinishAction()
            {
                VerifyNotDispatchingAction<SearchItemForShoppingListFinishedAction>();
            }
        }
    }

    public class HandleItemForShoppingListSearchResultSelectedAction
    {
        private readonly HandleItemForShoppingListSearchResultSelectedActionFixture _fixture;

        public HandleItemForShoppingListSearchResultSelectedAction()
        {
            _fixture = new HandleItemForShoppingListSearchResultSelectedActionFixture();
        }

        [Fact]
        public async Task HandleItemForShoppingListSearchResultSelectedAction_WithoutType_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedRequestWithoutType();
                _fixture.SetupActionWithoutType();
                _fixture.SetupStateWithoutType();
                _fixture.SetupAddingItemWithoutType();
                _fixture.SetupDispatchingChangeAction();
            });

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleItemForShoppingListSearchResultSelectedAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyAddingItemWithoutType();
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleItemForShoppingListSearchResultSelectedAction_WithType_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedRequestWithType();
                _fixture.SetupActionWithType();
                _fixture.SetupStateWithType();
                _fixture.SetupAddingItemWithType();
                _fixture.SetupDispatchingChangeAction();
            });

            _fixture.SetupStateReturningState();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleItemForShoppingListSearchResultSelectedAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            _fixture.VerifyAddingItemWithType();
            queue.VerifyOrder();
        }

        private sealed class HandleItemForShoppingListSearchResultSelectedActionFixture :
            ShoppingListSearchBarEffectsFixture
        {
            private AddItemToShoppingListRequest? _expectedRequestWithoutType;
            private AddItemWithTypeToShoppingListRequest? _expectedRequestWithType;
            private SelectedStoreChangedAction? _expectedChangeAction;

            public ItemForShoppingListSearchResultSelectedAction? Action { get; private set; }

            public void SetupExpectedRequestWithoutType()
            {
                _expectedRequestWithoutType = new DomainTestBuilder<AddItemToShoppingListRequest>()
                    .FillConstructorWith("quantity", (float)new DomainTestBuilder<int>().Create())
                    .Create();
            }

            public void SetupExpectedRequestWithType()
            {
                _expectedRequestWithType = new DomainTestBuilder<AddItemWithTypeToShoppingListRequest>().Create();
            }

            public void SetupStateWithoutType()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequestWithoutType);

                State = State with
                {
                    ShoppingList = State.ShoppingList! with
                    {
                        Id = _expectedRequestWithoutType.ShoppingListId
                    }
                };
            }

            public void SetupStateWithType()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequestWithType);

                State = State with
                {
                    ShoppingList = State.ShoppingList! with
                    {
                        Id = _expectedRequestWithType.ShoppingListId
                    }
                };
            }

            public void SetupActionWithoutType()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequestWithoutType);

                Action = new ItemForShoppingListSearchResultSelectedAction(
                    new SearchItemForShoppingListResult(
                        _expectedRequestWithoutType.ItemId.ActualId!.Value,
                        null,
                        "",
                        0,
                        (int)_expectedRequestWithoutType.Quantity,
                        "",
                        "",
                        "",
                        _expectedRequestWithoutType.SectionId!.Value));
            }

            public void SetupActionWithType()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequestWithType);

                Action = new ItemForShoppingListSearchResultSelectedAction(
                    new SearchItemForShoppingListResult(
                        _expectedRequestWithType.ItemId,
                        _expectedRequestWithType.ItemTypeId,
                        "",
                        0,
                        (int)_expectedRequestWithType.Quantity,
                        "",
                        "",
                        "",
                        _expectedRequestWithType.SectionId!.Value));
            }

            public void SetupAddingItemWithoutType()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequestWithoutType);
                ApiClientMock.SetupAddItemToShoppingListAsync(_expectedRequestWithoutType);
            }

            public void SetupAddingItemWithType()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequestWithType);
                ApiClientMock.SetupAddItemWithTypeToShoppingListAsync(_expectedRequestWithType);
            }

            public void VerifyAddingItemWithoutType()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequestWithoutType);
                ApiClientMock.VerifyAddItemToShoppingListAsync(_expectedRequestWithoutType, Times.Once);
            }

            public void VerifyAddingItemWithType()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequestWithType);
                ApiClientMock.VerifyAddItemWithTypeToShoppingListAsync(_expectedRequestWithType, Times.Once);
            }

            public void SetupDispatchingChangeAction()
            {
                _expectedChangeAction = new SelectedStoreChangedAction(State.SelectedStoreId);
                SetupDispatchingAction(_expectedChangeAction);
            }

            public void VerifyNotDispatchingChangeAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedChangeAction);
                VerifyNotDispatchingAction<SelectedStoreChangedAction>();
            }
        }
    }

    private abstract class ShoppingListSearchBarEffectsFixture : ShoppingListEffectsFixtureBase
    {
        protected int SearchDelayInMilliseconds = 1;

        public ShoppingListSearchBarEffects CreateSut()
        {
            var config = new ShoppingListConfiguration()
            {
                SearchDelayAfterInput = TimeSpan.FromMilliseconds(SearchDelayInMilliseconds),
            };

            return new ShoppingListSearchBarEffects(ApiClientMock.Object, ShoppingListStateMock.Object, config);
        }
    }
}