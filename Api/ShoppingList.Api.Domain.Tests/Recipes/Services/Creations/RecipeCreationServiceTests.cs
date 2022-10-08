using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using Xunit.Abstractions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Services.Creations;

public class RecipeCreationServiceTests
{
    private readonly RecipeCreationServiceFixture _fixture;

    public RecipeCreationServiceTests(ITestOutputHelper output)
    {
        _fixture = new RecipeCreationServiceFixture(output);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldStoreNewRecipe()
    {
        // Arrange
        _fixture.SetupCreation();
        _fixture.SetupCreatingNewRecipe();
        _fixture.SetupStoringRecipe();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Creation);
        TestPropertyNotSetException.ThrowIfNull(_fixture.StoredRecipe);

        // Act
        await sut.CreateAsync(_fixture.Creation);

        // Assert
        _fixture.VerifyStoringRecipe();
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldReturnStoredRecipe()
    {
        // Arrange
        _fixture.SetupCreation();
        _fixture.SetupCreatingNewRecipe();
        _fixture.SetupStoringRecipe();
        var sut = _fixture.CreateSut();

        TestPropertyNotSetException.ThrowIfNull(_fixture.Creation);
        TestPropertyNotSetException.ThrowIfNull(_fixture.StoredRecipe);

        // Act
        var result = await sut.CreateAsync(_fixture.Creation);

        // Assert
        result.Should().Be(_fixture.StoredRecipe);
    }

    private class RecipeCreationServiceFixture
    {
        private readonly RecipeRepositoryMock _recipeRepository = new(MockBehavior.Strict);
        private readonly RecipeFactoryMock _recipeFactoryMock = new(MockBehavior.Strict);
        private Recipe? _createdRecipe;
        private readonly ILogger<RecipeCreationService> _logger;

        public RecipeCreationServiceFixture(ITestOutputHelper output)
        {
            _logger = output.BuildLoggerFor<RecipeCreationService>();
        }

        public RecipeCreation? Creation { get; private set; }
        public Recipe? StoredRecipe { get; private set; }

        public RecipeCreationService CreateSut()
        {
            return new RecipeCreationService(
                _ => _recipeRepository.Object,
                _ => _recipeFactoryMock.Object,
                _logger,
                default);
        }

        public void SetupCreation()
        {
            Creation = new DomainTestBuilder<RecipeCreation>().Create();
        }

        public void SetupCreatingNewRecipe()
        {
            TestPropertyNotSetException.ThrowIfNull(Creation);

            _createdRecipe = new RecipeBuilder().Create();
            _recipeFactoryMock.SetupCreateNewAsync(Creation, _createdRecipe);
        }

        public void SetupStoringRecipe()
        {
            TestPropertyNotSetException.ThrowIfNull(_createdRecipe);

            StoredRecipe = new RecipeBuilder().Create();
            _recipeRepository.SetupStoreAsync(_createdRecipe, StoredRecipe);
        }

        public void VerifyStoringRecipe()
        {
            TestPropertyNotSetException.ThrowIfNull(_createdRecipe);

            _recipeRepository.VerifyStoreAsync(_createdRecipe, Times.Once);
        }
    }
}