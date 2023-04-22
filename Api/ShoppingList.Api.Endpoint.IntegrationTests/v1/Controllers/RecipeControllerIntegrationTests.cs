using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.ItemCategories.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Items.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Recipes.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.RecipeTags.Entities;
using ProjectHermes.ShoppingList.Api.TestTools.AutoFixture;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Text.RegularExpressions;
using Xunit;
using Ingredient = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Ingredient;
using PreparationStep = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.PreparationStep;
using Recipe = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Recipe;

namespace ProjectHermes.ShoppingList.Api.Endpoint.IntegrationTests.v1.Controllers;

public class RecipeControllerIntegrationTests
{
    [Collection(DockerCollection.Name)]
    public class CreateRecipeAsync
    {
        private readonly CreateRecipeAsyncFixture _fixture;

        public CreateRecipeAsync(DockerFixture dockerFixture)
        {
            _fixture = new CreateRecipeAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task CreateRecipeAsync_WithValidRequest_ShouldCreateRecipe()
        {
            // Arrange
            _fixture.SetupModel();
            _fixture.SetupContract();
            _fixture.SetupExpectedEntity();
            _fixture.SetupExpectedResult();
            await _fixture.PrepareDatabaseAsync();

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedEntity);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.CreateRecipeAsync(_fixture.Contract);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult!.Value.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(info =>
                info.Path.EndsWith(".Id") || info.Path == "Id"));

            var recipeEntities = (await _fixture.LoadAllRecipesAsync()).ToList();
            recipeEntities.Should().HaveCount(1);
            recipeEntities.First().Should().BeEquivalentTo(_fixture.ExpectedEntity, opt => opt
                .Excluding(info =>
                    info.Path.EndsWith(".Id")
                    || info.Path == "Id"
                    || Regex.IsMatch(info.Path, @"Ingredients\[\d+\]\.Recipe")
                    || Regex.IsMatch(info.Path, @"PreparationSteps\[\d+\]\.Recipe"))
                .ExcludeRowVersion()
            );
        }

        private sealed class CreateRecipeAsyncFixture : RecipeControllerFixture
        {
            private IRecipe? _model;

            public CreateRecipeAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public CreateRecipeContract? Contract { get; private set; }
            public RecipeContract? ExpectedResult { get; private set; }
            public Recipe? ExpectedEntity { get; private set; }

            public async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_model);
                TestPropertyNotSetException.ThrowIfNull(Contract);

                await ApplyMigrationsAsync(ArrangeScope);
                await using var itemCategoryContext = GetContextInstance<ItemCategoryContext>(ArrangeScope);
                await using var itemContext = GetContextInstance<ItemContext>(ArrangeScope);
                await using var tagContext = GetContextInstance<RecipeTagContext>(ArrangeScope);

                foreach (var ingredient in _model.Ingredients)
                {
                    var itemCategory = new TestBuilder<ItemCategory>()
                        .FillPropertyWith(i => i.Id, ingredient.ItemCategoryId)
                        .FillPropertyWith(i => i.Deleted, false)
                        .Create();

                    itemCategoryContext.Add(itemCategory);

                    var item = new ItemEntityBuilder()
                        .WithId(ingredient.DefaultItemId!.Value)
                        .WithDeleted(false)
                        .WithIsTemporary(false)
                        .WithItemTypes(new ItemTypeEntityBuilder()
                            .WithId(ingredient.DefaultItemTypeId!.Value)
                            .WithoutItem()
                            .WithAvailableAt(ItemTypeAvailableAtEntityMother.Initial().CreateMany(1).ToList())
                            .WithoutPredecessorId()
                            .WithoutPredecessor()
                            .CreateMany(1)
                            .ToList())
                        .WithEmptyAvailableAt()
                        .WithoutPredecessorId()
                        .WithoutPredecessor()
                        .Create();

                    itemContext.Add(item);
                }

