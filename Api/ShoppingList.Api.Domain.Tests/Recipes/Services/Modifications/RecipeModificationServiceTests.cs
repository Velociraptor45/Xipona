using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Recipes.Services.Modifications;

public class RecipeModificationServiceTests
{
    public class ModifyAsync
    {
        private readonly ModifyAsyncFixture _fixture = new();

        [Fact]
        public async Task ModifyAsync_WithValidRecipeId_ShouldModifyRecipe()
        {
            // Arrange
            _fixture.SetupModification();
            _fixture.SetupRecipe();
            _fixture.SetupFindingRecipe();
            _fixture.SetupModifyingRecipe();
            _fixture.SetupStoringRecipe();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            await sut.ModifyAsync(_fixture.Modification);

            // Assert
            _fixture.VerifyModifyingRecipe();
        }

        [Fact]
        public async Task ModifyAsync_WithValidRecipeId_ShouldStoreRecipe()
        {
            // Arrange
            _fixture.SetupModification();
            _fixture.SetupRecipe();
            _fixture.SetupFindingRecipe();
            _fixture.SetupModifyingRecipe();
            _fixture.SetupStoringRecipe();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            await sut.ModifyAsync(_fixture.Modification);

            // Assert
            _fixture.VerifyStoringRecipe();
        }

        [Fact]
        public async Task ModifyAsync_WithInvalidRecipeId_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupModification();
            _fixture.SetupNotFindingRecipe();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Modification);

