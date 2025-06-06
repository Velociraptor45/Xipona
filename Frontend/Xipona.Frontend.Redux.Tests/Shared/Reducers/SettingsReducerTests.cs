using FluentAssertions;
using Xipona.Frontend.Redux.Shared.Actions.Settings;
using Xipona.Frontend.Redux.Shared.Reducers;
using Xipona.Frontend.Redux.Shared.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.TestTools.Exceptions;

namespace Xipona.Frontend.Redux.Tests.Shared.Reducers;

public class SettingsReducerTests
{
    public class OnCloseSettings
    {
        private readonly OnCloseSettingsFixture _fixture = new();

        [Fact]
        public void OnCloseSettings_WithSettingsOpen_ShouldCloseSettings()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = SettingsReducer.OnCloseSettings(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCloseSettings_WithSettingsAlreadyClosed_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupInitialStateAlreadyClosed();
            _fixture.SetupExpectedState();

            // Act
            var result = SettingsReducer.OnCloseSettings(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnCloseSettingsFixture : SettingsReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with
                    {
                        SettingsOpen = true,
                        Editor = new DomainTestBuilder<SettingsEditor>().Create()
                    }
                };
            }

            public void SetupInitialStateAlreadyClosed()
            {
                InitialState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with
                    {
                        SettingsOpen = false,
                        Editor = new DomainTestBuilder<SettingsEditor>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with
                    {
                        SettingsOpen = false,
                        Editor = null
                    }
                };
            }
        }
    }

    public class OnOpenSettings
    {
        private readonly OnOpenSettingsFixture _fixture = new();

        [Fact]
        public void OnOpenSettings_WithSettingsClosed_ShouldOpenSettings()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = SettingsReducer.OnOpenSettings(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnOpenSettings_WithSettingsAlreadyOpen_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupInitialStateAlreadyOpen();
            _fixture.SetupExpectedState();

            // Act
            var result = SettingsReducer.OnOpenSettings(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnOpenSettingsFixture : SettingsReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with { SettingsOpen = false }
                };
            }

            public void SetupInitialStateAlreadyOpen()
            {
                InitialState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with { SettingsOpen = true }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with { SettingsOpen = true }
                };
            }
        }
    }

    public class OnSettingsLoaded
    {
        private readonly OnSettingsLoadedFixture _fixture = new();

        [Fact]
        public void OnSettingsLoaded_WithEditorInitialized_ShouldUpdateSettings()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = SettingsReducer.OnSettingsLoaded(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSettingsLoaded_WithEditorNull_ShouldUpdateSettings()
        {
            // Arrange
            _fixture.SetupInitialStateWithEditorNull();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = SettingsReducer.OnSettingsLoaded(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSettingsLoadedFixture : SettingsReducerFixture
        {
            public SettingsLoadedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with
                    {
                        Editor = new DomainTestBuilder<SettingsEditor>().Create()
                    }
                };
            }

            public void SetupInitialStateWithEditorNull()
            {
                InitialState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with
                    {
                        Editor = null
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SettingsLoadedAction(ExpectedState.Settings.Editor!.AllCurrencies);
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with
                    {
                        Editor = ExpectedState.Settings.Editor! with
                        {
                            GeneralSettings = ExpectedState.Settings.GeneralSettings!
                        }
                    }
                };
            }
        }
    }

    public class OnSelectedCurrencyChanged
    {
        private readonly OnSelectedCurrencyChangedFixture _fixture = new();

        [Fact]
        public void OnSelectedCurrencyChanged_ShouldUpdateCurrency()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = SettingsReducer.OnSelectedCurrencyChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSelectedCurrencyChangedFixture : SettingsReducerFixture
        {
            public SelectedCurrencyChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with
                    {
                        Editor = ExpectedState.Settings.Editor! with
                        {
                            GeneralSettings = ExpectedState.Settings.Editor.GeneralSettings with
                            {
                                Currency = new DomainTestBuilder<Currency>().Create()
                            }
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SelectedCurrencyChangedAction(ExpectedState.Settings.Editor!.GeneralSettings.Currency);
            }
        }
    }

    public class OnSaveSettingsStarted
    {
        private readonly OnSaveSettingsStartedFixture _fixture = new();

        [Fact]
        public void OnSaveSettingsStarted_WithNotSaving_ShouldSetIsSavingToTrue()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = SettingsReducer.OnSaveSettingsStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSaveSettingsStarted_WithAlreadySaving_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupInitialStateAlreadySaving();
            _fixture.SetupExpectedState();

            // Act
            var result = SettingsReducer.OnSaveSettingsStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSaveSettingsStartedFixture : SettingsReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with { IsSaving = false }
                };
            }

            public void SetupInitialStateAlreadySaving()
            {
                InitialState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with { IsSaving = true }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with { IsSaving = true }
                };
            }
        }
    }

    public class OnSaveSettingsFinished
    {
        private readonly OnSaveSettingsFinishedFixture _fixture = new();

        [Fact]
        public void OnSaveSettingsFinished_WithSaving_ShouldSetIsSavingToFalse()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = SettingsReducer.OnSaveSettingsFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSaveSettingsFinished_WithAlreadyNotSaving_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialStateAlreadyNotSaving();

            // Act
            var result = SettingsReducer.OnSaveSettingsFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSaveSettingsFinishedFixture : SettingsReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with
                    {
                        Editor = new DomainTestBuilder<SettingsEditor>().Create() with
                        {
                            GeneralSettings = ExpectedState.Settings.GeneralSettings!
                        },
                        GeneralSettings = new DomainTestBuilder<GeneralSettings>().Create(),
                        IsSaving = true
                    }
                };
            }

            public void SetupInitialStateAlreadyNotSaving()
            {
                InitialState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with
                    {
                        Editor = new DomainTestBuilder<SettingsEditor>().Create() with
                        {
                            GeneralSettings = ExpectedState.Settings.GeneralSettings!
                        },
                        GeneralSettings = new DomainTestBuilder<GeneralSettings>().Create(),
                        IsSaving = false
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with
                    {
                        Editor = null,
                        IsSaving = false
                    }
                };
            }
        }
    }

    public class OnGeneralSettingsLoaded
    {
        private readonly OnGeneralSettingsLoadedFixture _fixture = new();

        [Fact]
        public void OnGeneralSettingsLoaded_ShouldUpdateCurrency()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = SettingsReducer.OnGeneralSettingsLoaded(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnGeneralSettingsLoadedFixture : SettingsReducerFixture
        {
            public GeneralSettingsLoadedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Settings = ExpectedState.Settings with
                    {
                        GeneralSettings = new DomainTestBuilder<GeneralSettings>().Create()
                    }
                };
            }

            public void SetupAction()
            {
                Action = new GeneralSettingsLoadedAction(ExpectedState.Settings.GeneralSettings!);
            }
        }
    }

    private abstract class SettingsReducerFixture
    {
        public SharedState ExpectedState { get; protected set; } = new DomainTestBuilder<SharedState>().Create();
        public SharedState InitialState { get; protected set; } = new DomainTestBuilder<SharedState>().Create();
    }
}
