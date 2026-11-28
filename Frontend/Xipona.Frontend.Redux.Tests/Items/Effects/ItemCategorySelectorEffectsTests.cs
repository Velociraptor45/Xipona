using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.ItemCategories.States;
using Xipona.Frontend.Redux.Items.Actions.Editor.ItemCategorySelectors;
using Xipona.Frontend.Redux.Items.Effects;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.ItemCategories.States;
using Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace Xipona.Frontend.Redux.Tests.Items.Effects;

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

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingItemCategorySucceeded(x0);
                _fixture.SetupDispatchingFinishAction(x0);
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

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingItemCategoryFailedFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingItemCategoryFailedFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupGettingItemCategorySucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                ApiClientMock.SetupGetItemCategoryByIdAsync(_itemCategoryId.Value, _itemCategory, component);
            }

            public void SetupGettingItemCategoryFailedFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);
                ApiClientMock.SetupGetItemCategoryByIdAsyncThrowing(_itemCategoryId.Value,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingItemCategoryFailedFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);
                ApiClientMock.SetupGetItemCategoryByIdAsyncThrowing(_itemCategoryId.Value,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                SetupDispatchingAction(new LoadInitialItemCategoryFinishedAction(
                    new ItemCategorySearchResult(_itemCategory.Id, _itemCategory.Name)), component);
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

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupCreatingItemCategorySucceeded(x0);
                _fixture.SetupDispatchingFinishAction(x0);
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

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupCreatingItemCategoryFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupCreatingItemCategoryFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupCreatingItemCategorySucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                ApiClientMock.SetupCreateItemCategoryAsync(_input, _itemCategory, component);
            }

            public void SetupCreatingItemCategoryFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupCreateItemCategoryAsyncThrowing(_input,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupCreatingItemCategoryFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupCreateItemCategoryAsyncThrowing(_input,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                SetupDispatchingAction(new CreateNewItemCategoryFinishedAction(
                    new ItemCategorySearchResult(_itemCategory.Id, _itemCategory.Name)), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingInputChangedAction(x0);
            });

            // Act
            await ItemCategorySelectorEffects.HandleItemCategoryDropdownClosedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleItemCategoryDropdownClosedActionFixture : ItemCategorySelectorEffectsFixture
        {
            public void SetupDispatchingInputChangedAction(IQueueComponent component)
            {
                SetupDispatchingAction(new ItemCategoryInputChangedAction(string.Empty), component);
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

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingItemCategorySucceeded(x0);
                _fixture.SetupDispatchingFinishAction(x0);
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

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingItemCategoryFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupGettingItemCategorySucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                TestPropertyNotSetException.ThrowIfNull(_searchResults);
                ApiClientMock.SetupGetItemCategorySearchResultsAsync(_input, _searchResults, component);
            }

            public void SetupGettingItemCategoryFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupGetItemCategorySearchResultsAsyncThrowing(_input,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingItemCategoryFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupGetItemCategorySearchResultsAsyncThrowing(_input,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResults);
                SetupDispatchingAction(new SearchItemCategoryFinishedAction(_searchResults), component);
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