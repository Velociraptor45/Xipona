using Moq;
using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.Shared.Configurations;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using Xipona.Frontend.Redux.ShoppingList.Actions;
using Xipona.Frontend.Redux.ShoppingList.Actions.SearchBar;
using Xipona.Frontend.Redux.ShoppingList.Effects;
using Xipona.Frontend.Redux.ShoppingList.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.TestTools;
using Xipona.Frontend.TestTools.Exceptions;

namespace Xipona.Frontend.Redux.Tests.ShoppingLists.Effects;

public class ShoppingListSearchBarEffectsTests
{
    public class HandleItemForShoppingListSearchInputChangedAction
    {
        private readonly HandleItemForShoppingListSearchInputChangedActionFixture _fixture = new();

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
            await Task.Delay(1000, TestContext.Current.CancellationToken);

            // Assert
            _fixture.VerifyNotDispatchingSearchAction();
        }

        [Fact]
        public async Task HandleItemForShoppingListSearchInputChangedAction_WithValidInput_ShouldDispatchAction()
        {
            // Arrange
            _fixture.SetupAction();
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupCreatingTimer();
                _fixture.SetupDisposingTimer(x0);
                _fixture.SetupDispatchingSearchAction(x0);
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleItemForShoppingListSearchInputChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);
            _fixture.TimeProviderMock.CapturedCallback?.Invoke(null);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleItemForShoppingListSearchInputChangedActionFixture :
            ShoppingListSearchBarEffectsFixture
        {
            public ItemForShoppingListSearchInputChangedAction? Action { get; private set; }
            private readonly TimerMock _timerMock = new(MockBehavior.Strict);

            public void SetupAction()
            {
                SetupAction(new DomainTestBuilder<string>().Create());
            }

            public void SetupAction(string input)
            {
                Action = new ItemForShoppingListSearchInputChangedAction(input);
            }

            public void SetupDispatchingSearchAction(IQueueComponent component)
            {
                SetupDispatchingAction<SearchItemForShoppingListAction>(component);
            }

            public void SetupDisposingTimer(IQueueComponent component)
            {
                _timerMock.SetupDispose(component);
            }

            public void SetupCreatingTimer()
            {
                TimeProviderMock.SetupCreateTimer(TimeSpan.FromMilliseconds(SearchDelayInMilliseconds),
                    _timerMock.Object);
            }

            public void VerifyNotDispatchingSearchAction()
            {
                VerifyNotDispatchingAction<SearchItemForShoppingListAction>();
            }
        }
    }

    public class HandleSearchItemForShoppingListAction
    {
        private readonly HandleSearchItemForShoppingListActionFixture _fixture = new();

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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupInput();
                _fixture.SetupAction();
                _fixture.SetupSearchResult();
                _fixture.SetupSearchingForItems(x0);
                _fixture.SetupDispatchingFinishAction(x0);
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
            private SearchItemForShoppingListFinishedAction? _expectedAction;

            public SearchItemForShoppingListAction? Action { get; private set; }

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

            public void SetupSearchingForItems(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                ApiClientMock.SetupSearchItemsForShoppingListAsync(_input, State.SelectedStoreId, _searchResult, component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                _expectedAction = new SearchItemForShoppingListFinishedAction(_searchResult);
                SetupDispatchingAction(_expectedAction, component);
            }

            public void VerifyNotDispatchingFinishAction()
            {
                VerifyNotDispatchingAction<SearchItemForShoppingListFinishedAction>();
            }
        }
    }

    public class HandleItemForShoppingListSearchResultSelectedAction
    {
        private readonly HandleItemForShoppingListSearchResultSelectedActionFixture _fixture = new();

        [Fact]
        public async Task HandleItemForShoppingListSearchResultSelectedAction_WithoutType_ShouldCallEndpointAndDispatchActionInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupExpectedRequestWithoutType();
                _fixture.SetupActionWithoutType();
                _fixture.SetupStateWithoutType();
                _fixture.SetupAddingItemWithoutType(x0);
                _fixture.SetupDispatchingReloadShoppingListAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupExpectedRequestWithType();
                _fixture.SetupActionWithType();
                _fixture.SetupStateWithType();
                _fixture.SetupAddingItemWithType(x0);
                _fixture.SetupDispatchingReloadShoppingListAction(x0);
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
                        _expectedRequestWithoutType.ItemId,
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

            public void SetupAddingItemWithoutType(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequestWithoutType);
                ApiClientMock.SetupAddItemToShoppingListAsync(_expectedRequestWithoutType, component);
            }

            public void SetupAddingItemWithType(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRequestWithType);
                ApiClientMock.SetupAddItemWithTypeToShoppingListAsync(_expectedRequestWithType, component);
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

            public void SetupDispatchingReloadShoppingListAction(IQueueComponent component)
            {
                SetupDispatchingAction<ReloadCurrentShoppingListAction>(component);
            }
        }
    }

    private abstract class ShoppingListSearchBarEffectsFixture : ShoppingListEffectsFixtureBase
    {
        protected const int SearchDelayInMilliseconds = 1;

        public TimeProviderMock TimeProviderMock { get; } = new();

        public ShoppingListSearchBarEffects CreateSut()
        {
            var config = new ShoppingListConfiguration()
            {
                SearchDelayAfterInput = TimeSpan.FromMilliseconds(SearchDelayInMilliseconds),
            };

            return new ShoppingListSearchBarEffects(ApiClientMock.Object, ShoppingListStateMock.Object, config, TimeProviderMock.Object);
        }
    }
}