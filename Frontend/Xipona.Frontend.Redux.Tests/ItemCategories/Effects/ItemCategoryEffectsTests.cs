using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.ItemCategories.Actions;
using Xipona.Frontend.Redux.ItemCategories.Effects;
using Xipona.Frontend.Redux.ItemCategories.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace Xipona.Frontend.Redux.Tests.ItemCategories.Effects;

public class ItemCategoryEffectsTests
{
    public class HandleSearchItemCategoriesAction
    {
        private readonly HandleSearchItemCategoriesActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithSearchInputEmpty_ShouldDispatchFinishedActionWithEmptyResult()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInputEmpty();
                _fixture.SetupSearchResultEmpty();
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithSearchInput_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupSearchResult();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupSearchSucceeded(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithWithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupSearchFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithWithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupSearchFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSearchItemCategoriesActionFixture : ItemCategoryEffectsFixture
        {
            private string? _searchInput;
            private IReadOnlyCollection<ItemCategorySearchResult>? _searchResult;

            public void SetupSearchInput()
            {
                _searchInput = new DomainTestBuilder<string>().Create();
                State = State with
                {
                    Search = State.Search with
                    {
                        Input = _searchInput
                    }
                };
            }

            public void SetupSearchInputEmpty()
            {
                _searchInput = string.Empty;
                State = State with
                {
                    Search = State.Search with
                    {
                        Input = _searchInput
                    }
                };
            }

            public void SetupSearchResult()
            {
                _searchResult = new DomainTestBuilder<ItemCategorySearchResult>().CreateMany(2).ToList();
            }

            public void SetupSearchResultEmpty()
            {
                _searchResult = new List<ItemCategorySearchResult>();
            }

            public void SetupSearchSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                ApiClientMock.SetupGetItemCategorySearchResultsAsync(_searchInput, _searchResult, component);
            }

            public void SetupSearchFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);

                ApiClientMock.SetupGetItemCategorySearchResultsAsyncThrowing(_searchInput,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupSearchFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);

                ApiClientMock.SetupGetItemCategorySearchResultsAsyncThrowing(_searchInput,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                SetupDispatchingAction(new SearchItemCategoriesFinishedAction(_searchResult), component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<SearchItemCategoriesStartedAction>(component);
            }
        }
    }

    private abstract class ItemCategoryEffectsFixture : ItemCategoryEffectsFixtureBase
    {
        public ItemCategoryEffects CreateSut()
        {
            SetupStateReturningState();
            return new ItemCategoryEffects(ApiClientMock.Object, NavigationManagerMock.Object,
                ItemCategoryStateMock.Object);
        }
    }
}