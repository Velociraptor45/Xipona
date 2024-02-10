using Moq.Contrib.InOrder;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Recipes.Effects;

public class RecipeEffectsTests
{
    public class HandleSearchRecipeByNameAction
    {
        private readonly HandleSearchRecipeByNameActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchRecipeByNameAction_WithValidData_ShouldDispatchFinishedAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupSearchingSuccessfully();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchRecipeByNameAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task HandleSearchRecipeByNameAction_WithEmptySearchInput_ShouldDispatchEmptyFinishedAction(string searchInput)
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput(searchInput);
                _fixture.SetupDispatchingEmptyFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchRecipeByNameAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchRecipeByNameAction_WithErrorInApi_ShouldDispatchApiExceptionNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupSearchingFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchRecipeByNameAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchRecipeByNameAction_WithErrorWhileTransmittingRequest_ShouldDispatchErrorNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupSearchingFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchRecipeByNameAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSearchRecipeByNameActionFixture : RecipeEffectsFixture
        {
            private string? _searchInput;
            private List<RecipeSearchResult>? _expectedRecipeSearchResults;

            public void SetupSearchInput()
            {
                SetupSearchInput(new DomainTestBuilder<string>().Create());
            }

            public void SetupSearchInput(string searchInput)
            {
                _searchInput = searchInput;
                State = State with
                {
                    Search = State.Search with
                    {
                        Input = _searchInput
                    }
                };
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

            public void SetupDispatchingEmptyFinishedAction()
            {
                SetupDispatchingAction(new SearchRecipeFinishedAction(new List<RecipeSearchResult>(), SearchType.Name));
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRecipeSearchResults);
                SetupDispatchingAction(new SearchRecipeFinishedAction(_expectedRecipeSearchResults, SearchType.Name));
            }
        }
    }

    public class HandleSearchRecipeByTagsAction
    {
        private readonly HandleSearchRecipeByTagsActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchRecipeByTagsAction_WithValidData_ShouldDispatchFinishedAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSelectedTags();
                _fixture.SetupSearchingSuccessfully();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchRecipeByTagsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchRecipeByTagsAction_WithoutSelectedTags_ShouldDispatchEmptyFinishedAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupNoSelectedTags();
                _fixture.SetupDispatchingEmptyFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchRecipeByTagsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchRecipeByTagsAction_WithErrorInApi_ShouldDispatchApiExceptionNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSelectedTags();
                _fixture.SetupSearchingFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchRecipeByTagsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchRecipeByTagsAction_WithErrorWhileTransmittingRequest_ShouldDispatchErrorNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSelectedTags();
                _fixture.SetupSearchingFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchRecipeByTagsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSearchRecipeByTagsActionFixture : RecipeEffectsFixture
        {
            private IReadOnlyCollection<Guid>? _selectedTags;
            private List<RecipeSearchResult>? _expectedRecipeSearchResults;

            public void SetupNoSelectedTags()
            {
                SetupSelectedTags(Enumerable.Empty<Guid>());
            }

            public void SetupSelectedTags()
            {
                SetupSelectedTags(new DomainTestBuilder<Guid>().CreateMany(2));
            }

            private void SetupSelectedTags(IEnumerable<Guid> selectedTags)
            {
                _selectedTags = selectedTags.ToList();
                State = State with
                {
                    Search = State.Search with
                    {
                        SelectedRecipeTagIds = _selectedTags
                    }
                };
                SetupStateReturningState();
            }

            public void SetupSearchingSuccessfully()
            {
                TestPropertyNotSetException.ThrowIfNull(_selectedTags);

                _expectedRecipeSearchResults = new DomainTestBuilder<RecipeSearchResult>().CreateMany(2).ToList();
                ApiClientMock.SetupSearchRecipesByTagsAsync(_selectedTags, _expectedRecipeSearchResults);
            }

            public void SetupSearchingFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_selectedTags);
                ApiClientMock.SetupSearchRecipesByTagsAsyncThrowing(_selectedTags,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupSearchingFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_selectedTags);
                ApiClientMock.SetupSearchRecipesByTagsAsyncThrowing(_selectedTags,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingEmptyFinishedAction()
            {
                SetupDispatchingAction(new SearchRecipeFinishedAction(new List<RecipeSearchResult>(), SearchType.Tag));
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedRecipeSearchResults);
                SetupDispatchingAction(new SearchRecipeFinishedAction(_expectedRecipeSearchResults, SearchType.Tag));
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
        }
    }

    private abstract class RecipeEffectsFixture : RecipeEffectsFixtureBase
    {
        public RecipeEffects CreateSut()
        {
            SetupStateReturningState();
            return new RecipeEffects(ApiClientMock.Object, NavigationManagerMock.Object, RecipeStateMock.Object);
        }
    }
}