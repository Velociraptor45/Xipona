using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.SideDishes;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Effects;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Recipes.States;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Recipes.Effects;

public class SideDishSelectorEffectsTests
{
    public class HandleSearchSideDishesAction
    {
        private readonly HandleSearchSideDishesActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchSideDishesAction_WithRecipeNull_ShouldNotSearchRecipes()
        {
            // Arrange
            _fixture.SetupRecipeNull();

            var queue = CallQueue.Create(_ => { });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchSideDishesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchSideDishesAction_WithEmptyInput_ShouldNotSearchRecipes()
        {
            // Arrange
            _fixture.SetupSelectorWithoutInput();

            var queue = CallQueue.Create(_ => { });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchSideDishesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchSideDishesAction_WithValidInput_ShouldSearchRecipes()
        {
            // Arrange
            _fixture.SetupSelector();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingSearchResultSucceeded();
                _fixture.SetupDispatchingFinishAction();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchSideDishesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task
            HandleSearchSideDishesAction_WithValidInput_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupSelector();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingSearchResultFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchSideDishesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchSideDishesAction_WithValidInput_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupSelector();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingSearchResultFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchSideDishesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSearchSideDishesActionFixture : SideDishSelectorEffectsFixture
        {
            private IReadOnlyCollection<RecipeSearchResult>? _searchResults;

            public void SetupRecipeNull()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupSelector()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        SideDishSelector = new DomainTestBuilder<SideDishSelector>().Create()
                    }
                };
            }

            public void SetupSelectorWithoutInput()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        SideDishSelector = State.Editor.SideDishSelector with
                        {
                            Input = string.Empty
                        }
                    }
                };
            }

            public void SetupGettingSearchResultSucceeded()
            {
                _searchResults = new RecipeSearchResultBuilder().CreateMany(3).ToList();
                ApiClientMock.SetupSearchRecipesByNameAsync(State.Editor.SideDishSelector.Input, _searchResults);
            }

            public void SetupGettingSearchResultFailedWithErrorInApi()
            {
                ApiClientMock.SetupSearchRecipesByNameAsyncThrowing(State.Editor.SideDishSelector.Input,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingSearchResultFailedWithErrorWhileTransmittingRequest()
            {
                ApiClientMock.SetupSearchRecipesByNameAsyncThrowing(State.Editor.SideDishSelector.Input,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResults);

                SetupDispatchingAction(new SearchSideDishesFinishedAction(_searchResults));
            }
        }
    }

    private abstract class SideDishSelectorEffectsFixture : RecipeEffectsFixtureBase
    {
        public SideDishSelectorEffects CreateSut()
        {
            SetupStateReturningState();
            return new(ApiClientMock.Object, RecipeStateMock.Object);
        }
    }
}
