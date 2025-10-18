using Moq;
using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.Manufacturers.Actions;
using Xipona.Frontend.Redux.Manufacturers.Effects;
using Xipona.Frontend.Redux.Manufacturers.States;
using Xipona.Frontend.Redux.Shared.Ports.Requests.Manufacturers;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Shared.Ports;
using Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace Xipona.Frontend.Redux.Tests.Manufacturers.Effects;

public class ManufacturerEffectsTests
{
    public class HandleSearchManufacturersAction
    {
        private readonly HandleSearchManufacturersActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchManufacturersAction_WithSearchInputEmpty_ShouldDispatchFinishedActionWithEmptyResult()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInputEmpty();
                _fixture.SetupSearchResultEmpty();
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchManufacturersAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturersAction_WithSearchInput_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupSearchResult();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupSearchSucceeded(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchManufacturersAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturersAction_WithWithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupSearchFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchManufacturersAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturersAction_WithWithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupSearchFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchManufacturersAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSearchManufacturersActionFixture : ManufacturerEffectsFixture
        {
            private string? _searchInput;
            private IReadOnlyCollection<ManufacturerSearchResult>? _searchResult;

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
                _searchResult = new DomainTestBuilder<ManufacturerSearchResult>().CreateMany(2).ToList();
            }

            public void SetupSearchResultEmpty()
            {
                _searchResult = new List<ManufacturerSearchResult>();
            }

            public void SetupSearchSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                ApiClientMock.SetupGetManufacturerSearchResultsAsync(_searchInput, _searchResult, component);
            }

            public void SetupSearchFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);

                ApiClientMock.SetupGetManufacturerSearchResultsAsyncThrowing(_searchInput,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupSearchFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);

