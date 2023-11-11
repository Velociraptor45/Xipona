using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Models.Factories;

public class RecipeFactoryTests
{
    public class Create
    {
        private readonly CreateFixture _fixture;

        public Create()
        {
            _fixture = new CreateFixture();
        }

        [Fact]
        public void Create_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = sut.Create(
                _fixture.ExpectedResult.Id,
                _fixture.ExpectedResult.Name,
                _fixture.ExpectedResult.NumberOfServings,
                _fixture.ExpectedResult.Ingredients,
                _fixture.ExpectedResult.PreparationSteps,
                _fixture.ExpectedResult.Tags);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class CreateFixture : RecipeFactoryFixture
        {
            public IRecipe? ExpectedResult { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = new RecipeBuilder().Create();
            }
        }
    }

    public class CreateNew
    {
        private readonly CreateNewFixture _fixture;

        public CreateNew()
        {
            _fixture = new CreateNewFixture();
        }

        [Fact]
        public async Task CreateNew_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupCreation();
            _fixture.SetupIngredientCreationSuccessful();
            _fixture.SetupPreparationStepCreation();
            _fixture.SetupTagValidationSuccess();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Creation);

            // Act
            var result = await sut.CreateNewAsync(_fixture.Creation);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(i => i.Id));
            _fixture.VerifyTagValidation();
        }

        [Fact]
        public async Task CreateNew_WithIngredientCreationFailed_ShouldThrow()
        {
            // Arrange
            _fixture.SetupExpectedResult();
            _fixture.SetupCreation();
            _fixture.SetupIngredientCreationFailed();
            _fixture.SetupPreparationStepCreation();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Creation);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedException);

            // Act
            var func = async () => await sut.CreateNewAsync(_fixture.Creation);

            // Assert
            (await func.Should().ThrowExactlyAsync<DomainException>()).Where(e => e == _fixture.ExpectedException);
        }

        private sealed class CreateNewFixture : RecipeFactoryFixture
        {
            public IRecipe? ExpectedResult { get; private set; }
            public DomainException? ExpectedException { get; private set; }
            public RecipeCreation? Creation { get; private set; }

            public void SetupExpectedResult()
            {
                ExpectedResult = new RecipeBuilder().Create();
            }

            public void SetupCreation()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                Creation = new RecipeCreation(
                    ExpectedResult.Name,
                    ExpectedResult.NumberOfServings,
                    ExpectedResult.Ingredients.Select(i => new IngredientCreation(
                        i.ItemCategoryId,
                        i.QuantityType,
                        i.Quantity,
                        i.ShoppingListProperties)),
                    ExpectedResult.PreparationSteps.Select(p => new PreparationStepCreation(
                        p.Instruction,
                        p.SortingIndex)),
                    ExpectedResult.Tags);
            }

            public void SetupPreparationStepCreation()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                TestPropertyNotSetException.ThrowIfNull(Creation);

                var expectedStep = ExpectedResult.PreparationSteps.ToList();
                var creations = Creation.PreparationStepCreations.ToList();
                for (var i = 0; i < Creation.PreparationStepCreations.Count; i++)
                {
                    PreparationStepFactoryMock.SetupCreateNew(creations[i], expectedStep[i]);
                }
            }

            public void SetupIngredientCreationSuccessful()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);
                TestPropertyNotSetException.ThrowIfNull(Creation);

                var expectedIngredients = ExpectedResult.Ingredients.ToList();
                var creations = Creation.IngredientCreations.ToList();
                for (var i = 0; i < Creation.IngredientCreations.Count; i++)
                {
                    IngredientFactoryMock.SetupCreateNewAsync(creations[i], expectedIngredients[i]);
                }
            }

            public void SetupTagValidationSuccess()
            {
                TestPropertyNotSetException.ThrowIfNull(Creation);

                ValidatorMock.SetupValidateAsync(Creation.RecipeTagIds);
            }

            public void SetupIngredientCreationFailed()
            {
                TestPropertyNotSetException.ThrowIfNull(Creation);

                var creations = Creation.IngredientCreations.ToList();
                ExpectedException = new DomainException(new ItemCategoryNotFoundReason(ItemCategoryId.New));

                for (var i = 0; i < Creation.IngredientCreations.Count; i++)
                {
                    IngredientFactoryMock.SetupCreateNewAsync(creations[i]).ThrowsAsync(ExpectedException);
                }
            }

            public void VerifyTagValidation()
            {
                TestPropertyNotSetException.ThrowIfNull(Creation);
                ValidatorMock.VerifyValidateAsync(Creation.RecipeTagIds, Times.Once);
            }
        }
    }

    private abstract class RecipeFactoryFixture
    {
        protected readonly IngredientFactoryMock IngredientFactoryMock = new(MockBehavior.Strict);
        protected readonly PreparationStepFactoryMock PreparationStepFactoryMock = new(MockBehavior.Strict);
        protected readonly ValidatorMock ValidatorMock = new(MockBehavior.Strict);

        public RecipeFactory CreateSut()
        {
            return new RecipeFactory(IngredientFactoryMock.Object, ValidatorMock.Object,
                PreparationStepFactoryMock.Object);
        }
    }
}