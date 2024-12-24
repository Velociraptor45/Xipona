using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.SideDishes;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Recipes.Reducers;
public class SideDishSelectorReducerTests
{
    public class OnSideDishInputChanged
    {
        private readonly OnSideDishInputChangedFixture _fixture = new();

        [Fact]
        public void OnSideDishInputChanged_WithRecipeNull_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = SideDishSelectorReducer.OnSideDishInputChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSideDishInputChanged_WithValidInput_ShouldChangeInput()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = SideDishSelectorReducer.OnSideDishInputChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSideDishInputChangedFixture : SideDishSelectorReducerFixture
        {
            public SideDishInputChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        SideDishSelector = ExpectedState.Editor.SideDishSelector with
                        {
                            Input = new DomainTestBuilder<string>().Create()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SideDishInputChangedAction(ExpectedState.Editor.SideDishSelector.Input);
            }

        }
    }

    public class OnSideDishDropdownClosed
    {
        private readonly OnSideDishDropdownClosedFixture _fixture = new();

        [Fact]
        public void OnSideDishDropdownClosed_WithRecipeNull_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();

            // Act
            var result = SideDishSelectorReducer.OnSideDishDropdownClosed(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSideDishDropdownClosed_WithValidRecipe_ShouldResetInput()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupExpectedState();

            // Act
            var result = SideDishSelectorReducer.OnSideDishDropdownClosed(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSideDishDropdownClosedFixture : SideDishSelectorReducerFixture
        {
            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        SideDishSelector = ExpectedState.Editor.SideDishSelector with
                        {
                            Input = string.Empty
                        }
                    }
                };
            }
        }
    }

    public class OnSideDishChanged
    {
        private readonly OnSideDishChangedFixture _fixture = new();

        [Fact]
        public void OnSideDishChanged_WithRecipeNull_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = SideDishSelectorReducer.OnSideDishChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSideDishChanged_WithValidRecipe_ShouldChangeSelectedSideDish()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = SideDishSelectorReducer.OnSideDishChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSideDishChangedFixture : SideDishSelectorReducerFixture
        {
            public SideDishChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        SideDishSelector = ExpectedState.Editor.SideDishSelector with
                        {
                            SideDishes = [ExpectedState.Editor.Recipe!.SideDish!],
                            Input = string.Empty
                        }
                    }
                };
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            SideDish = new DomainTestBuilder<SideDish>().Create()
                        },
                        SideDishSelector = ExpectedState.Editor.SideDishSelector with
                        {
                            SideDishes = new DomainTestBuilder<SideDish>().CreateMany(2).ToList(),
                            Input = new DomainTestBuilder<string>().Create()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SideDishChangedAction(ExpectedState.Editor.Recipe!.SideDish!);
            }

            public void SetupActionForRecipeNull()
            {
                Action = new SideDishChangedAction(new DomainTestBuilder<SideDish>().Create());
            }
        }
    }

    public class OnSideDishCleared
    {
        private readonly OnSideDishClearedFixture _fixture = new();

        [Fact]
        public void OnSideDishCleared_WithRecipeNull_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupRecipeNull();
            _fixture.SetupInitialStateEqualsExpectedState();

            // Act
            var result = SideDishSelectorReducer.OnSideDishCleared(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSideDishCleared_WithValidRecipe_ShouldRemoveSideDish()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = SideDishSelectorReducer.OnSideDishCleared(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSideDishClearedFixture : SideDishSelectorReducerFixture
        {
            public SideDishClearedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            SideDish = null
                        },
                        SideDishSelector = ExpectedState.Editor.SideDishSelector with
                        {
                            SideDishes = [],
                            Input = string.Empty
                        }
                    }
                };
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            SideDish = new DomainTestBuilder<SideDish>().Create()
                        },
                        SideDishSelector = ExpectedState.Editor.SideDishSelector with
                        {
                            SideDishes = new DomainTestBuilder<SideDish>().CreateMany(2).ToList(),
                            Input = new DomainTestBuilder<string>().Create()
                        }
                    }
                };
            }
        }
    }

    public class OnSearchSideDishesFinished
    {
        private readonly OnSearchSideDishesFinishedFixture _fixture = new();

        [Fact]
        public void OnSearchSideDishesFinished_WithSelectedSideDishInResults_ShouldUpdateAvailableSideDishes()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupActionWithSelectedDishInResults();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = SideDishSelectorReducer.OnSearchSideDishesFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void OnSearchSideDishesFinished_WithSelectedSideDishNotInResults_ShouldUpdateAvailableSideDishes()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupActionWithSelectedDishNotInResults();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = SideDishSelectorReducer.OnSearchSideDishesFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        private sealed class OnSearchSideDishesFinishedFixture : SideDishSelectorReducerFixture
        {
            public SearchSideDishesFinishedAction? Action { get; private set; }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        SideDishSelector = ExpectedState.Editor.SideDishSelector with
                        {
                            SideDishes = [
                                ExpectedState.Editor.Recipe!.SideDish!,
                                new SideDish(Guid.NewGuid(), "A"),
                                new SideDish(Guid.NewGuid(), "B"),
                                new SideDish(Guid.NewGuid(), "C"),
                                ]
                        }
                    }
                };
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        SideDishSelector = ExpectedState.Editor.SideDishSelector with
                        {
                            SideDishes = new DomainTestBuilder<SideDish>().CreateMany(2).ToList(),
                        }
                    }
                };
            }

            public void SetupActionWithSelectedDishInResults()
            {
                var results = ExpectedState.Editor.SideDishSelector.SideDishes
                    .Select(s => new RecipeSearchResult(s.Id, s.Name))
                    .ToArray();
                Random.Shared.Shuffle(results);

                Action = new SearchSideDishesFinishedAction(results);
            }

            public void SetupActionWithSelectedDishNotInResults()
            {
                var results = ExpectedState.Editor.SideDishSelector.SideDishes
                    .Skip(1)
                    .Select(s => new RecipeSearchResult(s.Id, s.Name))
                    .ToArray();
                Random.Shared.Shuffle(results);

                Action = new SearchSideDishesFinishedAction(results);
            }
        }
    }

    private abstract class SideDishSelectorReducerFixture
    {
        public RecipeState ExpectedState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
        public RecipeState InitialState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
    }
}
