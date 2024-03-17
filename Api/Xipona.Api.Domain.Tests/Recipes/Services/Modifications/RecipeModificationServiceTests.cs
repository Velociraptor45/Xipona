using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Ports;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Recipes.Services.Modifications;

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
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

            // Act
            await sut.RemoveDefaultItemAsync(_fixture.ItemId.Value, _fixture.ItemTypeId);

            // Assert
            _fixture.VerifyModifyingRecipes();
            _fixture.VerifyStoringRecipes();
        }

        private sealed class RemoveDefaultItemAsyncFixture : RecipeModificationServiceFixture
        {
            private IReadOnlyCollection<RecipeMock>? _recipeMocks;
            public ItemId? ItemId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }

            public void SetupParameters()
            {
                ItemId = Domain.Items.Models.ItemId.New;
                ItemTypeId = Domain.Items.Models.ItemTypeId.New;
            }

            public void SetupModifyingRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMocks);
                TestPropertyNotSetException.ThrowIfNull(ItemId);
                TestPropertyNotSetException.ThrowIfNull(ItemTypeId);

                _recipeMocks.ToList().ForEach(m => m.SetupRemoveDefaultItem(ItemId.Value, ItemTypeId.Value));
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
                TestPropertyNotSetException.ThrowIfNull(ItemTypeId);

                _recipeMocks.ToList()
                    .ForEach(m => m.VerifyRemoveDefaultItem(ItemId.Value, ItemTypeId.Value, Times.Once));
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

    public class ModifyIngredientsAfterAvailabilityWasDeletedAsync
    {
        private readonly ModifyIngredientsAfterAvailabilityWasDeletedAsyncFixture _fixture = new();

        [Fact]
        public async Task ModifyIngredientsAfterAvailabilityWasDeletedAsync_WithFindingItem_ShouldModifyRecipes()
        {
            // Arrange
            _fixture.SetupFindingRecipes();
            _fixture.SetupFindingItem();
            _fixture.SetupModifyingRecipes();
            _fixture.SetupStoringRecipes();
            var sut = _fixture.CreateSut();

            // Act
            await sut.ModifyIngredientsAfterAvailabilityWasDeletedAsync(_fixture.ItemId, _fixture.ItemTypeId,
                _fixture.StoreId);

            // Assert
            _fixture.VerifyModifyingRecipes();
            _fixture.VerifyStoringRecipes();
        }

        [Fact]
        public async Task ModifyIngredientsAfterAvailabilityWasDeletedAsync_WithNotFindingItem_ShouldThrow()
        {
            // Arrange
            _fixture.SetupFindingRecipes();
            _fixture.SetupNotFindingItem();
            var sut = _fixture.CreateSut();

            // Act
            var func = async () => await sut.ModifyIngredientsAfterAvailabilityWasDeletedAsync(_fixture.ItemId,
                _fixture.ItemTypeId, _fixture.StoreId);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemNotFound);
        }

        private sealed class ModifyIngredientsAfterAvailabilityWasDeletedAsyncFixture : RecipeModificationServiceFixture
        {
            private IReadOnlyCollection<RecipeMock>? _recipeMocks;
            private Item? _item;

            public ItemId ItemId { get; } = ItemId.New;
            public ItemTypeId? ItemTypeId { get; } = Domain.Items.Models.ItemTypeId.New;
            public StoreId StoreId { get; } = StoreId.New;

            public void SetupFindingRecipes()
            {
                _recipeMocks = new List<RecipeMock>
                {
                    new(MockBehavior.Strict),
                    new(MockBehavior.Strict),
                };

                RecipeRepositoryMock.SetupFindByAsync(ItemId, ItemTypeId, StoreId, _recipeMocks.Select(m => m.Object));
            }

            public void SetupFindingItem()
            {
                _item = ItemMother.Initial().Create();
                ItemRepositoryMock.SetupFindActiveByAsync(ItemId, _item);
            }

            public void SetupNotFindingItem()
            {
                ItemRepositoryMock.SetupFindActiveByAsync(ItemId, null);
            }

            public void SetupModifyingRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMocks);
                TestPropertyNotSetException.ThrowIfNull(_item);

                foreach (var recipeMock in _recipeMocks)
                {
                    recipeMock.SetupModifyIngredientsAfterAvailabilityWasDeleted(ItemId, ItemTypeId, _item, StoreId);
                }
            }

            public void SetupStoringRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMocks);

                foreach (var recipeMock in _recipeMocks)
                {
                    RecipeRepositoryMock.SetupStoreAsync(recipeMock.Object, recipeMock.Object);
                }
            }

            public void VerifyModifyingRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMocks);
                TestPropertyNotSetException.ThrowIfNull(_item);

                foreach (var recipeMock in _recipeMocks)
                {
                    recipeMock.VerifyModifyIngredientsAfterAvailabilityWasDeleted(ItemId, ItemTypeId, _item, StoreId, Times.Once);
                }
            }

            public void VerifyStoringRecipes()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipeMocks);

                foreach (var recipeMock in _recipeMocks)
                {
                    RecipeRepositoryMock.VerifyStoreAsync(recipeMock.Object, Times.Once);
                }
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
                RecipeRepositoryMock.Object,
                ItemRepositoryMock.Object,
                ValidatorMock.Object);
        }
    }
}