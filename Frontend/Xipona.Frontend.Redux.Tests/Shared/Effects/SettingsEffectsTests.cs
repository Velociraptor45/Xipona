using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.Shared.Actions.Settings;
using Xipona.Frontend.Redux.Shared.Effects;
using Xipona.Frontend.Redux.Shared.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace Xipona.Frontend.Redux.Tests.Shared.Effects;

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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupExpectedGeneralSettings();
                _fixture.SetupGettingGeneralSettings(x0);
                _fixture.SetupDispatchingLoadedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingGeneralSettingsThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingGeneralSettingsThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupGettingGeneralSettings(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedGeneralSettings);
                ApiClientMock.SetupGetGeneralSettingsAsync(_expectedGeneralSettings, component);
            }

            public void SetupGettingGeneralSettingsThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupGetGeneralSettingsAsyncThrowing(
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingGeneralSettingsThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupGetGeneralSettingsAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingLoadedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedGeneralSettings);
                SetupDispatchingAction(new GeneralSettingsLoadedAction(_expectedGeneralSettings), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupExpectedCurrencies();
                _fixture.SetupGettingCurrencies(x0);
                _fixture.SetupDispatchingLoadedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingCurrenciesThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupGettingCurrenciesThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupGettingCurrencies(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedCurrencies);
                ApiClientMock.SetupGetAllCurrenciesAsync(_expectedCurrencies, component);
            }

            public void SetupGettingCurrenciesThrowsApiException(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllCurrenciesAsyncThrowing(
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingCurrenciesThrowsHttpRequestException(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllCurrenciesAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingLoadedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedCurrencies);
                SetupDispatchingAction(new SettingsLoadedAction(_expectedCurrencies), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStateWithEditor();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupUpdatingGeneralSettings(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
                _fixture.SetupDispatchingCloseAction(x0);
                _fixture.SetupSuccessNotification(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStateWithEditor();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupUpdatingGeneralSettingsThrowsApiException(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupStateWithEditor();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupUpdatingGeneralSettingsThrowsHttpRequestException(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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

            public void SetupUpdatingGeneralSettings(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedCurrency);
                ApiClientMock.SetupUpdateGeneralSettingsAsync(_expectedCurrency, component);
            }

            public void SetupUpdatingGeneralSettingsThrowsApiException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedCurrency);
                ApiClientMock.SetupUpdateGeneralSettingsAsyncThrowing(
                    _expectedCurrency,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupUpdatingGeneralSettingsThrowsHttpRequestException(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedCurrency);
                ApiClientMock.SetupUpdateGeneralSettingsAsyncThrowing(
                    _expectedCurrency,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<SaveSettingsStartedAction>(component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                SetupDispatchingAction<SaveSettingsFinishedAction>(component);
            }

            public void SetupDispatchingCloseAction(IQueueComponent component)
            {
                SetupDispatchingAction<CloseSettingsAction>(component);
            }

            public void SetupSuccessNotification(IQueueComponent component)
            {
                NotificationServiceMock.SetupNotifySuccess("Successfully saved general settings", 2f, component);
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
