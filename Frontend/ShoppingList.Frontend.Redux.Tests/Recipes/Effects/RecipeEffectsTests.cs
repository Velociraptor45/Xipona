using Moq.Contrib.InOrder;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Recipes.Effects;

public class RecipeEffectsTests
{
    public class HandleSearchRecipeAction
    {
        private readonly HandleSearchRecipeActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchRecipeAction_WithValidData_ShouldDispatchFinishedAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupSearchingSuccessfully();
                _fixture.SetupAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchRecipeAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task HandleSearchRecipeAction_WithValidData_ShouldDispatchEmptyFinishedAction(string searchInput)
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput(searchInput);
                _fixture.SetupAction();
                _fixture.SetupDispatchingEmptyFinishedAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchRecipeAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchRecipeAction_WithErrorInApi_ShouldDispatchApiExceptionNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupSearchingFailedWithErrorInApi();
                _fixture.SetupAction();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchRecipeAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchRecipeAction_WithErrorWhileTransmittingRequest_ShouldDispatchErrorNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupSearchingFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupAction();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchRecipeAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSearchRecipeActionFixture : RecipeEffectsFixture
        {
            private string? _searchInput;
            private List<RecipeSearchResult>? _expectedRecipeSearchResults;
            public SearchRecipeAction? Action { get; private set; }

            public void SetupSearchInput()
            {
                SetupSearchInput(new DomainTestBuilder<string>().Create());
            }

            public void SetupSearchInput(string searchInput)
            {
                _searchInput = searchInput;
            }

            public void SetupSearchingSuccessfully()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);

                _expectedRecipeSearchResults = new DomainTestBuilder<RecipeSearchResult>().CreateMany(2).ToList();
                ApiClientMock.SetupSearchRecipesByNameAsync(_searchInput, _expectedRecipeSearchResults);
            }

            public void SetupSearchingFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                ApiClientMock.SetupSearchRecipesByNameAsyncThrowing(_searchInput,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupSearchingFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                ApiClientMock.SetupSearchRecipesByNameAsyncThrowing(_searchInput,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                Action = new SearchRecipeAction(_searchInput);
            }

            public void SetupDispatchingEmptyFinishedAction()
            {
                SetupDispatchingAction(new SearchRecipeFinishedAction(new List<RecipeSearchResult>()));
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRecipeSearchResults);
                SetupDispatchingAction(new SearchRecipeFinishedAction(_expectedRecipeSearchResults));
            }

            public void SetupDispatchingExceptionNotificationAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }

            public void SetupDispatchingErrorNotificationAction()
            {
                SetupDispatchingAnyAction<DisplayErrorNotificationAction>();
            }
        }
    }

    public class HandleLoadRecipeTagsAction
    {
        private readonly HandleLoadRecipeTagsActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadRecipeTagsAction_WithValidData_ShouldDispatchFinishedAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchingSuccessfully();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadRecipeTagsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadRecipeTagsAction_WithErrorInApi_ShouldDispatchApiExceptionNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchingFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadRecipeTagsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadRecipeTagsAction_WithErrorWhileTransmittingRequest_ShouldDispatchErrorNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchingFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadRecipeTagsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadRecipeTagsActionFixture : RecipeEffectsFixture
        {
            private List<RecipeTag>? _expectedRecipeSearchResults;

            public void SetupSearchingSuccessfully()
            {
                _expectedRecipeSearchResults = new DomainTestBuilder<RecipeTag>().CreateMany(2).ToList();
                ApiClientMock.SetupGetAllRecipeTagsAsync(_expectedRecipeSearchResults);
            }

            public void SetupSearchingFailedWithErrorInApi()
            {
                ApiClientMock.SetupGetAllRecipeTagsAsyncThrowing(new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupSearchingFailedWithErrorWhileTransmittingRequest()
            {
                ApiClientMock.SetupGetAllRecipeTagsAsyncThrowing(new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRecipeSearchResults);
                SetupDispatchingAction(new LoadRecipeTagsFinishedAction(_expectedRecipeSearchResults));
            }

            public void SetupDispatchingExceptionNotificationAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }

            public void SetupDispatchingErrorNotificationAction()
            {
                SetupDispatchingAnyAction<DisplayErrorNotificationAction>();
            }
        }
    }

    private abstract class RecipeEffectsFixture : RecipeEffectsFixtureBase
    {
        public RecipeEffects CreateSut()
        {
            SetupStateReturningState();
            return new RecipeEffects(ApiClientMock.Object, NavigationManagerMock.Object);
        }
    }
}