                foreach (var tagId in Contract.RecipeTagIds)
                {
                    tagContext.RecipeTags.Add(new RecipeTagEntityBuilder().WithId(tagId).Create());
                }

                await itemCategoryContext.SaveChangesAsync();
                await itemContext.SaveChangesAsync();
                await tagContext.SaveChangesAsync();
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(_model);

                Contract = new CreateRecipeContract(
                    _model.Name,
                    _model.Ingredients.Select(i => new CreateIngredientContract(
                        i.ItemCategoryId,
                        (int)i.QuantityType,
                        i.Quantity.Value,
                        i.DefaultItemId,
                        i.DefaultItemTypeId,
                        i.ShoppingListProperties?.DefaultStoreId,
                        i.ShoppingListProperties?.AddToShoppingListByDefault)),
                    _model.PreparationSteps.Select(p => new CreatePreparationStepContract(
                        p.Instruction.Value,
                        p.SortingIndex)),
                    _model.Tags.Select(t => t.Value));
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(_model);

                ExpectedResult = new RecipeContract(
                    _model.Id,
                    _model.Name,
                    _model.Ingredients.Select(i => new IngredientContract(
                        i.Id,
                        i.ItemCategoryId,
                        (int)i.QuantityType,
                        i.Quantity.Value,
                        i.ShoppingListProperties?.DefaultItemId,
                        i.ShoppingListProperties?.DefaultItemTypeId,
                        i.ShoppingListProperties?.DefaultStoreId,
                        i.ShoppingListProperties?.AddToShoppingListByDefault)),
                    _model.PreparationSteps.Select(p => new PreparationStepContract(
                        p.Id,
                        p.Instruction.Value,
                        p.SortingIndex)),
                    _model.Tags.Select(t => t.Value));
            }

            public void SetupExpectedEntity()
            {
                TestPropertyNotSetException.ThrowIfNull(_model);

                ExpectedEntity = new Recipe
                {
                    Id = _model.Id,
                    Name = _model.Name,
                    Ingredients = _model.Ingredients.Select(i => new Ingredient
                    {
                        Id = i.Id,
                        ItemCategoryId = i.ItemCategoryId,
                        Quantity = i.Quantity.Value,
                        QuantityType = (int)i.QuantityType,
                        DefaultItemId = i.ShoppingListProperties?.DefaultItemId,
                        DefaultItemTypeId = i.ShoppingListProperties?.DefaultItemTypeId,
                        DefaultStoreId = i.ShoppingListProperties?.DefaultStoreId,
                        AddToShoppingListByDefault = i.ShoppingListProperties?.AddToShoppingListByDefault,
                        RecipeId = _model.Id
                    }).ToList(),
                    PreparationSteps = _model.PreparationSteps.Select(p => new PreparationStep
                    {
                        Id = p.Id,
                        Instruction = p.Instruction.Value,
                        SortingIndex = p.SortingIndex,
                        RecipeId = _model.Id
                    }).ToList(),
                };
            }

            public void SetupModel()
            {
                _model = new RecipeBuilder().Create();
            }
        }
    }

    [Collection(DockerCollection.Name)]
    public class SearchRecipesByNameAsync
    {
        private readonly SearchRecipesByNameAsyncFixture _fixture;

        public SearchRecipesByNameAsync(DockerFixture dockerFixture)
        {
            _fixture = new SearchRecipesByNameAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task SearchRecipesByNameAsync_WithValidSearchInput_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupExistingRecipe();
            await _fixture.PrepareDatabaseAsync();
            _fixture.SetupSearchInput();
            _fixture.SetupExpectedResult();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.SearchInput);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.SearchRecipesByNameAsync(_fixture.SearchInput);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var collection = ((OkObjectResult)result).Value;
            collection.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class SearchRecipesByNameAsyncFixture : RecipeControllerFixture
        {
            private Recipe? _existingEntityToBeFound;
            private Recipe? _existingEntityNotToBeFound;

            public SearchRecipesByNameAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public IReadOnlyCollection<RecipeSearchResultContract>? ExpectedResult { get; private set; }
            public string? SearchInput { get; private set; }

            public async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingEntityToBeFound);
                TestPropertyNotSetException.ThrowIfNull(_existingEntityNotToBeFound);

                await ApplyMigrationsAsync(ArrangeScope);
                await using var recipeContext = GetContextInstance<RecipeContext>(ArrangeScope);

                recipeContext.Add(_existingEntityToBeFound);
                recipeContext.Add(_existingEntityNotToBeFound);

                await recipeContext.SaveChangesAsync();
            }

            public void SetupSearchInput()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingEntityToBeFound);

                var nameLength = _existingEntityToBeFound.Name.Length;

                var generatorStart = new RandomNumericSequenceGenerator(0, nameLength - 25);
                var start = (int)generatorStart.Create(typeof(int), null);

                var generatorEnd = new RandomNumericSequenceGenerator(start + 7, nameLength - 1);
                var end = (int)generatorEnd.Create(typeof(int), null);

                SearchInput = _existingEntityToBeFound.Name[start..end];
            }

            public void SetupExistingRecipe()
            {
                var modelToBeFound = new RecipeBuilder().Create();
                _existingEntityToBeFound = new Recipe
                {
                    Id = modelToBeFound.Id,
                    Name = modelToBeFound.Name,
                    Ingredients = modelToBeFound.Ingredients.Select(i => new Ingredient
                    {
                        Id = i.Id,
                        ItemCategoryId = i.ItemCategoryId,
                        Quantity = i.Quantity.Value,
                        QuantityType = (int)i.QuantityType,
                        RecipeId = modelToBeFound.Id
                    }).ToList(),
                    PreparationSteps = modelToBeFound.PreparationSteps.Select(p => new PreparationStep
                    {
                        Id = p.Id,
                        Instruction = p.Instruction.Value,
                        SortingIndex = p.SortingIndex,
                        RecipeId = modelToBeFound.Id
                    }).ToList()
                };

                var modelNotToBeFound = new RecipeBuilder().Create();
                _existingEntityNotToBeFound = new Recipe
                {
                    Id = modelNotToBeFound.Id,
                    Name = modelNotToBeFound.Name,
                    Ingredients = modelNotToBeFound.Ingredients.Select(i => new Ingredient
                    {
                        Id = i.Id,
                        ItemCategoryId = i.ItemCategoryId,
                        Quantity = i.Quantity.Value,
                        QuantityType = (int)i.QuantityType,
                        RecipeId = modelNotToBeFound.Id
                    }).ToList(),
                    PreparationSteps = modelNotToBeFound.PreparationSteps.Select(p => new PreparationStep
                    {
                        Id = p.Id,
                        Instruction = p.Instruction.Value,
                        SortingIndex = p.SortingIndex,
                        RecipeId = modelNotToBeFound.Id
                    }).ToList()
                };
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingEntityToBeFound);

                ExpectedResult = new List<RecipeSearchResultContract>()
                {
                    new(_existingEntityToBeFound.Id, _existingEntityToBeFound.Name)
                };
            }
        }
    }

    [Collection(DockerCollection.Name)]
    public class ModifyRecipeAsync
    {
        private readonly ModifyRecipeAsyncFixture _fixture;

        public ModifyRecipeAsync(DockerFixture dockerFixture)
        {
            _fixture = new ModifyRecipeAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task ModifyRecipeAsync_WithAllModificationTypes_ShouldModifyRecipe()
        {
            // Arrange
            _fixture.SetupRecipeId();
            _fixture.SetupExistingRecipe();
            _fixture.SetupExpectedRecipe();
            _fixture.SetupContract();
            await _fixture.PrepareDatabase();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.RecipeId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedRecipe);
            TestPropertyNotSetException.ThrowIfNull(_fixture.Contract);

            // Act
            var result = await sut.ModifyRecipeAsync(_fixture.RecipeId.Value, _fixture.Contract);

            // Assert
            result.Should().BeOfType<OkResult>();
            var entities = (await _fixture.LoadAllRecipesAsync()).ToList();
            entities.Should().HaveCount(1);
            var entity = entities.First();
            entity.Ingredients.Should().HaveCount(3);
            entity.PreparationSteps.Should().HaveCount(3);

            // ingredients
            var firstExpectedIngredient = _fixture.ExpectedRecipe.Ingredients.ElementAt(0);
            var secondExpectedIngredient = _fixture.ExpectedRecipe.Ingredients.ElementAt(1);
            var thirdExpectedIngredient = _fixture.ExpectedRecipe.Ingredients.ElementAt(2);

            var ingredientEntities = entity.Ingredients.ToDictionary(i => i.Id);

            ingredientEntities.Should().ContainKey(firstExpectedIngredient.Id);
            var firstIngredientEntity = ingredientEntities[firstExpectedIngredient.Id];
            ingredientEntities.Remove(firstIngredientEntity.Id);
            firstIngredientEntity.Should().BeEquivalentTo(firstExpectedIngredient,
                opt => opt.Excluding(p => new[] { "Recipe" }.Contains(p.Path)));

            ingredientEntities.Should().ContainKey(secondExpectedIngredient.Id);
            var secondIngredientEntity = ingredientEntities[secondExpectedIngredient.Id];
            ingredientEntities.Remove(secondIngredientEntity.Id);
            secondIngredientEntity.Should().BeEquivalentTo(secondExpectedIngredient,
                opt => opt.Excluding(p => new[] { "Recipe" }.Contains(p.Path)));

            var thirdIngredientEntity = ingredientEntities.Single().Value;
            thirdIngredientEntity.Should().BeEquivalentTo(thirdExpectedIngredient,
                opt => opt.Excluding(p => new[] { "Id", "Recipe" }.Contains(p.Path)));

            // preparation steps
            var firstExpectedStep = _fixture.ExpectedRecipe.PreparationSteps.ElementAt(0);
            var secondExpectedStep = _fixture.ExpectedRecipe.PreparationSteps.ElementAt(1);
            var thirdExpectedStep = _fixture.ExpectedRecipe.PreparationSteps.ElementAt(2);

            var stepEntities = entity.PreparationSteps.ToDictionary(i => i.Id);

            stepEntities.Should().ContainKey(firstExpectedStep.Id);
            var firstStepEntity = stepEntities[firstExpectedStep.Id];
            stepEntities.Remove(firstExpectedStep.Id);
            firstStepEntity.Should().BeEquivalentTo(firstExpectedStep,
                opt => opt.Excluding(p => new[] { "Recipe" }.Contains(p.Path)));

            stepEntities.Should().ContainKey(secondExpectedStep.Id);
            var secondStepEntity = stepEntities[secondExpectedStep.Id];
            stepEntities.Remove(secondStepEntity.Id);
            secondStepEntity.Should().BeEquivalentTo(secondExpectedStep,
                opt => opt.Excluding(p => new[] { "Recipe" }.Contains(p.Path)));

            var thirdStepEntity = stepEntities.Single().Value;
            thirdStepEntity.Should().BeEquivalentTo(thirdExpectedStep,
                opt => opt.Excluding(p => new[] { "Id", "Recipe" }.Contains(p.Path)));
        }

        private class ModifyRecipeAsyncFixture : RecipeControllerFixture
        {
            private Recipe? _existingRecipe;
            private List<ItemCategory>? _itemCategories;
            private List<Item>? _items;

            public ModifyRecipeAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public RecipeId? RecipeId { get; private set; }
            public Recipe? ExpectedRecipe { get; private set; }
            public ModifyRecipeContract? Contract { get; private set; }

            public void SetupRecipeId()
            {
                RecipeId = Domain.Recipes.Models.RecipeId.New;
            }

            public void SetupExistingRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(RecipeId);

                var recipeId = RecipeId.Value;
                _existingRecipe = new RecipeEntityBuilder()
                    .WithId(recipeId)
                    .WithIngredients(new IngredientEntityBuilder().WithRecipeId(recipeId).CreateMany(3).ToList())
                    .WithPreparationSteps(new PreparationStepEntityBuilder().WithRecipeId(recipeId).CreateMany(3).ToList())
                    .WithTags(new TagsForRecipeEntityBuilder().WithRecipeId(recipeId).CreateMany(2).ToList())
                    .Create();
            }

            public void SetupExpectedRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingRecipe);
                TestPropertyNotSetException.ThrowIfNull(RecipeId);

                var recipeId = RecipeId.Value;
                var ingredients = GetExpectedIngredients().ToList();
                var steps = GetExpectedPreparationSteps().ToList();

                ExpectedRecipe = new RecipeEntityBuilder()
                    .WithId(recipeId)
                    .WithIngredients(ingredients)
                    .WithPreparationSteps(steps)
                    .Create();

                _itemCategories = ingredients
                        .Select(i => new ItemCategoryEntityBuilder().WithDeleted(false).WithId(i.ItemCategoryId).Create())
                        .ToList();

                _items = ingredients
                    .Where(i => i.DefaultItemId.HasValue && i.DefaultItemTypeId.HasValue)
                    .Select(i => new ItemEntityBuilder()
                        .WithId(i.DefaultItemId!.Value)
                        .WithDeleted(false)
                        .WithIsTemporary(false)
                        .WithItemTypes(new ItemTypeEntityBuilder()
                            .WithId(i.DefaultItemTypeId!.Value)
                            .WithoutItem()
                            .WithAvailableAt(ItemTypeAvailableAtEntityMother.Initial().CreateMany(1).ToList())
                            .WithoutPredecessorId()
                            .WithoutPredecessor()
                            .CreateMany(1)
                            .ToList())
                        .WithEmptyAvailableAt()
                        .WithoutPredecessorId()
                        .WithoutPredecessor()
                        .Create())
                    .ToList();
            }

            private IEnumerable<Ingredient> GetExpectedIngredients()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingRecipe);

                var first = _existingRecipe.Ingredients.ElementAt(0);
                var second = _existingRecipe.Ingredients.ElementAt(1);
                var third = _existingRecipe.Ingredients.ElementAt(2);

                yield return first;

                yield return new IngredientEntityBuilder()
                    .WithRecipeId(second.RecipeId)
                    .WithId(second.Id)
                    .Create();

                yield return new IngredientEntityBuilder()
                    .WithRecipeId(third.RecipeId)
                    .Create();
            }

            private IEnumerable<PreparationStep> GetExpectedPreparationSteps()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingRecipe);

                var first = _existingRecipe.PreparationSteps.ElementAt(0);
                var second = _existingRecipe.PreparationSteps.ElementAt(1);
                var third = _existingRecipe.PreparationSteps.ElementAt(2);

                yield return first;

                yield return new PreparationStepEntityBuilder()
                    .WithRecipeId(second.RecipeId)
                    .WithId(second.Id)
                    .Create();

                yield return new PreparationStepEntityBuilder()
                    .WithRecipeId(third.RecipeId)
                    .Create();
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedRecipe);

                var firstIngredient = ExpectedRecipe.Ingredients.ElementAt(0);
                var secondIngredient = ExpectedRecipe.Ingredients.ElementAt(1);
                var thirdIngredient = ExpectedRecipe.Ingredients.ElementAt(2);
                var ingredients = new List<ModifyIngredientContract>
                {
                    new(firstIngredient.Id, firstIngredient.ItemCategoryId, firstIngredient.QuantityType,
                        firstIngredient.Quantity, firstIngredient.DefaultItemId, firstIngredient.DefaultItemTypeId,
                        firstIngredient.DefaultStoreId, firstIngredient.AddToShoppingListByDefault),
                    new(secondIngredient.Id, secondIngredient.ItemCategoryId, secondIngredient.QuantityType,
                        secondIngredient.Quantity, secondIngredient.DefaultItemId, secondIngredient.DefaultItemTypeId,
                        secondIngredient.DefaultStoreId, secondIngredient.AddToShoppingListByDefault),
                    new(null, thirdIngredient.ItemCategoryId, thirdIngredient.QuantityType, thirdIngredient.Quantity,
                        thirdIngredient.DefaultItemId, thirdIngredient.DefaultItemTypeId,
                        thirdIngredient.DefaultStoreId, thirdIngredient.AddToShoppingListByDefault)
                };

                var firstStep = ExpectedRecipe.PreparationSteps.ElementAt(0);
                var secondStep = ExpectedRecipe.PreparationSteps.ElementAt(1);
                var thirdStep = ExpectedRecipe.PreparationSteps.ElementAt(2);
                var steps = new List<ModifyPreparationStepContract>
                {
                    new(firstStep.Id, firstStep.Instruction, firstStep.SortingIndex),
                    new(secondStep.Id, secondStep.Instruction, secondStep.SortingIndex),
                    new(null, thirdStep.Instruction, thirdStep.SortingIndex)
                };

                Contract = new ModifyRecipeContract(
                    ExpectedRecipe.Name,
                    ingredients,
                    steps,
                    ExpectedRecipe.Tags.Select(t => t.RecipeTagId));
            }

            public async Task PrepareDatabase()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingRecipe);
                TestPropertyNotSetException.ThrowIfNull(_itemCategories);
                TestPropertyNotSetException.ThrowIfNull(_items);
                TestPropertyNotSetException.ThrowIfNull(Contract);

                await ApplyMigrationsAsync(ArrangeScope);
                await using var dbContext = GetContextInstance<RecipeContext>(ArrangeScope);
                await using var tagDbContext = GetContextInstance<RecipeTagContext>(ArrangeScope);
                await using var itemCategoryDbContext = GetContextInstance<ItemCategoryContext>(ArrangeScope);
                await using var itemDbContext = GetContextInstance<ItemContext>(ArrangeScope);

                dbContext.Add(_existingRecipe);
                itemCategoryDbContext.AddRange(_itemCategories);
                itemDbContext.AddRange(_items);
                foreach (var existingRecipeTag in _existingRecipe.Tags)
                {
                    tagDbContext.RecipeTags
                        .Add(new RecipeTagEntityBuilder().WithId(existingRecipeTag.RecipeTagId).Create());
                }
                foreach (var tagId in Contract.RecipeTagIds)
                {
                    tagDbContext.RecipeTags.Add(new RecipeTagEntityBuilder().WithId(tagId).Create());
                }

                await dbContext.SaveChangesAsync();
                await itemCategoryDbContext.SaveChangesAsync();
                await itemDbContext.SaveChangesAsync();
                await tagDbContext.SaveChangesAsync();
            }
        }
    }

    public abstract class RecipeControllerFixture : DatabaseFixture
    {
        protected readonly IServiceScope ArrangeScope;

        protected RecipeControllerFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            ArrangeScope = CreateServiceScope();
        }

        public RecipeController CreateSut()
        {
            var actScope = CreateServiceScope();
            return actScope.ServiceProvider.GetRequiredService<RecipeController>();
        }

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
            yield return scope.ServiceProvider.GetRequiredService<RecipeTagContext>();
            yield return scope.ServiceProvider.GetRequiredService<RecipeContext>();
        }

        public async Task<IEnumerable<Recipe>> LoadAllRecipesAsync()
        {
            using var assertScope = CreateServiceScope();
            var recipeContext = GetContextInstance<RecipeContext>(assertScope);

            return await recipeContext.Recipes.AsNoTracking()
                .Include(r => r.Ingredients)
                .Include(r => r.PreparationSteps)
                .ToListAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ArrangeScope.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}