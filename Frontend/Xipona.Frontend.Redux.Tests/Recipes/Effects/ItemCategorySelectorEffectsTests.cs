using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.Ingredients;
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

    public class HandleSearchItemCategoriesAction
    {
        private readonly HandleSearchItemCategoriesActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithRecipeNull_ShouldNotSearchItemCategories()
        {
            // Arrange
            _fixture.SetupRecipeNull();
            _fixture.SetupActionForInvalidIngredient();

            var queue = CallQueue.Create(_ => { });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithIngredientWithoutInput_ShouldNotSearchItemCategories()
        {
            // Arrange
            _fixture.SetupIngredientWithoutInput();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ => { });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithInvalidIngredientKey_ShouldNotSearchItemCategories()
        {
            // Arrange
            _fixture.SetupIngredient();
            _fixture.SetupActionForInvalidIngredient();

            var queue = CallQueue.Create(_ => { });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithIngredient_ShouldSearchItemCategories()
        {
            // Arrange
            _fixture.SetupIngredient();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingSearchResultSucceeded();
                _fixture.SetupDispatchingFinishAction();
            });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task
            HandleSearchItemCategoriesAction_WithIngredient_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupIngredient();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingSearchResultFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithIngredient_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupIngredient();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingSearchResultFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSearchItemCategoriesActionFixture : ItemCategorySelectorEffectsFixture
        {
            private EditedIngredient? _ingredient;
            private IReadOnlyCollection<ItemCategorySearchResult>? _searchResults;
            public SearchItemCategoriesAction? Action { get; private set; }

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

            public void SetupIngredient()
            {
                _ingredient = new DomainTestBuilder<EditedIngredient>().Create();
                AddIngredientToState();
            }

            public void SetupIngredientWithoutInput()
            {
                _ingredient = new DomainTestBuilder<EditedIngredient>().Create() with
                {
                    ItemCategorySelector = new DomainTestBuilder<ItemCategorySelector>().Create() with
                    {
                        Input = string.Empty
                    }
                };
                AddIngredientToState();
            }

            private void AddIngredientToState()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                var ingredients = State.Editor.Recipe!.Ingredients.ToList();
                ingredients.Add(_ingredient);
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Recipe = State.Editor.Recipe with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupGettingSearchResultSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);

                _searchResults = new ItemCategorySearchResultBuilder().CreateMany(3).ToList();
                ApiClientMock.SetupGetItemCategorySearchResultsAsync(_ingredient.ItemCategorySelector.Input, _searchResults);
            }

            public void SetupGettingSearchResultFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);

                ApiClientMock.SetupGetItemCategorySearchResultsAsyncThrowing(_ingredient.ItemCategorySelector.Input,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingSearchResultFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);

                ApiClientMock.SetupGetItemCategorySearchResultsAsyncThrowing(_ingredient.ItemCategorySelector.Input,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                Action = new SearchItemCategoriesAction(_ingredient.Key);
            }

            public void SetupActionForInvalidIngredient()
            {
                Action = new SearchItemCategoriesAction(Guid.NewGuid());
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                TestPropertyNotSetException.ThrowIfNull(_searchResults);

                SetupDispatchingAction(new SearchItemCategoriesFinishedAction(_searchResults, _ingredient.Key));
            }
        }
    }

    public class HandleSelectedItemCategoryChangedAction
    {
        private readonly HandleSelectedItemCategoryChangedActionFixture _fixture = new();

        [Fact]
        public async Task HandleSelectedItemCategoryChangedAction_WithIngredient_ShouldDispatchCorrectActions()
        {
            // Arrange
            _fixture.SetupIngredient();
            _fixture.SetupItemCategoryId();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingItemCategoryChangedAction();
                _fixture.SetupDispatchingLoadItemsAction();
            });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSelectedItemCategoryChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSelectedItemCategoryChangedAction_WithIngredient_WithItemCategoryIdAlreadySet_ShouldNotDispatchAnyAction()
        {
            // Arrange
            _fixture.SetupIngredient();
            _fixture.SetupItemCategoryIdAlreadySet();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ => { });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSelectedItemCategoryChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSelectedItemCategoryChangedAction_WithInvalidIngredient_ShouldNotDispatchAnyAction()
        {
            // Arrange
            _fixture.SetupActionForInvalidIngredient();

            var queue = CallQueue.Create(_ => { });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSelectedItemCategoryChangedAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSelectedItemCategoryChangedActionFixture : ItemCategorySelectorEffectsFixture
        {
            private EditedIngredient? _ingredient;
            private Guid? _itemCategoryId;
            public SelectedItemCategoryChangedAction? Action { get; private set; }

            public void SetupIngredient()
            {
                _ingredient = new DomainTestBuilder<EditedIngredient>().Create();
                AddIngredientToState();
            }

            public void SetupItemCategoryId()
            {
                _itemCategoryId = Guid.NewGuid();
            }

            public void SetupItemCategoryIdAlreadySet()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                _itemCategoryId = _ingredient.ItemCategoryId;
            }

            private void AddIngredientToState()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                var ingredients = State.Editor.Recipe!.Ingredients.ToList();
                ingredients.Add(_ingredient);
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Recipe = State.Editor.Recipe with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);
                Action = new SelectedItemCategoryChangedAction(_ingredient.Key, _itemCategoryId.Value);
            }

            public void SetupActionForInvalidIngredient()
            {
                Action = new SelectedItemCategoryChangedAction(Guid.NewGuid(), Guid.NewGuid());
            }

            public void SetupDispatchingItemCategoryChangedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);

                SetupDispatchingAction(new ItemCategoryChangedAction(_ingredient.Key, _itemCategoryId.Value));
            }

            public void SetupDispatchingLoadItemsAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);

                SetupDispatchingAction(new LoadItemsForItemCategoryAction(_ingredient.Key, _itemCategoryId.Value));
            }
        }
    }

    public class HandleLoadItemsForItemCategoryAction
    {
        private readonly HandleLoadItemsForItemCategoryActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadItemsForItemCategoryAction_WithApiCallSucceeded_ShouldLoadItemsForItemCategory()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingSearchResultSucceeded();
                _fixture.SetupDispatchingFinishAction();
            });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadItemsForItemCategoryAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadItemsForItemCategoryAction_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingSearchResultFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadItemsForItemCategoryAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task
            HandleLoadItemsForItemCategoryAction_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingSearchResultFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadItemsForItemCategoryAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadItemsForItemCategoryActionFixture : ItemCategorySelectorEffectsFixture
        {
            private Guid _itemCategoryId;
            private IReadOnlyCollection<SearchItemByItemCategoryResult>? _searchResults;
            public LoadItemsForItemCategoryAction? Action { get; private set; }

            public void SetupItemCategoryId()
            {
                _itemCategoryId = Guid.NewGuid();
            }

            public void SetupGettingSearchResultSucceeded()
            {
                _searchResults = new DomainTestBuilder<SearchItemByItemCategoryResult>().CreateMany(3).ToList();
                ApiClientMock.SetupSearchItemByItemCategoryAsync(_itemCategoryId, _searchResults);
            }

            public void SetupGettingSearchResultFailedWithErrorInApi()
            {
                ApiClientMock.SetupSearchItemByItemCategoryAsyncThrowing(_itemCategoryId,
                                       new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingSearchResultFailedWithErrorWhileTransmittingRequest()
            {
                ApiClientMock.SetupSearchItemByItemCategoryAsyncThrowing(_itemCategoryId,
                                       new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupAction()
            {
                Action = new LoadItemsForItemCategoryAction(Guid.NewGuid(), _itemCategoryId);
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResults);
                TestPropertyNotSetException.ThrowIfNull(Action);

                SetupDispatchingAction(new LoadItemsForItemCategoryFinishedAction(Action.IngredientKey, _searchResults));
            }
        }
    }

    public class HandleCreateNewItemCategoryAction
    {
        private readonly HandleCreateNewItemCategoryActionFixture _fixture = new();

        [Fact]
        public async Task HandleCreateNewItemCategoryAction_WithRecipeNull_ShouldNotCreateItemCategory()
        {
            // Arrange
            _fixture.SetupRecipeNull();
            _fixture.SetupActionForInvalidIngredient();

            var queue = CallQueue.Create(_ => { });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleCreateNewItemCategoryAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateNewItemCategoryAction_WithIngredientWithoutItemCategoryInput_ShouldNotCreateItemCategory()
        {
            // Arrange
            _fixture.SetupIngredientWithoutItemCategoryInput();
            _fixture.SetupActionForInvalidIngredient();

            var queue = CallQueue.Create(_ => { });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleCreateNewItemCategoryAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateNewItemCategoryAction_WithIngredient_ShouldCreateItemCategory()
        {
            // Arrange
            _fixture.SetupIngredient();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupCreatingItemCategorySucceeded();
                _fixture.SetupDispatchingFinishAction();
                _fixture.SetupDispatchingLoadItemsAction();
            });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleCreateNewItemCategoryAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task
            HandleCreateNewItemCategoryAction_WithIngredient_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupIngredient();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupCreatingItemCategoryFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleCreateNewItemCategoryAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateNewItemCategoryAction_WithIngredient_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupIngredient();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupCreatingItemCategoryFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleCreateNewItemCategoryAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleCreateNewItemCategoryActionFixture : ItemCategorySelectorEffectsFixture
        {
            private EditedIngredient? _ingredient;
            private EditedItemCategory? _itemCategory;
            public CreateNewItemCategoryAction? Action { get; private set; }

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

            public void SetupIngredient()
            {
                _ingredient = new DomainTestBuilder<EditedIngredient>().Create();
                AddIngredientToState();
            }

            public void SetupIngredientWithoutItemCategoryInput()
            {
                _ingredient = new DomainTestBuilder<EditedIngredient>().Create() with
                {
                    ItemCategorySelector = new DomainTestBuilder<ItemCategorySelector>().Create() with
                    {
                        Input = string.Empty
                    }
                };
                AddIngredientToState();
            }

            private void AddIngredientToState()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                var ingredients = State.Editor.Recipe!.Ingredients.ToList();
                ingredients.Add(_ingredient);
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Recipe = State.Editor.Recipe with
                        {
                            Ingredients = ingredients
                        }
                    }
                };
            }

            public void SetupCreatingItemCategorySucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);

                _itemCategory = new DomainTestBuilder<EditedItemCategory>().Create();
                ApiClientMock.SetupCreateItemCategoryAsync(_ingredient.ItemCategorySelector.Input, _itemCategory);
            }

            public void SetupCreatingItemCategoryFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);

                ApiClientMock.SetupCreateItemCategoryAsyncThrowing(_ingredient.ItemCategorySelector.Input,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupCreatingItemCategoryFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);

                ApiClientMock.SetupCreateItemCategoryAsyncThrowing(_ingredient.ItemCategorySelector.Input,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);

                SetupDispatchingAction(new CreateNewItemCategoryFinishedAction(_ingredient.Key,
                    new ItemCategorySearchResult(_itemCategory.Id, _itemCategory.Name)));
            }

            public void SetupDispatchingLoadItemsAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);

                SetupDispatchingAction(new LoadItemsForItemCategoryAction(_ingredient.Key, _itemCategory.Id));
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredient);
                Action = new CreateNewItemCategoryAction(_ingredient.Key);
            }

            public void SetupActionForInvalidIngredient()
            {
                Action = new CreateNewItemCategoryAction(Guid.NewGuid());
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