            // Act
            var func = async () => await sut.ModifyAsync(_fixture.Modification);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.RecipeNotFound);
        }

        private sealed class ModifyAsyncFixture : RecipeModificationServiceFixture
        {
            private RecipeMock? _recipeMock;

            public RecipeModification? Modification { get; private set; }

            public void SetupModification()
            {
                Modification = new DomainTestBuilder<RecipeModification>().Create();
            }

            public void SetupRecipe()
            {
                _recipeMock = new RecipeMock(MockBehavior.Strict);
            }

            public void SetupModifyingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMock);
                TestPropertyNotSetException.ThrowIfNull(Modification);

                _recipeMock.SetupModifyAsync(Modification, ValidatorMock.Object);
            }

            public void SetupFindingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);
                TestPropertyNotSetException.ThrowIfNull(_recipeMock);

                RecipeRepositoryMock.SetupFindByAsync(Modification.Id, _recipeMock.Object);
            }

            public void SetupNotFindingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(Modification);

                RecipeRepositoryMock.SetupFindByAsync(Modification.Id, null);
            }

            public void SetupStoringRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMock);

                RecipeRepositoryMock.SetupStoreAsync(_recipeMock.Object, _recipeMock.Object);
            }

            public void VerifyModifyingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMock);
                TestPropertyNotSetException.ThrowIfNull(Modification);

                _recipeMock.VerifyModifyAsync(Modification, ValidatorMock.Object, Times.Once);
            }

            public void VerifyStoringRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMock);
                RecipeRepositoryMock.VerifyStoreAsync(_recipeMock.Object, Times.Once);
            }
        }
    }

    public class RemoveDefaultItemAsync
    {
        private readonly RemoveDefaultItemAsyncFixture _fixture = new();

        [Fact]
        public async Task RemoveDefaultItemAsync_WithValidParameters_ShouldRemoveDefaultItemFromIngredients()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupFindingRecipes();
            _fixture.SetupModifyingRecipes();
            _fixture.SetupStoringRecipes();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

            // Act
            await sut.RemoveDefaultItemAsync(_fixture.ItemId.Value);

            // Assert
            _fixture.VerifyModifyingRecipes();
            _fixture.VerifyStoringRecipes();
        }

        private sealed class RemoveDefaultItemAsyncFixture : RecipeModificationServiceFixture
        {
            private IReadOnlyCollection<RecipeMock>? _recipeMocks;
            public ItemId? ItemId { get; private set; }

            public void SetupParameters()
            {
                ItemId = Domain.Items.Models.ItemId.New;
            }

            public void SetupModifyingRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMocks);
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                _recipeMocks.ToList().ForEach(m => m.SetupRemoveDefaultItem(ItemId.Value));
            }

            public void SetupFindingRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                _recipeMocks = new List<RecipeMock>
                {
                    new(MockBehavior.Strict),
                    new(MockBehavior.Strict)
                };
                RecipeRepositoryMock.SetupFindByAsync(ItemId.Value, _recipeMocks.Select(m => m.Object));
            }

            public void SetupStoringRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMocks);
                _recipeMocks.ToList().ForEach(m => RecipeRepositoryMock.SetupStoreAsync(m.Object, m.Object));
            }

            public void VerifyModifyingRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMocks);
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                _recipeMocks.ToList()
                    .ForEach(m => m.VerifyRemoveDefaultItem(ItemId.Value, Times.Once));
            }

            public void VerifyStoringRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMocks);
                _recipeMocks.ToList().ForEach(m => RecipeRepositoryMock.VerifyStoreAsync(m.Object, Times.Once));
            }
        }
    }

    public class ModifyIngredientsAfterItemUpdateAsync
    {
        private readonly ModifyIngredientsAfterItemUpdateAsyncFixture _fixture = new();

        [Fact]
        public async Task ModifyIngredientsAfterItemUpdateAsync_WithValidParameters_ShouldModifyIngredients()
        {
            // Arrange
            _fixture.SetupParameters();
            _fixture.SetupFindingRecipes();
            _fixture.SetupModifyingRecipes();
            _fixture.SetupStoringRecipes();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewItem);

            // Act
            await sut.ModifyIngredientsAfterItemUpdateAsync(_fixture.OldItemId.Value, _fixture.NewItem);

            // Assert
            _fixture.VerifyModifyingRecipes();
            _fixture.VerifyStoringRecipes();
        }

        private sealed class ModifyIngredientsAfterItemUpdateAsyncFixture : RecipeModificationServiceFixture
        {
            private IReadOnlyCollection<RecipeMock>? _recipeMocks;
            public ItemId? OldItemId { get; private set; }
            public IItem? NewItem { get; private set; }

            public void SetupParameters()
            {
                OldItemId = ItemId.New;
                NewItem = ItemMother.Initial().Create();
            }

            public void SetupModifyingRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMocks);
                TestPropertyNotSetException.ThrowIfNull(OldItemId);
                TestPropertyNotSetException.ThrowIfNull(NewItem);

                _recipeMocks.ToList().ForEach(m => m.SetupModifyIngredientsAfterItemUpdate(OldItemId.Value, NewItem));
            }

            public void SetupFindingRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(OldItemId);

                _recipeMocks = new List<RecipeMock>
                {
                    new(MockBehavior.Strict),
                    new(MockBehavior.Strict)
                };
                RecipeRepositoryMock.SetupFindByAsync(OldItemId.Value, _recipeMocks.Select(m => m.Object));
            }

            public void SetupStoringRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMocks);
                _recipeMocks.ToList().ForEach(m => RecipeRepositoryMock.SetupStoreAsync(m.Object, m.Object));
            }

            public void VerifyModifyingRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMocks);
                TestPropertyNotSetException.ThrowIfNull(OldItemId);
                TestPropertyNotSetException.ThrowIfNull(NewItem);

                _recipeMocks.ToList()
                    .ForEach(m => m.VerifyModifyIngredientsAfterItemUpdate(OldItemId.Value, NewItem, Times.Once));
            }

            public void VerifyStoringRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMocks);
                _recipeMocks.ToList().ForEach(m => RecipeRepositoryMock.VerifyStoreAsync(m.Object, Times.Once));
            }
        }
    }

    private abstract class RecipeModificationServiceFixture
    {
        protected readonly RecipeRepositoryMock RecipeRepositoryMock = new(MockBehavior.Strict);
        protected readonly ItemRepositoryMock ItemRepositoryMock = new(MockBehavior.Strict);
        protected readonly ValidatorMock ValidatorMock = new(MockBehavior.Strict);

        public RecipeModificationService CreateSut()
        {
            return new RecipeModificationService(
                _ => RecipeRepositoryMock.Object,
                _ => ItemRepositoryMock.Object,
                _ => ValidatorMock.Object,
                default);
        }
    }
}