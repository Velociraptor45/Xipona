using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
using ProjectHermes.Xipona.Frontend.Redux.Items.Effects;
using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Manufacturers.States;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Items.Effects;

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

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingManufacturerSucceeded();
                _fixture.SetupDispatchingFinishAction();
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

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingManufacturerFailedFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
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

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingManufacturerFailedFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
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

            public void SetupGettingManufacturerSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                ApiClientMock.SetupGetManufacturerByIdAsync(_itemCategoryId.Value, _itemCategory);
            }

            public void SetupGettingManufacturerFailedFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);
                ApiClientMock.SetupGetManufacturerByIdAsyncThrowing(_itemCategoryId.Value,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingManufacturerFailedFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategoryId);
                ApiClientMock.SetupGetManufacturerByIdAsyncThrowing(_itemCategoryId.Value,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                SetupDispatchingAction(new LoadInitialManufacturerFinishedAction(
                    new ManufacturerSearchResult(_itemCategory.Id, _itemCategory.Name)));
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

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupCreatingManufacturerSucceeded();
                _fixture.SetupDispatchingFinishAction();
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

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupCreatingManufacturerFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
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

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupCreatingManufacturerFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
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

            public void SetupCreatingManufacturerSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                ApiClientMock.SetupCreateManufacturerAsync(_input, _itemCategory);
            }

            public void SetupCreatingManufacturerFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupCreateManufacturerAsyncThrowing(_input,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupCreatingManufacturerFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupCreateManufacturerAsyncThrowing(_input,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                SetupDispatchingAction(new CreateNewManufacturerFinishedAction(
                    new ManufacturerSearchResult(_itemCategory.Id, _itemCategory.Name)));
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingInputChangedAction();
            });

            // Act
            await ManufacturerSelectorEffects.HandleManufacturerDropdownClosedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleManufacturerDropdownClosedActionFixture : ManufacturerSelectorEffectsFixture
        {
            public void SetupDispatchingInputChangedAction()
            {
                SetupDispatchingAction(new ManufacturerInputChangedAction(string.Empty));
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

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingManufacturerSucceeded();
                _fixture.SetupDispatchingFinishAction();
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

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingManufacturerFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
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

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingManufacturerFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
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

            public void SetupGettingManufacturerSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                TestPropertyNotSetException.ThrowIfNull(_searchResults);
                ApiClientMock.SetupGetManufacturerSearchResultsAsync(_input, _searchResults);
            }

            public void SetupGettingManufacturerFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupGetManufacturerSearchResultsAsyncThrowing(_input,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingManufacturerFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);
                ApiClientMock.SetupGetManufacturerSearchResultsAsyncThrowing(_input,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResults);
                SetupDispatchingAction(new SearchManufacturerFinishedAction(_searchResults));
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingInputChangedAction();
            });

            // Act
            await ManufacturerSelectorEffects.HandleClearManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleClearManufacturerActionFixture : ManufacturerSelectorEffectsFixture
        {
            public void SetupDispatchingInputChangedAction()
            {
                SetupDispatchingAction(new ManufacturerInputChangedAction(string.Empty));
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