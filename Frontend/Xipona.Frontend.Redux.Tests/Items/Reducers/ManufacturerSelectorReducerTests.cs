using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
using ProjectHermes.Xipona.Frontend.Redux.Items.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Items.Reducers;

public class ManufacturerSelectorReducerTests
{
    public class OnLoadInitialManufacturerFinished
    {
        private readonly OnLoadInitialManufacturerFinishedFixture _fixture = new();

        [Fact]
        public void OnLoadInitialManufacturerFinished_WithValidData_ShouldSetManufacturer()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ManufacturerSelectorReducer.OnLoadInitialManufacturerFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnLoadInitialManufacturerFinishedFixture : ManufacturerSelectorReducerFixture
        {
            public LoadInitialManufacturerFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ManufacturerSelector = ExpectedState.Editor.ManufacturerSelector with
                        {
                            Manufacturers = new DomainTestBuilder<ManufacturerSearchResult>().CreateMany(2).ToList()
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ManufacturerSelector = ExpectedState.Editor.ManufacturerSelector with
                        {
                            Manufacturers = new DomainTestBuilder<ManufacturerSearchResult>().CreateMany(1).ToList()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new LoadInitialManufacturerFinishedAction(
                    ExpectedState.Editor.ManufacturerSelector.Manufacturers.First());
            }
        }
    }

    public class OnCreateNewManufacturerFinished
    {
        private readonly OnCreateNewManufacturerFinishedFixture _fixture = new();

        [Fact]
        public void OnCreateNewManufacturerFinished_WithValidData_ShouldSetManufacturerAndManufacturerId()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ManufacturerSelectorReducer.OnCreateNewManufacturerFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnCreateNewManufacturerFinishedFixture : ManufacturerSelectorReducerFixture
        {
            public CreateNewManufacturerFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ManufacturerId = Guid.NewGuid()
                        },
                        ManufacturerSelector = ExpectedState.Editor.ManufacturerSelector with
                        {
                            Manufacturers = new DomainTestBuilder<ManufacturerSearchResult>().CreateMany(2).ToList()
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                var manufacturers = new DomainTestBuilder<ManufacturerSearchResult>().CreateMany(1).ToList();

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ManufacturerId = manufacturers.First().Id
                        },
                        ManufacturerSelector = ExpectedState.Editor.ManufacturerSelector with
                        {
                            Manufacturers = manufacturers,
                            Input = string.Empty
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new CreateNewManufacturerFinishedAction(
                    ExpectedState.Editor.ManufacturerSelector.Manufacturers.First());
            }
        }
    }

    public class OnManufacturerInputChanged
    {
        private readonly OnManufacturerInputChangedFixture _fixture = new();

        [Fact]
        public void OnManufacturerInputChanged_WithValidInput_ShouldSetInput()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ManufacturerSelectorReducer.OnManufacturerInputChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnManufacturerInputChangedFixture : ManufacturerSelectorReducerFixture
        {
            public ManufacturerInputChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ManufacturerSelector = ExpectedState.Editor.ManufacturerSelector with
                        {
                            Input = new DomainTestBuilder<string>().Create()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new ManufacturerInputChangedAction(ExpectedState.Editor.ManufacturerSelector.Input);
            }
        }
    }

    public class OnSearchManufacturerFinished
    {
        private readonly OnSearchManufacturerFinishedFixture _fixture = new();

        [Fact]
        public void OnSearchManufacturerFinished_WithManufacturerSelected_ShouldSetManufacturersAndKeepSelected()
        {
            // Arrange
            _fixture.SetupExpectedStateWithSelectedManufacturer();
            _fixture.SetupInitialState();
            _fixture.SetupActionWithSelectedManufacturer();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ManufacturerSelectorReducer.OnSearchManufacturerFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void OnSearchManufacturerFinished_WithNoManufacturerSelected_ShouldSetManufacturers()
        {
            // Arrange
            _fixture.SetupExpectedStateWithoutSelectedManufacturer();
            _fixture.SetupInitialState();
            _fixture.SetupActionWithoutSelectedManufacturer();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ManufacturerSelectorReducer.OnSearchManufacturerFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnSearchManufacturerFinishedFixture : ManufacturerSelectorReducerFixture
        {
            public SearchManufacturerFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ManufacturerSelector = ExpectedState.Editor.ManufacturerSelector with
                        {
                            Manufacturers = new List<ManufacturerSearchResult>
                            {
                                new DomainTestBuilder<ManufacturerSearchResult>().Create(),
                                ExpectedState.Editor.ManufacturerSelector.Manufacturers.First()
                            }
                        }
                    }
                };
            }

