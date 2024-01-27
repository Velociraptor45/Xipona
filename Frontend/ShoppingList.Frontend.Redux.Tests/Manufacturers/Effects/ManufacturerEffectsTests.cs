using Moq;
using Moq.Contrib.InOrder;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Manufacturers;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Manufacturers.Effects;

public class ManufacturerEffectsTests
{
    public class HandleSearchManufacturersAction
    {
        private readonly HandleSearchManufacturersActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchManufacturersAction_WithSearchInputEmpty_ShouldDispatchFinishedActionWithEmptyResult()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInputEmpty();
                _fixture.SetupSearchResultEmpty();
                _fixture.SetupAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchManufacturersAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturersAction_WithSearchInput_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupSearchResult();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchSucceeded();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchManufacturersAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturersAction_WithWithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchManufacturersAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchManufacturersAction_WithWithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchManufacturersAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSearchManufacturersActionFixture : ManufacturerEffectsFixture
        {
            private string? _searchInput;
            private IReadOnlyCollection<ManufacturerSearchResult>? _searchResult;
            public SearchManufacturersAction? Action { get; private set; }

            public void SetupSearchInput()
            {
                _searchInput = new DomainTestBuilder<string>().Create();
            }

            public void SetupSearchInputEmpty()
            {
                _searchInput = string.Empty;
            }

            public void SetupSearchResult()
            {
                _searchResult = new DomainTestBuilder<ManufacturerSearchResult>().CreateMany(2).ToList();
            }

            public void SetupSearchResultEmpty()
            {
                _searchResult = new List<ManufacturerSearchResult>();
            }

            public void SetupSearchSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                ApiClientMock.SetupGetManufacturerSearchResultsAsync(_searchInput, _searchResult);
            }

            public void SetupSearchFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);

                ApiClientMock.SetupGetManufacturerSearchResultsAsyncThrowing(_searchInput,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupSearchFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);

                ApiClientMock.SetupGetManufacturerSearchResultsAsyncThrowing(_searchInput,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);

                Action = new SearchManufacturersAction(_searchInput);
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                SetupDispatchingAction(new SearchManufacturersFinishedAction(_searchResult));
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<SearchManufacturersStartedAction>();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupManufacturer();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupGettingManufacturerSucceeded();
                _fixture.SetupDispatchingFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupGettingManufacturerFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupGettingManufacturerFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
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

            public void SetupGettingManufacturerSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturer);

                ApiClientMock.SetupGetManufacturerByIdAsync(_id.Value, _manufacturer);
            }

            public void SetupGettingManufacturerFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupGetManufacturerByIdAsyncThrowing(_id.Value,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingManufacturerFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupGetManufacturerByIdAsyncThrowing(_id.Value,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                Action = new LoadManufacturerForEditingAction(_id.Value);
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturer);

                SetupDispatchingAction(new LoadManufacturerForEditingFinishedAction(_manufacturer));
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<LoadManufacturerForEditingStartedAction>();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupEmptyId();
                _fixture.SetupManufacturerName();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreateManufacturerSucceeded();
                _fixture.SetupSuccessCreateNotification();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupEmptyId();
                _fixture.SetupManufacturerName();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreateManufacturerFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupEmptyId();
                _fixture.SetupManufacturerName();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupCreateManufacturerFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupManufacturerName();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyManufacturerSucceeded();
                _fixture.SetupUpdateSearchResultsAfterSaveAction();
                _fixture.SetupSuccessModifyNotification();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupManufacturerName();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyManufacturerFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupManufacturerName();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupModifyManufacturerFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
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

            public void SetupCreateManufacturerSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupCreateManufacturerAsync(_manufacturerName,
                    new DomainTestBuilder<EditedManufacturer>().Create());
            }

            public void SetupCreateManufacturerFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupCreateManufacturerAsyncThrowing(_manufacturerName,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupCreateManufacturerFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupCreateManufacturerAsyncThrowing(_manufacturerName,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupModifyManufacturerSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupModifyManufacturerAsync(new ModifyManufacturerRequest(_id.Value, _manufacturerName));
            }

            public void SetupModifyManufacturerFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupModifyManufacturerAsyncThrowing(new ModifyManufacturerRequest(_id.Value, _manufacturerName),
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupModifyManufacturerFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ApiClientMock.SetupModifyManufacturerAsyncThrowing(new ModifyManufacturerRequest(_id.Value, _manufacturerName),
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupSuccessCreateNotification()
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ShoppingListNotificationServiceMock.SetupNotifySuccessAsync($"Successfully created manufacturer {_manufacturerName}");
            }

            public void SetupSuccessModifyNotification()
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                ShoppingListNotificationServiceMock.SetupNotifySuccessAsync($"Successfully modified manufacturer {_manufacturerName}");
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                SetupDispatchingAction<SavingManufacturerFinishedAction>();
            }

            public void SetupUpdateSearchResultsAfterSaveAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);
                TestPropertyNotSetException.ThrowIfNull(_manufacturerName);

                SetupDispatchingAction(new UpdateSearchResultsAfterSaveAction(_id.Value, _manufacturerName));
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<SavingManufacturerStartedAction>();
            }

            public void SetupDispatchingLeaveAction()
            {
                SetupDispatchingAction<LeaveManufacturerEditorAction>();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupGettingManufacturerSucceeded();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupCloseDialogAction();
                _fixture.SetupSuccessNotification();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupGettingManufacturerFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
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
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupId();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupGettingManufacturerFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
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

            public void SetupGettingManufacturerSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupDeleteManufacturerAsync(_id.Value);
            }

            public void SetupGettingManufacturerFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupDeleteManufacturerAsyncThrowing(_id.Value,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingManufacturerFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_id);

                ApiClientMock.SetupDeleteManufacturerAsyncThrowing(_id.Value,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishedAction()
            {
                SetupDispatchingAction<DeletingManufacturerFinishedAction>();
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<DeletingManufacturerStartedAction>();
            }

            public void SetupCloseDialogAction()
            {
                SetupDispatchingAction(new CloseDeleteManufacturerDialogAction(true));
            }

            public void SetupSuccessNotification()
            {
                ShoppingListNotificationServiceMock.SetupNotifySuccessAsync($"Successfully deleted manufacturer {State.Editor.Manufacturer!.Name}");
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