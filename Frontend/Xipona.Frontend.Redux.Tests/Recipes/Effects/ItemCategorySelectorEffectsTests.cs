using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients.ItemCategorySelectors;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Effects;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Recipes.Effects;

public class ItemCategorySelectorEffectsTests
{
    public class HandleLoadInitialItemCategoryAction
    {
        private readonly HandleLoadInitialItemCategoryActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadInitialItemCategoryAction_WithNoItemCategoriesLoaded_ShouldLoadItemCategories()
        {
            // Arrange
            _fixture.SetupIngredientWithoutItemCategories();
            _fixture.SetupItemCategory();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingItemCategorySucceeded();
                _fixture.SetupDispatchingFinishAction();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadInitialItemCategoryAction(_fixture.Action!, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadInitialItemCategoryAction_WithItemCategoriesLoaded_ShouldNotLoadItemCategories()
        {
            // Arrange
            _fixture.SetupIngredientWithItemCategories();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ => { });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadInitialItemCategoryAction(_fixture.Action!, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadInitialItemCategoryAction_WithNoItemCategorySelected_ShouldNotLoadItemCategories()
        {
            // Arrange
            _fixture.SetupIngredientWithoutSelectedItemCategory();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ => { });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadInitialItemCategoryAction(_fixture.Action!, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadInitialItemCategoryAction_WithIngredientNotInState_ShouldNotLoadItemCategories()
        {
            // Arrange
            _fixture.SetupIngredientNotInState();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ => { });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadInitialItemCategoryAction(_fixture.Action!, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadInitialItemCategoryAction_WithNotItemCategoriesLoaded_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupIngredientWithoutItemCategories();
            _fixture.SetupItemCategory();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingItemCategoryFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadInitialItemCategoryAction(_fixture.Action!, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task
            HandleLoadInitialItemCategoryAction_WithNotItemCategoriesLoaded_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupIngredientWithoutItemCategories();
            _fixture.SetupItemCategory();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });

            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadInitialItemCategoryAction(_fixture.Action!, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadInitialItemCategoryActionFixture : ItemCategorySelectorEffectsFixture
        {
            private EditedIngredient? _ingredient;
            private EditedItemCategory? _itemCategory;
            private ItemCategorySearchResult? _searchResult;
            public LoadInitialItemCategoryAction? Action { get; private set; }

            public void SetupIngredientWithoutSelectedItemCategory()
            {
                SetupIngredient(Guid.Empty);
            }

            public void SetupIngredientWithoutItemCategories()
            {
                SetupIngredient(Guid.NewGuid());
            }

            private void SetupIngredient(Guid itemCategoryId)
            {
                var itemCategorySelector = new DomainTestBuilder<ItemCategorySelector>().Create() with
                {
                    ItemCategories = []
                };
                _ingredient = new DomainTestBuilder<EditedIngredient>().Create() with
                {
                    ItemCategorySelector = itemCategorySelector,
                    ItemCategoryId = itemCategoryId
                };

                var ingredients = State.Editor.Recipe!.Ingredients.ToList();
                ingredients.Add(_ingredient);

                State = State with
                {
                    Editor = State.Editor with
                    {
                        Recipe = State.Editor.Recipe with
                        {
                            Ingredients = ingredients,
                        }
                    }
                };
            }

            public void SetupIngredientWithItemCategories()
            {
                _ingredient = new DomainTestBuilder<EditedIngredient>().Create() with
                {
                    ItemCategoryId = Guid.Empty
                };

                var ingredients = State.Editor.Recipe!.Ingredients.ToList();
                ingredients.Add(_ingredient);

                State = State with
                {
                    Editor = State.Editor with
                    {
                        Recipe = State.Editor.Recipe with
                        {
                            Ingredients = ingredients,
                        }
                    }
                };
            }

            public void SetupIngredientNotInState()
            {
                var itemCategorySelector = new DomainTestBuilder<ItemCategorySelector>().Create() with
                {
                    ItemCategories = []
                };
                _ingredient = new DomainTestBuilder<EditedIngredient>().Create() with
                {
                    ItemCategorySelector = itemCategorySelector
                };
            }

            public void SetupItemCategory()
            {
                _searchResult = new ItemCategorySearchResultBuilder().Create();
                _itemCategory = new DomainTestBuilder<EditedItemCategory>().Create() with
                {
                    Id = _searchResult.Id,
                    Name = _searchResult.Name
                };
            }

            public void SetupGettingItemCategorySucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);

                ApiClientMock.SetupGetItemCategoryByIdAsync(_ingredient.ItemCategoryId, _itemCategory);
            }

            public void SetupGettingItemCategoryFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);

                ApiClientMock.SetupGetItemCategoryByIdAsyncThrowing(_ingredient.ItemCategoryId,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);

                ApiClientMock.SetupGetItemCategoryByIdAsyncThrowing(_ingredient.ItemCategoryId,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                Action = new LoadInitialItemCategoryAction(_ingredient);
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                SetupDispatchingAction(new LoadInitialItemCategoryFinishedAction(_ingredient.Key, _searchResult));
            }
        }
    }

    private abstract class ItemCategorySelectorEffectsFixture : RecipeEffectsFixtureBase
    {
        public ItemCategorySelectorEffects CreateSut()
        {
            SetupStateReturningState();
            return new(ApiClientMock.Object, RecipeStateMock.Object);
        }
    }
}