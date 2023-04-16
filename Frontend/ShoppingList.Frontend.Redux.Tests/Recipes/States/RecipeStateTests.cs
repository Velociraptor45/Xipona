using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Recipes.States;

public class RecipeStateTests
{
    public class GetTagNamesFor
    {
        private readonly GetTagNamesForFixture _fixture = new();

        [Fact]
        public void GetTagNamesFor_WithRecipeTagsIsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            _fixture.SetupRecipeTags();
            _fixture.SetupRecipeTagIdsEmpty();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.RecipeTagIds);

            // Act
            var result = sut.GetTagNamesFor(_fixture.RecipeTagIds);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetTagNamesFor_WithRecipeTagIdsNotPartOfTags_ShouldReturnEmptyList()
        {
            // Arrange
            _fixture.SetupRecipeTags();
            _fixture.SetupRecipeTagIdsNotPartOfTags();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.RecipeTagIds);

            // Act
            var result = sut.GetTagNamesFor(_fixture.RecipeTagIds);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetTagNamesFor_WithRecipeTagIdsPartOfTags_ShouldReturnTagNames()
        {
            // Arrange
            _fixture.SetupRecipeTags();
            _fixture.SetupRecipeTagIdsPartOfTags();
            _fixture.SetupExpectedResult();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.RecipeTagIds);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.GetTagNamesFor(_fixture.RecipeTagIds);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class GetTagNamesForFixture
        {
            private List<RecipeTag>? _tags;
            public List<Guid>? RecipeTagIds { get; private set; }
            public List<string>? ExpectedResult { get; private set; }

            public void SetupRecipeTags()
            {
                _tags = new DomainTestBuilder<RecipeTag>().CreateMany(4).ToList();
            }

            public void SetupRecipeTagIdsPartOfTags()
            {
                TestPropertyNotSetException.ThrowIfNull(_tags);
                RecipeTagIds = new List<Guid>
                {
                    _tags[0].Id,
                    _tags[2].Id
                };
            }

            public void SetupRecipeTagIdsNotPartOfTags()
            {
                TestPropertyNotSetException.ThrowIfNull(_tags);
                RecipeTagIds = new List<Guid>
                {
                    Guid.NewGuid(),
                    Guid.NewGuid()
                };
            }

            public void SetupRecipeTagIdsEmpty()
            {
                RecipeTagIds = new List<Guid>
                {
                    Guid.NewGuid(),
                    Guid.NewGuid()
                };
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(_tags);
                ExpectedResult = new List<string>
                {
                    _tags[0].Name,
                    _tags[2].Name
                };
            }

            public RecipeState CreateSut()
            {
                var state = new DomainTestBuilder<RecipeState>().Create();

                if (_tags is not null)
                {
                    state = state with
                    {
                        RecipeTags = _tags
                    };
                }

                return state;
            }
        }
    }
}