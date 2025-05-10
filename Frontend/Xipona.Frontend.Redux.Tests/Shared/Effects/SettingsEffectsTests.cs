using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Actions.Settings;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Effects;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Shared.Effects;

public class SettingsEffectsTests
{
    public class HandleLoadGeneralSettingsAction
    {
        private readonly HandleLoadGeneralSettingsActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadGeneralSettingsAction_WithGeneralSettingsInState_ShouldNotDoAnything()
        {
            // Arrange
            _fixture.SetupStateWithGeneralSettings();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadGeneralSettingsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadGeneralSettingsAction_WithSuccessfulRequest_ShouldDispatchLoadedAction()
        {
            // Arrange
            _fixture.SetupStateWithoutGeneralSettings();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedGeneralSettings();
                _fixture.SetupGettingGeneralSettings();
                _fixture.SetupDispatchingLoadedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadGeneralSettingsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadGeneralSettingsAction_WithApiException_ShouldDispatchExceptionNotificationAction()
        {
            // Arrange
            _fixture.SetupStateWithoutGeneralSettings();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingGeneralSettingsThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadGeneralSettingsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadGeneralSettingsAction_WithHttpRequestException_ShouldDispatchErrorNotificationAction()
        {
            // Arrange
            _fixture.SetupStateWithoutGeneralSettings();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingGeneralSettingsThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadGeneralSettingsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadGeneralSettingsActionFixture : SettingsEffectsFixture
        {
            private GeneralSettings? _expectedGeneralSettings;

            public void SetupExpectedGeneralSettings()
            {
                _expectedGeneralSettings = new DomainTestBuilder<GeneralSettings>().Create();
            }

            public void SetupGettingGeneralSettings()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedGeneralSettings);
                ApiClientMock.SetupGetGeneralSettingsAsync(_expectedGeneralSettings);
            }

            public void SetupGettingGeneralSettingsThrowsApiException()
            {
                ApiClientMock.SetupGetGeneralSettingsAsyncThrowing(
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingGeneralSettingsThrowsHttpRequestException()
            {
                ApiClientMock.SetupGetGeneralSettingsAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingLoadedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedGeneralSettings);
                SetupDispatchingAction(new GeneralSettingsLoadedAction(_expectedGeneralSettings));
            }

            public void SetupStateWithGeneralSettings()
            {
                State = State with
                {
                    Settings = State.Settings with
                    {
                        GeneralSettings = new DomainTestBuilder<GeneralSettings>().Create()
                    }
                };
            }

            public void SetupStateWithoutGeneralSettings()
            {
                State = State with
                {
                    Settings = State.Settings with
                    {
                        GeneralSettings = null
                    }
                };
            }
        }
    }

    public class HandleOpenSettingsAction
    {
        private readonly HandleOpenSettingsActionFixture _fixture = new();

        [Fact]
        public async Task HandleOpenSettingsAction_WithSuccessfulRequest_ShouldDispatchLoadedAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupExpectedCurrencies();
                _fixture.SetupGettingCurrencies();
                _fixture.SetupDispatchingLoadedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleOpenSettingsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleOpenSettingsAction_WithApiException_ShouldDispatchExceptionNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingCurrenciesThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleOpenSettingsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleOpenSettingsAction_WithHttpRequestException_ShouldDispatchErrorNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupGettingCurrenciesThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleOpenSettingsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleOpenSettingsActionFixture : SettingsEffectsFixture
        {
            private List<Currency>? _expectedCurrencies;

            public void SetupExpectedCurrencies()
            {
                _expectedCurrencies = new DomainTestBuilder<Currency>().CreateMany(3).ToList();
            }

            public void SetupGettingCurrencies()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedCurrencies);
                ApiClientMock.SetupGetAllCurrenciesAsync(_expectedCurrencies);
            }

            public void SetupGettingCurrenciesThrowsApiException()
            {
                ApiClientMock.SetupGetAllCurrenciesAsyncThrowing(
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingCurrenciesThrowsHttpRequestException()
            {
                ApiClientMock.SetupGetAllCurrenciesAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingLoadedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedCurrencies);
                SetupDispatchingAction(new SettingsLoadedAction(_expectedCurrencies));
            }
        }
    }

    public class HandleSaveSettingsAction
    {
        private readonly HandleSaveSettingsActionFixture _fixture = new();

        [Fact]
        public async Task HandleSaveSettingsAction_WithoutEditor_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupStateWithoutEditor();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveSettingsAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSaveSettingsAction_WithValidState_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStateWithEditor();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupUpdatingGeneralSettings();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingCloseAction();
                _fixture.SetupSuccessNotification();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveSettingsAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSaveSettingsAction_WithApiException_ShouldDispatchExceptionNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStateWithEditor();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupUpdatingGeneralSettingsThrowsApiException();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveSettingsAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSaveSettingsAction_WithHttpRequestException_ShouldDispatchErrorNotificationAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupStateWithEditor();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupUpdatingGeneralSettingsThrowsHttpRequestException();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSaveSettingsAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSaveSettingsActionFixture : SettingsEffectsFixture
        {
            private Currency? _expectedCurrency;

            public SaveSettingsAction Action { get; } = new();

            public void SetupStateWithEditor()
            {
                _expectedCurrency = new DomainTestBuilder<Currency>().Create();
                State = State with
                {
                    Settings = State.Settings with
                    {
                        Editor = State.Settings.Editor! with
                        {
                            GeneralSettings = new GeneralSettings(_expectedCurrency)
                        }
                    }
                };
            }

            public void SetupStateWithoutEditor()
            {
                State = State with
                {
                    Settings = State.Settings with
                    {
                        Editor = null
                    }
                };
            }

            public void SetupUpdatingGeneralSettings()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedCurrency);
                ApiClientMock.SetupUpdateGeneralSettingsAsync(_expectedCurrency);
            }

            public void SetupUpdatingGeneralSettingsThrowsApiException()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedCurrency);
                ApiClientMock.SetupUpdateGeneralSettingsAsyncThrowing(
                    _expectedCurrency,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupUpdatingGeneralSettingsThrowsHttpRequestException()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedCurrency);
                ApiClientMock.SetupUpdateGeneralSettingsAsyncThrowing(
                    _expectedCurrency,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<SaveSettingsStartedAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                SetupDispatchingAction<SaveSettingsFinishedAction>();
            }

            public void SetupDispatchingCloseAction()
            {
                SetupDispatchingAction<CloseSettingsAction>();
            }

            public void SetupSuccessNotification()
            {
                NotificationServiceMock.SetupNotifySuccess("Successfully saved general settings");
            }
        }
    }

    private abstract class SettingsEffectsFixture : SettingsEffectsFixtureBase
    {
        public SettingsEffects CreateSut()
        {
            SetupStateReturningState();
            return new SettingsEffects(ApiClientMock.Object, SharedStateMock.Object, NotificationServiceMock.Object);
        }
    }
}
