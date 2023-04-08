using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Recipes.Reducers;

public class RecipeReducerTests
{
    public class OnEnterRecipeSearchPage
    {
        private readonly OnEnterRecipeSearchPageFixture _fixture;

        public OnEnterRecipeSearchPage()
        {
            _fixture = new OnEnterRecipeSearchPageFixture();
        }

        [Fact]
        public void OnEnterRecipeSearchPage_WithValidData_ShouldSetRecipeNull()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeReducer.OnEnterRecipeSearchPage(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnEnterRecipeSearchPageFixture : RecipeReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = new DomainTestBuilder<EditedRecipe>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }
        }
    }

    public class OnSearchRecipeFinished
    {
        private readonly OnSearchRecipeFinishedFixture _fixture;

        public OnSearchRecipeFinished()
        {
            _fixture = new OnSearchRecipeFinishedFixture();
        }

        [Fact]
        public void OnSearchRecipeFinished_WithValidData_ShouldSortResults()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeReducer.OnSearchRecipeFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        private sealed class OnSearchRecipeFinishedFixture : RecipeReducerFixture
        {
            public SearchRecipeFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        SearchResults = new DomainTestBuilder<RecipeSearchResult>().CreateMany(2).ToList()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        SearchResults = new List<RecipeSearchResult>
                        {
                            new DomainTestBuilder<RecipeSearchResult>()
                                .FillPropertyWith(r => r.Name, $"A{new DomainTestBuilder<string>().Create()}")
                                .Create(),
                            new DomainTestBuilder<RecipeSearchResult>()
                                .FillPropertyWith(r => r.Name, $"B{new DomainTestBuilder<string>().Create()}")
                                .Create(),
                            new DomainTestBuilder<RecipeSearchResult>()
                                .FillPropertyWith(r => r.Name, $"Z{new DomainTestBuilder<string>().Create()}")
                                .Create()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SearchRecipeFinishedAction(ExpectedState.Search.SearchResults.Reverse().ToList());
            }
        }
    }

    public class OnLoadRecipeTagsFinished
    {
        private readonly OnLoadRecipeTagsFinishedFixture _fixture;

        public OnLoadRecipeTagsFinished()
        {
            _fixture = new OnLoadRecipeTagsFinishedFixture();
        }

        [Fact]
        public void OnLoadRecipeTagsFinished_WithValidData_ShouldSortResults()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeReducer.OnLoadRecipeTagsFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        private sealed class OnLoadRecipeTagsFinishedFixture : RecipeReducerFixture
        {
            public LoadRecipeTagsFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    RecipeTags = new DomainTestBuilder<RecipeTag>().CreateMany(2).ToList()
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    RecipeTags = new List<RecipeTag>
                    {
                        new DomainTestBuilder<RecipeTag>().Create() with
                        {
                            Name = $"A{new DomainTestBuilder<string>().Create()}"
                        },
                        new DomainTestBuilder<RecipeTag>().Create() with
                        {
                            Name = $"B{new DomainTestBuilder<string>().Create()}"
                        },
                        new DomainTestBuilder<RecipeTag>().Create() with
                        {
                            Name = $"Z{new DomainTestBuilder<string>().Create()}"
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new LoadRecipeTagsFinishedAction(ExpectedState.RecipeTags.Reverse().ToList());
            }
        }
    }

    private abstract class RecipeReducerFixture
    {
        public RecipeState ExpectedState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
        public RecipeState InitialState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
    }
}