            public void SetupExpectedStateWithSelectedManufacturer()
            {
                var manufacturer = ExpectedState.Editor.ManufacturerSelector.Manufacturers.Last();

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ManufacturerId = manufacturer.Id
                        },
                        ManufacturerSelector = ExpectedState.Editor.ManufacturerSelector with
                        {
                            Manufacturers = new List<ManufacturerSearchResult>
                            {
                                manufacturer,
                                new DomainTestBuilder<ManufacturerSearchResult>()
                                    .FillPropertyWith(r => r.Name, $"A{new DomainTestBuilder<string>().Create()}")
                                    .Create(),
                                new DomainTestBuilder<ManufacturerSearchResult>()
                                    .FillPropertyWith(r => r.Name, $"B{new DomainTestBuilder<string>().Create()}")
                                    .Create(),
                                new DomainTestBuilder<ManufacturerSearchResult>()
                                    .FillPropertyWith(r => r.Name, $"Z{new DomainTestBuilder<string>().Create()}")
                                    .Create()
                            }
                        }
                    }
                };
            }

            public void SetupExpectedStateWithoutSelectedManufacturer()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ManufacturerId = null
                        },
                        ManufacturerSelector = ExpectedState.Editor.ManufacturerSelector with
                        {
                            Manufacturers = new List<ManufacturerSearchResult>
                            {
                                new DomainTestBuilder<ManufacturerSearchResult>()
                                    .FillPropertyWith(r => r.Name, $"A{new DomainTestBuilder<string>().Create()}")
                                    .Create(),
                                new DomainTestBuilder<ManufacturerSearchResult>()
                                    .FillPropertyWith(r => r.Name, $"B{new DomainTestBuilder<string>().Create()}")
                                    .Create(),
                                new DomainTestBuilder<ManufacturerSearchResult>()
                                    .FillPropertyWith(r => r.Name, $"Z{new DomainTestBuilder<string>().Create()}")
                                    .Create()
                            }
                        }
                    }
                };
            }

            public void SetupActionWithSelectedManufacturer()
            {
                Action = new SearchManufacturerFinishedAction(
                    ExpectedState.Editor.ManufacturerSelector.Manufacturers.Skip(1).Reverse().ToList());
            }

            public void SetupActionWithoutSelectedManufacturer()
            {
                Action = new SearchManufacturerFinishedAction(
                    ExpectedState.Editor.ManufacturerSelector.Manufacturers.Reverse().ToList());
            }
        }
    }

    public class OnSelectedManufacturerChanged
    {
        private readonly OnSelectedManufacturerChangedFixture _fixture = new();

        [Fact]
        public void OnSelectedManufacturerChanged_WithValidData_ShouldSetManufacturerId()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ManufacturerSelectorReducer.OnSelectedManufacturerChanged(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnSelectedManufacturerChangedFixture : ManufacturerSelectorReducerFixture
        {
            public SelectedManufacturerChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ManufacturerId = Guid.NewGuid()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SelectedManufacturerChangedAction(ExpectedState.Editor.Item!.ManufacturerId!.Value);
            }
        }
    }

    public class OnClearManufacturer
    {
        private readonly OnClearManufacturerFixture _fixture = new();

        [Fact]
        public void OnClearManufacturer_WithValidData_ShouldSetManufacturerId()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ManufacturerSelectorReducer.OnClearManufacturer(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnClearManufacturerFixture : ManufacturerSelectorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ManufacturerId = Guid.NewGuid()
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ManufacturerId = null
                        }
                    }
                };
            }
        }
    }

    private abstract class ManufacturerSelectorReducerFixture
    {
        public ItemState ExpectedState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
        public ItemState InitialState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
    }
}