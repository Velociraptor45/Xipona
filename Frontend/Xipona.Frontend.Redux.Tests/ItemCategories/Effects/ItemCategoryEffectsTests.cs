using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.Effects;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ItemCategories.Effects;

public class ItemCategoryEffectsTests
{
    public class HandleSearchItemCategoriesAction
    {
        private readonly HandleSearchItemCategoriesActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithSearchInputEmpty_ShouldDispatchFinishedActionWithEmptyResult()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInputEmpty();
                _fixture.SetupSearchResultEmpty();
                _fixture.SetupDispatchingFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupSearchResult();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchSucceeded();
                _fixture.SetupDispatchingFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
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

            public void SetupSearchSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                ApiClientMock.SetupGetItemCategorySearchResultsAsync(_searchInput, _searchResult);
            }

            public void SetupSearchFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);

                ApiClientMock.SetupGetItemCategorySearchResultsAsyncThrowing(_searchInput,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupSearchFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);

                ApiClientMock.SetupGetItemCategorySearchResultsAsyncThrowing(_searchInput,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                SetupDispatchingAction(new SearchItemCategoriesFinishedAction(_searchResult));
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<SearchItemCategoriesStartedAction>();
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