using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Creation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.RecipeTags.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.RecipeTags.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.RecipeTags.Services.Creation;

public class RecipeTagCreationServiceTests
{
    public class CreateAsync
    {
        private readonly CreateAsyncFixture _fixture = new();

        [Fact]
        public async Task CreateAsync_WithValidName_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupName();
            _fixture.SetupCreatingRecipeTag();
            _fixture.SetupStoringRecipeTag();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Name);

            // Act
            var result = await sut.CreateAsync(_fixture.Name);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedRecipeTag);
        }

        private sealed class CreateAsyncFixture : RecipeTagCreationServiceFixture
        {
            private IRecipeTag? _createdRecipeTag;
            public RecipeTagName? Name { get; private set; }
            public IRecipeTag? ExpectedRecipeTag { get; private set; }

            public void SetupName()
            {
                Name = new DomainTestBuilder<RecipeTagName>().Create();
            }

            public void SetupStoringRecipeTag()
            {
                TestPropertyNotSetException.ThrowIfNull(Name);
                TestPropertyNotSetException.ThrowIfNull(_createdRecipeTag);

                ExpectedRecipeTag = new RecipeTagBuilder().WithName(Name).Create();
                RecipeTagRepositoryMock.SetupStoreAsync(_createdRecipeTag, ExpectedRecipeTag);
            }

            public void SetupCreatingRecipeTag()
            {
                TestPropertyNotSetException.ThrowIfNull(Name);
                _createdRecipeTag = new RecipeTagBuilder().WithName(Name).Create();
                RecipeTagFactoryMock.SetupCreateNew(Name, _createdRecipeTag);
            }
        }
    }

    private abstract class RecipeTagCreationServiceFixture
    {
        protected readonly RecipeTagFactoryMock RecipeTagFactoryMock = new(MockBehavior.Strict);
        protected readonly RecipeTagRepositoryMock RecipeTagRepositoryMock = new(MockBehavior.Strict);

        public RecipeTagCreationService CreateSut()
        {
            return new RecipeTagCreationService(
                RecipeTagFactoryMock.Object,
                RecipeTagRepositoryMock.Object);
        }
    }
}