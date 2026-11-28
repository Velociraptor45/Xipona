using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
using Xipona.Frontend.Redux.Items.Effects;
using Xipona.Frontend.Redux.Manufacturers.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Manufacturers.States;
using Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace Xipona.Frontend.Redux.Tests.Items.Effects;

public class ManufacturerSelectorEffectsTests
{
    public class HandleLoadInitialManufacturerAction
    {
        private readonly HandleLoadInitialManufacturerActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadInitialManufacturerAction_WithItemNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupItemNull();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ => { });

            // Act
            await sut.HandleLoadInitialManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadInitialManufacturerAction_WithManufacturerIdNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupManufacturerIdNull();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ => { });

            // Act
            await sut.HandleLoadInitialManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadInitialManufacturerAction_WithManufacturerId_ShouldGetManufacturer()
        {
            // Arrange
            _fixture.SetupManufacturerId();
            _fixture.SetupManufacturer();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingManufacturerSucceeded(x0);
                _fixture.SetupDispatchingFinishAction(x0);
            });

            // Act
            await sut.HandleLoadInitialManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadInitialManufacturerAction_WithManufacturerId_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupManufacturerId();
            _fixture.SetupManufacturer();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingManufacturerFailedFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });

            // Act
            await sut.HandleLoadInitialManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadInitialManufacturerAction_WithManufacturerId_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupManufacturerId();
            _fixture.SetupManufacturer();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingManufacturerFailedFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
            });

            // Act
            await sut.HandleLoadInitialManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadInitialManufacturerActionFixture : ManufacturerSelectorEffectsFixture
        {
            private Guid? _itemCategoryId;
            private EditedManufacturer? _itemCategory;

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

            public void SetupManufacturerIdNull()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = State.Editor.Item! with
                        {
                            ManufacturerId = null
                        }
                    }
                };
            }

            public void SetupManufacturerId()
            {
                _itemCategoryId = State.Editor.Item!.ManufacturerId;
            }

            public void SetupManufacturer()
            {
                _itemCategory = new DomainTestBuilder<EditedManufacturer>().Create();
            }

            public void SetupGettingManufacturerSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                ApiClientMock.SetupGetManufacturerByIdAsync(_itemCategoryId.Value, _itemCategory, component);
            }

            public void SetupGettingManufacturerFailedFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);
                ApiClientMock.SetupGetManufacturerByIdAsyncThrowing(_itemCategoryId.Value,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingManufacturerFailedFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);
                ApiClientMock.SetupGetManufacturerByIdAsyncThrowing(_itemCategoryId.Value,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                SetupDispatchingAction(new LoadInitialManufacturerFinishedAction(
                    new ManufacturerSearchResult(_itemCategory.Id, _itemCategory.Name)), component);
            }
        }
    }

    public class HandleCreateNewManufacturerAction
    {
        private readonly HandleCreateNewManufacturerActionFixture _fixture = new();

        [Fact]
        public async Task HandleCreateNewManufacturerAction_WithCreationSucceeded_ShouldCreateManufacturer()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupManufacturer();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupCreatingManufacturerSucceeded(x0);
                _fixture.SetupDispatchingFinishAction(x0);
            });

            // Act
            await sut.HandleCreateNewManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateNewManufacturerAction_WithCreationFailedWithErrorInApi_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupManufacturer();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupCreatingManufacturerFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });

            // Act
            await sut.HandleCreateNewManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleCreateNewManufacturerAction_WithCreationFailedWithErrorWhileTransmittingRequest_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupManufacturer();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupCreatingManufacturerFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
            });

            // Act
            await sut.HandleCreateNewManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleCreateNewManufacturerActionFixture : ManufacturerSelectorEffectsFixture
        {
            private string? _input;
            private EditedManufacturer? _itemCategory;

            public void SetupInput()
            {
                _input = State.Editor.ManufacturerSelector.Input;
            }

            public void SetupManufacturer()
            {
                _itemCategory = new DomainTestBuilder<EditedManufacturer>().Create();
            }

            public void SetupCreatingManufacturerSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                ApiClientMock.SetupCreateManufacturerAsync(_input, _itemCategory, component);
            }

            public void SetupCreatingManufacturerFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupCreateManufacturerAsyncThrowing(_input,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupCreatingManufacturerFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupCreateManufacturerAsyncThrowing(_input,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                SetupDispatchingAction(new CreateNewManufacturerFinishedAction(
                    new ManufacturerSearchResult(_itemCategory.Id, _itemCategory.Name)), component);
            }
        }
    }

    public class HandleManufacturerDropdownClosedAction
    {
        private readonly HandleManufacturerDropdownClosedActionFixture _fixture = new();

        [Fact]
        public async Task HandleManufacturerDropdownClosedAction_WithEmptyInput_ShouldDispatchInputChangedAction()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingInputChangedAction(x0);
            });

            // Act
            await ManufacturerSelectorEffects.HandleManufacturerDropdownClosedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleManufacturerDropdownClosedActionFixture : ManufacturerSelectorEffectsFixture
        {
            public void SetupDispatchingInputChangedAction(IQueueComponent component)
            {
                SetupDispatchingAction(new ManufacturerInputChangedAction(string.Empty), component);
            }
        }
    }

    public class HandleSearchManufacturerAction
    {
        private readonly HandleSearchManufacturerActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchManufacturerAction_WithEmptyInput_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupInputEmpty();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(_ => { });

            // Act
            await sut.HandleSearchManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturerAction_WithInput_ShouldDispatchFinishAction()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupSearchResults();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingManufacturerSucceeded(x0);
                _fixture.SetupDispatchingFinishAction(x0);
            });

            // Act
            await sut.HandleSearchManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturerAction_WithInput_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupSearchResults();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingManufacturerFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });

            // Act
            await sut.HandleSearchManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturerAction_WithInput_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupSearchResults();
            var sut = _fixture.CreateSut();

            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingManufacturerFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
            });

            // Act
            await sut.HandleSearchManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSearchManufacturerActionFixture : ManufacturerSelectorEffectsFixture
        {
            private string? _input;
            private IReadOnlyCollection<ManufacturerSearchResult>? _searchResults;

            public void SetupInput()
            {
                _input = State.Editor.ManufacturerSelector.Input;
            }

            public void SetupInputEmpty()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        ManufacturerSelector = State.Editor.ManufacturerSelector with
                        {
                            Input = string.Empty
                        }
                    }
                };
            }

            public void SetupSearchResults()
            {
                _searchResults = new ManufacturerSearchResultBuilder().CreateMany(3).ToList();
            }

            public void SetupGettingManufacturerSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                TestPropertyNotSetException.ThrowIfNull(_searchResults);
                ApiClientMock.SetupGetManufacturerSearchResultsAsync(_input, _searchResults, component);
            }

            public void SetupGettingManufacturerFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupGetManufacturerSearchResultsAsyncThrowing(_input,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingManufacturerFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupGetManufacturerSearchResultsAsyncThrowing(_input,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResults);
                SetupDispatchingAction(new SearchManufacturerFinishedAction(_searchResults), component);
            }
        }
    }

    public class HandleClearManufacturerAction
    {
        private readonly HandleClearManufacturerActionFixture _fixture = new();

        [Fact]
        public async Task HandleClearManufacturerAction_WithEmptyInput_ShouldDispatchInputChangedAction()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingInputChangedAction(x0);
            });

            // Act
            await ManufacturerSelectorEffects.HandleClearManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleClearManufacturerActionFixture : ManufacturerSelectorEffectsFixture
        {
            public void SetupDispatchingInputChangedAction(IQueueComponent component)
            {
                SetupDispatchingAction(new ManufacturerInputChangedAction(string.Empty), component);
            }
        }
    }

    private abstract class ManufacturerSelectorEffectsFixture : ItemEffectsFixtureBase
    {
        public ManufacturerSelectorEffects CreateSut()
        {
            SetupStateReturningState();
            return new ManufacturerSelectorEffects(ApiClientMock.Object, ItemStateMock.Object);
        }
    }
}