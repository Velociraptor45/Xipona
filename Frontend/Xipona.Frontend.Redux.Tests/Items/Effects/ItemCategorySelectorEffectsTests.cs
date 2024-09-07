using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.ItemCategorySelectors;
using ProjectHermes.Xipona.Frontend.Redux.Items.Effects;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.ItemCategories.States;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Items.Effects;

public class ItemCategorySelectorEffectsTests
{
    public class HandleLoadInitialItemCategoryAction
    {
        private readonly HandleLoadInitialItemCategoryActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadInitialItemCategoryAction_WithItemNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupItemNull();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ => { });

            // Act
            await sut.HandleLoadInitialItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadInitialItemCategoryAction_WithItemCategoryIdNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupItemCategoryIdNull();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ => { });

            // Act
            await sut.HandleLoadInitialItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadInitialItemCategoryAction_WithItemCategoryId_ShouldGetItemCategory()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupItemCategory();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingItemCategorySucceeded();
                _fixture.SetupDispatchingFinishAction();
            });

            // Act
            await sut.HandleLoadInitialItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadInitialItemCategoryAction_WithItemCategoryId_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupItemCategory();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingItemCategoryFailedFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });

            // Act
            await sut.HandleLoadInitialItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadInitialItemCategoryAction_WithItemCategoryId_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupItemCategory();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingItemCategoryFailedFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });

            // Act
            await sut.HandleLoadInitialItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadInitialItemCategoryActionFixture : ItemCategorySelectorEffectsFixture
        {
            private Guid? _itemCategoryId;
            private EditedItemCategory? _itemCategory;

            public void SetupItemNull()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = null
                    }
                };
            }

            public void SetupItemCategoryIdNull()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            ItemCategoryId = null
                        }
                    }
                };
            }

            public void SetupItemCategoryId()
            {
                _itemCategoryId = State.Editor.Item!.ItemCategoryId;
            }

            public void SetupItemCategory()
            {
                _itemCategory = new DomainTestBuilder<EditedItemCategory>().Create();
            }

            public void SetupGettingItemCategorySucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                ApiClientMock.SetupGetItemCategoryByIdAsync(_itemCategoryId.Value, _itemCategory);
            }

            public void SetupGettingItemCategoryFailedFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);
                ApiClientMock.SetupGetItemCategoryByIdAsyncThrowing(_itemCategoryId.Value,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingItemCategoryFailedFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);
                ApiClientMock.SetupGetItemCategoryByIdAsyncThrowing(_itemCategoryId.Value,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                SetupDispatchingAction(new LoadInitialItemCategoryFinishedAction(
                    new ItemCategorySearchResult(_itemCategory.Id, _itemCategory.Name)));
            }
        }
    }

    public class HandleCreateNewItemCategoryAction
    {
        private readonly HandleCreateNewItemCategoryActionFixture _fixture = new();

        [Fact]
        public async Task HandleCreateNewItemCategoryAction_WithCreationSucceeded_ShouldCreateItemCategory()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupItemCategory();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupCreatingItemCategorySucceeded();
                _fixture.SetupDispatchingFinishAction();
            });

            // Act
            await sut.HandleCreateNewItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateNewItemCategoryAction_WithCreationFailedWithErrorInApi_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupItemCategory();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupCreatingItemCategoryFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });

            // Act
            await sut.HandleCreateNewItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateNewItemCategoryAction_WithCreationFailedWithErrorWhileTransmittingRequest_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupItemCategory();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupCreatingItemCategoryFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });

            // Act
            await sut.HandleCreateNewItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleCreateNewItemCategoryActionFixture : ItemCategorySelectorEffectsFixture
        {
            private string? _input;
            private EditedItemCategory? _itemCategory;

            public void SetupInput()
            {
                _input = State.Editor.ItemCategorySelector.Input;
            }

            public void SetupItemCategory()
            {
                _itemCategory = new DomainTestBuilder<EditedItemCategory>().Create();
            }

            public void SetupCreatingItemCategorySucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                ApiClientMock.SetupCreateItemCategoryAsync(_input, _itemCategory);
            }

            public void SetupCreatingItemCategoryFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupCreateItemCategoryAsyncThrowing(_input,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupCreatingItemCategoryFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupCreateItemCategoryAsyncThrowing(_input,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                SetupDispatchingAction(new CreateNewItemCategoryFinishedAction(
                    new ItemCategorySearchResult(_itemCategory.Id, _itemCategory.Name)));
            }
        }
    }

    public class HandleItemCategoryDropdownClosedAction
    {
        private readonly HandleItemCategoryDropdownClosedActionFixture _fixture = new();

        [Fact]
        public async Task HandleItemCategoryDropdownClosedAction_WithEmptyInput_ShouldDispatchInputChangedAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingInputChangedAction();
            });

            // Act
            await ItemCategorySelectorEffects.HandleItemCategoryDropdownClosedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleItemCategoryDropdownClosedActionFixture : ItemCategorySelectorEffectsFixture
        {
            public void SetupDispatchingInputChangedAction()
            {
                SetupDispatchingAction(new ItemCategoryInputChangedAction(string.Empty));
            }
        }
    }

    public class HandleSearchItemCategoryAction
    {
        private readonly HandleSearchItemCategoryActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchItemCategoryAction_WithEmptyInput_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupInputEmpty();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ => { });

            // Act
            await sut.HandleSearchItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoryAction_WithInput_ShouldDispatchFinishAction()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupSearchResults();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingItemCategorySucceeded();
                _fixture.SetupDispatchingFinishAction();
            });

            // Act
            await sut.HandleSearchItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoryAction_WithInput_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupSearchResults();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingItemCategoryFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });

            // Act
            await sut.HandleSearchItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoryAction_WithInput_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupSearchResults();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });

            // Act
            await sut.HandleSearchItemCategoryAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSearchItemCategoryActionFixture : ItemCategorySelectorEffectsFixture
        {
            private string? _input;
            private IReadOnlyCollection<ItemCategorySearchResult>? _searchResults;

            public void SetupInput()
            {
                _input = State.Editor.ItemCategorySelector.Input;
            }

            public void SetupInputEmpty()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ItemCategorySelector = State.Editor.ItemCategorySelector with
                        {
                            Input = string.Empty
                        }
                    }
                };
            }

            public void SetupSearchResults()
            {
                _searchResults = new ItemCategorySearchResultBuilder().CreateMany(3).ToList();
            }

            public void SetupGettingItemCategorySucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                TestPropertyNotSetException.ThrowIfNull(_searchResults);
                ApiClientMock.SetupGetItemCategorySearchResultsAsync(_input, _searchResults);
            }

            public void SetupGettingItemCategoryFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupGetItemCategorySearchResultsAsyncThrowing(_input,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupGetItemCategorySearchResultsAsyncThrowing(_input,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResults);
                SetupDispatchingAction(new SearchItemCategoryFinishedAction(_searchResults));
            }
        }
    }

    private abstract class ItemCategorySelectorEffectsFixture : ItemEffectsFixtureBase
    {
        public ItemCategorySelectorEffects CreateSut()
        {
            SetupStateReturningState();
            return new ItemCategorySelectorEffects(ApiClientMock.Object, ItemStateMock.Object);
        }
    }
}