                ApiClientMock.SetupGetManufacturerSearchResultsAsyncThrowing(_searchInput,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                SetupDispatchingAction(new SearchManufacturersFinishedAction(_searchResult), component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<SearchManufacturersStartedAction>(component);
            }
        }
    }

    public class HandleLoadManufacturerForEditingAction
    {
        private readonly HandleLoadManufacturerForEditingActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchManufacturersAction_WithValidId_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupId();
                _fixture.SetupManufacturer();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupGettingManufacturerSucceeded(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadManufacturerForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturersAction_WithWithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupId();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupGettingManufacturerFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadManufacturerForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturersAction_WithWithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupId();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupGettingManufacturerFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadManufacturerForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadManufacturerForEditingActionFixture : ManufacturerEffectsFixture
        {
            private Guid? _id;
            private EditedManufacturer? _manufacturer;
            public LoadManufacturerForEditingAction? Action { get; private set; }

            public void SetupId()
            {
                _id = Guid.NewGuid();
            }

            public void SetupManufacturer()
            {
                _manufacturer = new DomainTestBuilder<EditedManufacturer>().Create();
            }

            public void SetupGettingManufacturerSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturer);

                ApiClientMock.SetupGetManufacturerByIdAsync(_id.Value, _manufacturer, component);
            }

            public void SetupGettingManufacturerFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupGetManufacturerByIdAsyncThrowing(_id.Value,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingManufacturerFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupGetManufacturerByIdAsyncThrowing(_id.Value,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                Action = new LoadManufacturerForEditingAction(_id.Value);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturer);

                SetupDispatchingAction(new LoadManufacturerForEditingFinishedAction(_manufacturer), component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<LoadManufacturerForEditingStartedAction>(component);
            }
        }
    }

    public class HandleSaveManufacturerAction
    {
        private readonly HandleSaveManufacturerActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchManufacturersAction_WithEmptyId_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupEmptyId();
                _fixture.SetupManufacturerName();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreateManufacturerSucceeded(x0);
                _fixture.SetupSuccessCreateNotification(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturersAction_WithEmptyId_WithWithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupEmptyId();
                _fixture.SetupManufacturerName();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreateManufacturerFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturersAction_WithEmptyId_WithWithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupEmptyId();
                _fixture.SetupManufacturerName();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupCreateManufacturerFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturersAction_WithFilledId_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupId();
                _fixture.SetupManufacturerName();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyManufacturerSucceeded(x0);
                _fixture.SetupUpdateSearchResultsAfterSaveAction(x0);
                _fixture.SetupSuccessModifyNotification(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingLeaveAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturersAction_WithFilledId_WithWithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupId();
                _fixture.SetupManufacturerName();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyManufacturerFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturersAction_WithFilledId_WithWithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupId();
                _fixture.SetupManufacturerName();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupModifyManufacturerFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSaveManufacturerActionFixture : ManufacturerEffectsFixture
        {
            private Guid? _id;
            private string? _manufacturerName;

            public void SetupId()
            {
                _id = Guid.NewGuid();
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Manufacturer = State.Editor.Manufacturer! with
                        {
                            Id = _id.Value
                        }
                    }
                };
            }

            public void SetupEmptyId()
            {
                _id = Guid.Empty;
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Manufacturer = State.Editor.Manufacturer! with
                        {
                            Id = _id.Value
                        }
                    }
                };
            }

            public void SetupManufacturerName()
            {
                _manufacturerName = new DomainTestBuilder<string>().Create();
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Manufacturer = State.Editor.Manufacturer! with
                        {
                            Name = _manufacturerName
                        }
                    }
                };
            }

            public void SetupCreateManufacturerSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupCreateManufacturerAsync(_manufacturerName,
                    new DomainTestBuilder<EditedManufacturer>().Create(), component);
            }

            public void SetupCreateManufacturerFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupCreateManufacturerAsyncThrowing(_manufacturerName,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupCreateManufacturerFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupCreateManufacturerAsyncThrowing(_manufacturerName,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupModifyManufacturerSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupModifyManufacturerAsync(new ModifyManufacturerRequest(_id.Value, _manufacturerName), component);
            }

            public void SetupModifyManufacturerFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupModifyManufacturerAsyncThrowing(new ModifyManufacturerRequest(_id.Value, _manufacturerName),
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupModifyManufacturerFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupModifyManufacturerAsyncThrowing(new ModifyManufacturerRequest(_id.Value, _manufacturerName),
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupSuccessCreateNotification(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully created manufacturer {_manufacturerName}", 2f, component);
            }

            public void SetupSuccessModifyNotification(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully modified manufacturer {_manufacturerName}", 2f, component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                SetupDispatchingAction<SavingManufacturerFinishedAction>(component);
            }

            public void SetupUpdateSearchResultsAfterSaveAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                SetupDispatchingAction(new UpdateSearchResultsAfterSaveAction(_id.Value, _manufacturerName), component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<SavingManufacturerStartedAction>(component);
            }

            public void SetupDispatchingLeaveAction(IQueueComponent component)
            {
                SetupDispatchingAction(new LeaveManufacturerEditorAction(true), component);
            }
        }
    }

    public class HandleDeleteManufacturerAction
    {
        private readonly HandleDeleteManufacturerActionFixture _fixture = new();

        [Fact]
        public async Task HandleDeleteManufacturerAction_WithValidId_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupId();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupGettingManufacturerSucceeded(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupCloseDialogAction(x0);
                _fixture.SetupSuccessNotification(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleDeleteManufacturerAction_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupId();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupGettingManufacturerFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleDeleteManufacturerAction_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupId();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupGettingManufacturerFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleDeleteManufacturerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleDeleteManufacturerActionFixture : ManufacturerEffectsFixture
        {
            private Guid? _id;

            public void SetupId()
            {
                _id = Guid.NewGuid();
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Manufacturer = State.Editor.Manufacturer! with
                        {
                            Id = _id.Value
                        }
                    }
                };
            }

            public void SetupGettingManufacturerSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupDeleteManufacturerAsync(_id.Value, component);
            }

            public void SetupGettingManufacturerFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupDeleteManufacturerAsyncThrowing(_id.Value,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingManufacturerFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupDeleteManufacturerAsyncThrowing(_id.Value,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                SetupDispatchingAction<DeletingManufacturerFinishedAction>(component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<DeletingManufacturerStartedAction>(component);
            }

            public void SetupCloseDialogAction(IQueueComponent component)
            {
                SetupDispatchingAction(new CloseDeleteManufacturerDialogAction(true), component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                ShoppingListNotificationServiceMock
                    .SetupNotifySuccess($"Successfully deleted manufacturer {State.Editor.Manufacturer!.Name}", 2f, component);
            }
        }
    }

    private abstract class ManufacturerEffectsFixture : ManufacturerEffectsFixtureBase
    {
        protected readonly ShoppingListNotificationServiceMock ShoppingListNotificationServiceMock =
            new(MockBehavior.Strict);

        public ManufacturerEffects CreateSut()
        {
            SetupStateReturningState();
            return new ManufacturerEffects(ApiClientMock.Object, NavigationManagerMock.Object,
                ManufacturerStateMock.Object, ShoppingListNotificationServiceMock.Object);
        }
    }
}