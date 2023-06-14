using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
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
                    modification.Quantity, modification.ShoppingListProperties, createdIngredient);
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

    public class ModifyAfterItemUpdate
    {
        private readonly ModifyAfterItemUpdateFixture _fixture = new();

        [Fact]
        public void ModifyAfterItemUpdate_WithOldItemReference_ShouldModifyIngredient()
        {
            // Arrange
            _fixture.SetupTwoItems();
            _fixture.SetupMatchingNewItem();
            _fixture.SetupChangingDefaultItem();
            _fixture.SetupExpectedIngredientsForMatchingIngredient();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedIngredients);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);

            // Act
            sut.ModifyAfterItemUpdate(_fixture.OldItemId.Value, _fixture.NewItem);

            // Assert
            sut.Should().BeEquivalentTo(_fixture.ExpectedIngredients);
        }

        [Fact]
        public void ModifyAfterItemUpdate_WithNoOldItemReference_ShouldNoModifyIngredient()
        {
            // Arrange
            _fixture.SetupTwoItems();
            _fixture.SetupNotMatchingNewItem();
            _fixture.SetupExpectedIngredientsForNotMatchingIngredient();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedIngredients);
            TestPropertyNotSetException.ThrowIfNull(_fixture.OldItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);

            // Act
            sut.ModifyAfterItemUpdate(_fixture.OldItemId.Value, _fixture.NewItem);

            // Assert
            sut.Should().BeEquivalentTo(_fixture.ExpectedIngredients);
        }

        private sealed class ModifyAfterItemUpdateFixture
        {
            private Ingredient? _modifiedIngredient;
            private IReadOnlyCollection<IngredientMock>? _ingredientMocks;
            public ItemId? OldItemId { get; private set; }
            public IItem? NewItem { get; private set; }
            public IReadOnlyCollection<IIngredient>? ExpectedIngredients { get; private set; }

            public void SetupTwoItems()
            {
                _ingredientMocks = new List<IngredientMock>
                {
                    new(new IngredientBuilder().Create(), MockBehavior.Strict),
                    new(new IngredientBuilder().Create(), MockBehavior.Strict)
                };
            }

            public void SetupNotMatchingNewItem()
            {
                OldItemId = ItemId.New;
                NewItem = ItemMother.Initial().WithPredecessorId(OldItemId).Create();
            }

            public void SetupMatchingNewItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredientMocks);

                OldItemId = _ingredientMocks.Last().Object.DefaultItemId!.Value;
                NewItem = ItemMother.Initial().WithPredecessorId(OldItemId).Create();
            }

            public void SetupChangingDefaultItem()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredientMocks);
                TestPropertyNotSetException.ThrowIfNull(OldItemId);
                TestPropertyNotSetException.ThrowIfNull(NewItem);

                _modifiedIngredient = new IngredientBuilder().Create();
                _ingredientMocks.Last().SetupChangingDefaultItem(OldItemId.Value, NewItem, _modifiedIngredient);
            }

            public void SetupExpectedIngredientsForMatchingIngredient()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredientMocks);
                TestPropertyNotSetException.ThrowIfNull(_modifiedIngredient);

                ExpectedIngredients = new List<IIngredient>
                {
                    _ingredientMocks.First().Object,
                    _modifiedIngredient
                };
            }

            public void SetupExpectedIngredientsForNotMatchingIngredient()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredientMocks);

                ExpectedIngredients = _ingredientMocks.Select(m => m.Object).ToList();
            }

            public Ingredients CreateSut()
            {
                TestPropertyNotSetException.ThrowIfNull(_ingredientMocks);

                return new Ingredients(_ingredientMocks.Select(m => m.Object),
                    new IngredientFactoryMock(MockBehavior.Strict).Object);
            }
        }
    }
}