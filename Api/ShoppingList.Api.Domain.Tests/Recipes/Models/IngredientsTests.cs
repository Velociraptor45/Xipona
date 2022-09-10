using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Models;

public class IngredientsTests
{
    public class ModifyManyAsync
    {
        private readonly ModifyManyAsyncFixture _fixture;

        public ModifyManyAsync()
        {
            _fixture = new ModifyManyAsyncFixture();
        }

        [Fact]
        public async Task ModifyManyAsync_WithInvalidIngredientId_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupIngredientToModifyWithInvalidId();
            var sut = _fixture.CreateSut();

            // Act
            var func = async () =>
                await sut.ModifyManyAsync(_fixture.IngredientModifications, _fixture.ValidatorMock.Object);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.IngredientNotFound);
        }

        [Fact]
        public async Task ModifyManyAsync_WithIngredientToModify_ShouldModifyIngredient()
        {
            // Arrange
            _fixture.SetupIngredientToModify();
            var sut = _fixture.CreateSut();

            // Act
            await sut.ModifyManyAsync(_fixture.IngredientModifications, _fixture.ValidatorMock.Object);

            // Assert
            _fixture.VerifyModifyingIngredientToModify();
        }

        [Fact]
        public async Task ModifyManyAsync_WithIngredientToModify_ShouldModifyItselfCorrectly()
        {
            // Arrange
            _fixture.SetupIngredientToModify();
            var sut = _fixture.CreateSut();

            // Act
            await sut.ModifyManyAsync(_fixture.IngredientModifications, _fixture.ValidatorMock.Object);

            // Assert
            sut.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public async Task ModifyManyAsync_WithIngredientToCreate_ShouldModifyItselfCorrectly()
        {
            // Arrange
            _fixture.SetupIngredientToCreate();
            var sut = _fixture.CreateSut();

            // Act
            await sut.ModifyManyAsync(_fixture.IngredientModifications, _fixture.ValidatorMock.Object);

            // Assert
            sut.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public async Task ModifyManyAsync_WithIngredientToDelete_ShouldModifyItselfCorrectly()
        {
            // Arrange
            _fixture.SetupIngredientToDelete();
            var sut = _fixture.CreateSut();

            // Act
            await sut.ModifyManyAsync(_fixture.IngredientModifications, _fixture.ValidatorMock.Object);

            // Assert
            sut.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class ModifyManyAsyncFixture
        {
            private readonly IngredientFactoryMock _ingredientFactoryMock = new(MockBehavior.Strict);
            private readonly List<IIngredient> _ingredients = new();
            private readonly List<IIngredient> _expectedResult = new();
            private readonly List<IngredientModification> _modifications = new();

            private IngredientMock? _ingredientMockToModify;
            private IngredientModification? _modificationForIngredientMockToModify;

            public ValidatorMock ValidatorMock { get; } = new(MockBehavior.Strict);
            public IReadOnlyCollection<IngredientModification> IngredientModifications => _modifications;
            public IReadOnlyCollection<IIngredient> ExpectedResult => _expectedResult;

            public Ingredients CreateSut()
            {
                return new Ingredients(_ingredients, _ingredientFactoryMock.Object);
            }

            public void SetupIngredientToModifyWithInvalidId()
            {
                _ingredientMockToModify = new(new IngredientBuilder().Create(), MockBehavior.Strict);
                _ingredients.Add(_ingredientMockToModify.Object);

                _modificationForIngredientMockToModify = new DomainTestBuilder<IngredientModification>()
                    .Create();
                _modifications.Add(_modificationForIngredientMockToModify);
            }

            public void SetupIngredientToModify()
            {
                _ingredientMockToModify = new(new IngredientBuilder().Create(), MockBehavior.Strict);
                var ingredientId = _ingredientMockToModify.Object.Id;
                _ingredients.Add(_ingredientMockToModify.Object);

                _modificationForIngredientMockToModify = new DomainTestBuilder<IngredientModification>()
                    .FillConstructorWith("id", (IngredientId?)ingredientId)
                    .Create();
                _modifications.Add(_modificationForIngredientMockToModify);

                var modifiedIngredient = new IngredientBuilder().WithId(ingredientId).Create();
                _expectedResult.Add(modifiedIngredient);

                _ingredientMockToModify.SetupModifyAsync(_modificationForIngredientMockToModify, ValidatorMock.Object,
                    modifiedIngredient);
            }

            public void SetupIngredientToCreate()
            {
                var modification = new DomainTestBuilder<IngredientModification>()
                    .FillConstructorWith("id", (IngredientId?)null)
                    .Create();
                _modifications.Add(modification);

                var createdIngredient = new IngredientBuilder().Create();
                _expectedResult.Add(createdIngredient);

                _ingredientFactoryMock.SetupCreateNewAsync(modification.ItemCategoryId, modification.QuantityType,
                    modification.Quantity, createdIngredient);
            }

            public void SetupIngredientToDelete()
            {
                _ingredients.Add(new IngredientBuilder().Create());
            }

            public void VerifyModifyingIngredientToModify()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredientMockToModify);
                TestPropertyNotSetException.ThrowIfNull(_modificationForIngredientMockToModify);

                _ingredientMockToModify.VerifyModifyAsync(_modificationForIngredientMockToModify, ValidatorMock.Object,
                    Times.Once);
            }
        }
    }
}