﻿using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Commands.CreateRecipe;
using ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Commands.ModifyRecipe;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Contexts;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Entities;
using ProjectHermes.Xipona.Api.Repositories.Items.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Contexts;
using ProjectHermes.Xipona.Api.Repositories.RecipeTags.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Stores.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Stores.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.ItemCategories.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Recipes.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.RecipeTags.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Stores.Entities;
using ProjectHermes.Xipona.Api.TestTools.AutoFixture;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using ProjectHermes.Xipona.Api.TestTools.Extensions;
using System;
using System.Text.RegularExpressions;
using Xunit;
using Ingredient = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Ingredient;
using Item = ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item;
using ItemType = ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemType;
using PreparationStep = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.PreparationStep;
using Recipe = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Recipe;

namespace ProjectHermes.Xipona.Api.Endpoint.IntegrationTests.v1.Endpoints;

public class RecipeEndpointsIntegrationTests
{
    public class CreateRecipeAsync : IAssemblyFixture<DockerFixture>
    {
        private readonly CreateRecipeAsyncFixture _fixture;

        public CreateRecipeAsync(DockerFixture dockerFixture)
        {
            _fixture = new CreateRecipeAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task CreateRecipeAsync_WithSideDishExisting_ShouldCreateRecipe()
        {
            // Arrange
            _fixture.SetupExistingSideDish();
            _fixture.SetupExpectedEntity();
            _fixture.SetupContract();
            _fixture.SetupExpectedResult();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedEntity);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            using var assertScope = _fixture.CreateServiceScope();
            result.Should().BeOfType<CreatedAtRoute<RecipeContract>>();
            var createdResult = result as CreatedAtRoute<RecipeContract>;
            createdResult!.Value.Should().BeEquivalentTo(_fixture.ExpectedResult, opt => opt.Excluding(info =>
                info.Path.EndsWith(".Id") || info.Path == "Id"));

            var recipeEntities = (await _fixture.LoadAllRecipesAsync(assertScope)).ToList();
            recipeEntities.Should().HaveCount(2);
            var recipe = recipeEntities.First(r => r.Id != _fixture.ExistingSideDishId);
            recipe.Should().BeEquivalentTo(_fixture.ExpectedEntity, opt => opt
                .Excluding(info =>
                    info.Path.EndsWith(".Id")
                    || info.Path == "Id"
                    || info.Path == "CreatedAt"
                    || Regex.IsMatch(info.Path, @"^Tags\[\d+\]\.Recipe")
                    || Regex.IsMatch(info.Path, @"^Ingredients\[\d+\]\.Recipe")
                    || Regex.IsMatch(info.Path, @"^PreparationSteps\[\d+\]\.Recipe"))
                .ExcludeRowVersion()
            );
            recipe.CreatedAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(20));
        }

        [Fact]
        public async Task CreateRecipeAsync_WithSideDishNotExisting_ShouldReturnUnprocessableEntity()
        {
            // Arrange
            _fixture.SetupExpectedEntityWithInvalidSideDish();
            _fixture.SetupContract();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedEntity);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            using var assertScope = _fixture.CreateServiceScope();
            result.Should().BeOfType<UnprocessableEntity<ErrorContract>>();
            var ueResult = result as UnprocessableEntity<ErrorContract>;
            ueResult!.Value.Should().NotBeNull();
            ueResult.Value.Should().BeOfType<ErrorContract>();
            var error = ueResult.Value!;
            error.ErrorCode.Should().Be((int)ErrorReasonCode.RecipeNotFound);
        }

        private sealed class CreateRecipeAsyncFixture : RecipeEndpointFixture
        {
            private List<ItemCategory>? _itemCategories;
            private List<Item>? _items;
            private Recipe? _existingSideDish;

            public CreateRecipeAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public CreateRecipeContract? Contract { get; private set; }
            public RecipeContract? ExpectedResult { get; private set; }
            public Recipe? ExpectedEntity { get; private set; }
            public Guid? ExistingSideDishId => _existingSideDish?.Id;

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(Contract);

                var scope = CreateServiceScope();
                return await RecipeEndpoints.CreateRecipe(
                    Contract,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider
                        .GetRequiredService<IToDomainConverter<CreateRecipeContract, CreateRecipeCommand>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<RecipeReadModel, RecipeContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    default);
            }

            public async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedEntity);
                TestPropertyNotSetException.ThrowIfNull(Contract);
                TestPropertyNotSetException.ThrowIfNull(_itemCategories);
                TestPropertyNotSetException.ThrowIfNull(_items);

                await ApplyMigrationsAsync(ArrangeScope);

                if (_existingSideDish is not null)
                {
                    await using var recipeContext = GetContextInstance<RecipeContext>(ArrangeScope);
                    await recipeContext.AddAsync(_existingSideDish);
                    await recipeContext.SaveChangesAsync();
                }

                await using var itemCategoryContext = GetContextInstance<ItemCategoryContext>(ArrangeScope);
                await using var itemContext = GetContextInstance<ItemContext>(ArrangeScope);
                await using var tagContext = GetContextInstance<RecipeTagContext>(ArrangeScope);

                await itemCategoryContext.AddRangeAsync(_itemCategories);
                await itemContext.AddRangeAsync(_items);

                foreach (var tagId in Contract.RecipeTagIds)
                {
                    await tagContext.RecipeTags.AddAsync(new RecipeTagEntityBuilder().WithId(tagId).Create());
                }

                await itemCategoryContext.SaveChangesAsync();
                await itemContext.SaveChangesAsync();
                await tagContext.SaveChangesAsync();
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedEntity);

                Contract = new CreateRecipeContract(
                    ExpectedEntity.Name,
                    ExpectedEntity.NumberOfServings,
                    ExpectedEntity.Ingredients.Select(i => new CreateIngredientContract(
                        i.ItemCategoryId,
                        i.QuantityType,
                        i.Quantity,
                        i.DefaultItemId,
                        i.DefaultItemTypeId,
                        i.DefaultStoreId,
                        i.AddToShoppingListByDefault)),
                    ExpectedEntity.PreparationSteps.Select(p => new CreatePreparationStepContract(
                        p.Instruction,
                        p.SortingIndex)),
                    ExpectedEntity.Tags.Select(t => t.RecipeTagId),
                    ExpectedEntity.SideDishId);
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedEntity);
                TestPropertyNotSetException.ThrowIfNull(_existingSideDish);
                TestPropertyNotSetException.ThrowIfNull(_itemCategories);
                TestPropertyNotSetException.ThrowIfNull(_items);

                var ingredients = new List<IngredientContract>()
                {
                    CreateContract(ExpectedEntity.Ingredients.ElementAt(0), _itemCategories[0].Name),
                    CreateContract(ExpectedEntity.Ingredients.ElementAt(1), _items[0].Name),
                    CreateContract(ExpectedEntity.Ingredients.ElementAt(2), $"{_items[1].Name} {_items[1].ItemTypes.First().Name}")
                };

                ExpectedResult = new RecipeContract(
                    ExpectedEntity.Id,
                    ExpectedEntity.Name,
                    ExpectedEntity.NumberOfServings,
                    ingredients,
                    ExpectedEntity.PreparationSteps.Select(p => new PreparationStepContract(
                        p.Id,
                        p.Instruction,
                        p.SortingIndex)),
                    ExpectedEntity.Tags.Select(t => t.RecipeTagId),
                    new SideDishContract(_existingSideDish.Id, _existingSideDish.Name));

                IngredientContract CreateContract(Ingredient i, string name)
                {
                    return new IngredientContract(
                        i.Id,
                        name,
                        i.ItemCategoryId,
                        i.QuantityType,
                        i.Quantity,
                        i.DefaultItemId,
                        i.DefaultItemTypeId,
                        i.DefaultStoreId,
                        i.AddToShoppingListByDefault);
                }
            }

            public void SetupExpectedEntity()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingSideDish);

                var ingredients = new List<Ingredient>()
                {
                    new IngredientEntityBuilder().WithoutDefaultItemId().WithoutDefaultItemTypeId()
                        .WithoutDefaultStoreId().WithoutAddToShoppingListByDefault().Create(),
                    new IngredientEntityBuilder().WithoutDefaultItemTypeId().Create(),
                    new IngredientEntityBuilder().Create()
                };

                ExpectedEntity = new RecipeEntityBuilder()
                    .WithIngredients(ingredients)
                    .WithSideDishId(_existingSideDish.Id)
                    .Create();

                SetupItemCategoriesAndItems();
            }

            public void SetupExpectedEntityWithInvalidSideDish()
            {
                var ingredients = new List<Ingredient>()
                {
                    new IngredientEntityBuilder().WithoutDefaultItemId().WithoutDefaultItemTypeId()
                        .WithoutDefaultStoreId().WithoutAddToShoppingListByDefault().Create(),
                    new IngredientEntityBuilder().WithoutDefaultItemTypeId().Create(),
                    new IngredientEntityBuilder().Create()
                };

                ExpectedEntity = new RecipeEntityBuilder()
                    .WithIngredients(ingredients)
                    .Create();

                SetupItemCategoriesAndItems();
            }

            private void SetupItemCategoriesAndItems()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedEntity);

                _itemCategories = ExpectedEntity.Ingredients
                    .Select(i => new ItemCategoryEntityBuilder()
                        .WithId(i.ItemCategoryId)
                        .WithDeleted(false)
                        .Create())
                    .ToList();

                _items = new List<Item>()
                {
                    ItemEntityMother.Initial()
                        .WithId(ExpectedEntity.Ingredients.ElementAt(1).DefaultItemId!.Value)
                        .Create(),
                    ItemEntityMother.InitialWithTypes()
                        .WithId(ExpectedEntity.Ingredients.ElementAt(2).DefaultItemId!.Value)
                        .WithItemType(ItemTypeEntityMother.Initial()
                            .WithId(ExpectedEntity.Ingredients.ElementAt(2).DefaultItemTypeId!.Value).Create())
                        .Create(),
                };
            }

            public void SetupExistingSideDish()
            {
                _existingSideDish = new RecipeEntityBuilder().WithEmptyTags().WithoutSideDishId().Create();
            }
        }
    }

    public class SearchRecipesByNameAsync : IAssemblyFixture<DockerFixture>
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().BeOfType<Ok<List<RecipeSearchResultContract>>>();
            var collection = ((Ok<List<RecipeSearchResultContract>>)result).Value;
            collection.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class SearchRecipesByNameAsyncFixture : RecipeEndpointFixture
        {
            private Recipe? _existingEntityToBeFound;
            private Recipe? _existingEntityNotToBeFound;

            public SearchRecipesByNameAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public IReadOnlyCollection<RecipeSearchResultContract>? ExpectedResult { get; private set; }
            public string? SearchInput { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(SearchInput);

                var scope = CreateServiceScope();
                return await RecipeEndpoints.SearchRecipesByName(
                    SearchInput,
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider
                        .GetRequiredService<IToContractConverter<RecipeSearchResult, RecipeSearchResultContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    default);
            }

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

    public class ModifyRecipeAsync : IAssemblyFixture<DockerFixture>
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

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedRecipe);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            using var assertScope = _fixture.CreateServiceScope();
            result.Should().BeOfType<NoContent>();
            var entities = (await _fixture.LoadAllRecipesAsync(assertScope)).ToList();
            entities.Should().HaveCount(2);
            var entity = entities.First(r => r.Id == _fixture.ExpectedRecipe.Id);

            entity.Should().BeEquivalentTo(_fixture.ExpectedRecipe,
                opt => opt.Excluding(p => p.Ingredients).Excluding(p => p.PreparationSteps).Excluding(p => p.Tags)
                    .WithCreatedAtPrecision().ExcludeRecipeCycleRef().ExcludeRowVersion());

            entity.Ingredients.Should().HaveCount(3);
            entity.PreparationSteps.Should().HaveCount(3);

            // tags
            entity.Tags.Select(t => t.RecipeTagId).Should()
                .BeEquivalentTo(_fixture.ExpectedRecipe.Tags.Select(t => t.RecipeTagId));

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

        private class ModifyRecipeAsyncFixture : RecipeEndpointFixture
        {
            private Recipe? _existingRecipe;
            private Recipe? _existingSideDish;
            private List<ItemCategory>? _itemCategories;
            private List<Item>? _items;

            public ModifyRecipeAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public RecipeId? RecipeId { get; private set; }
            public Recipe? ExpectedRecipe { get; private set; }
            public ModifyRecipeContract? Contract { get; private set; }

            public async Task<IResult> ActAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(RecipeId);
                TestPropertyNotSetException.ThrowIfNull(Contract);

                var scope = CreateServiceScope();
                return await RecipeEndpoints.ModifyRecipe(
                    RecipeId.Value,
                    Contract,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<
                        IToDomainConverter<(Guid, ModifyRecipeContract), ModifyRecipeCommand>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    default);
            }

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
                    .WithoutSideDishId()
                    .Create();
                _existingSideDish = new RecipeEntityBuilder().WithEmptyTags().WithoutSideDishId().Create();
            }

            public void SetupExpectedRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingRecipe);
                TestPropertyNotSetException.ThrowIfNull(_existingSideDish);
                TestPropertyNotSetException.ThrowIfNull(RecipeId);

                var recipeId = RecipeId.Value;
                var ingredients = GetExpectedIngredients().ToList();
                var steps = GetExpectedPreparationSteps().ToList();

                ExpectedRecipe = new RecipeEntityBuilder()
                    .WithId(recipeId)
                    .WithIngredients(ingredients)
                    .WithPreparationSteps(steps)
                    .WithSideDishId(_existingSideDish.Id)
                    .WithCreatedAt(_existingRecipe.CreatedAt)
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
                    ExpectedRecipe.NumberOfServings,
                    ingredients,
                    steps,
                    ExpectedRecipe.Tags.Select(t => t.RecipeTagId),
                    ExpectedRecipe.SideDishId);
            }

            public async Task PrepareDatabase()
            {
                TestPropertyNotSetException.ThrowIfNull(_existingRecipe);
                TestPropertyNotSetException.ThrowIfNull(_existingSideDish);
                TestPropertyNotSetException.ThrowIfNull(_itemCategories);
                TestPropertyNotSetException.ThrowIfNull(_items);
                TestPropertyNotSetException.ThrowIfNull(Contract);

                await ApplyMigrationsAsync(ArrangeScope);
                await using var recipeContext = GetContextInstance<RecipeContext>(ArrangeScope);
                await using var tagDbContext = GetContextInstance<RecipeTagContext>(ArrangeScope);
                await using var itemCategoryDbContext = GetContextInstance<ItemCategoryContext>(ArrangeScope);
                await using var itemDbContext = GetContextInstance<ItemContext>(ArrangeScope);

                recipeContext.Add(_existingRecipe);
                recipeContext.Add(_existingSideDish);
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

                await recipeContext.SaveChangesAsync();
                await itemCategoryDbContext.SaveChangesAsync();
                await itemDbContext.SaveChangesAsync();
                await tagDbContext.SaveChangesAsync();
            }
        }
    }

    public class GetItemAmountsForOneServingAsync : IAssemblyFixture<DockerFixture>
    {
        private readonly GetItemAmountsForOneServingAsyncFixture _fixture;

        public GetItemAmountsForOneServingAsync(DockerFixture dockerFixture)
        {
            _fixture = new GetItemAmountsForOneServingAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task GetItemAmountsForOneServingAsync_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupRecipeId();
            _fixture.SetupExpectedResult();
            _fixture.SetupStores();
            _fixture.SetupItems();
            _fixture.SetupRecipe();
            await _fixture.SetupDatabase();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().BeOfType<Ok<ItemAmountsForOneServingContract>>();
            var okObjectResult = (Ok<ItemAmountsForOneServingContract>)result;
            okObjectResult.Value.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        private sealed class GetItemAmountsForOneServingAsyncFixture : RecipeEndpointFixture
        {
            private readonly List<Item> _items = new();
            private readonly List<Store> _stores = new();
            private Recipe? _recipe;

            public GetItemAmountsForOneServingAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public Guid RecipeId { get; private set; }
            public ItemAmountsForOneServingContract? ExpectedResult { get; private set; }

            public async Task<IResult> ActAsync()
            {
                var scope = CreateServiceScope();
                return await RecipeEndpoints.GetItemAmountsForOneServing(
                    RecipeId,
                    scope.ServiceProvider.GetRequiredService<IQueryDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<
                        IToContractConverter<IEnumerable<ItemAmountForOneServing>, ItemAmountsForOneServingContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    default);
            }

            public void SetupRecipeId()
            {
                RecipeId = Guid.NewGuid();
            }

            public void SetupExpectedResult()
            {
                var availability1 = new TestBuilder<ItemAmountForOneServingAvailabilityContract>().Create();
                var availability2 = new TestBuilder<ItemAmountForOneServingAvailabilityContract>().Create();
                var fluidToUnitName = $"{new DomainTestBuilder<string>().Create()} {new DomainTestBuilder<string>().Create()}";

                ExpectedResult = new ItemAmountsForOneServingContract(new List<ItemAmountForOneServingContract>
                {
                    // unit to unit
                    new TestBuilder<ItemAmountForOneServingContract>()
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.ItemTypeId).LowerFirstChar(), (Guid?)null)
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.Quantity).LowerFirstChar(), 0.5f) // 1.5 units (with 3 units in 1 packet)
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.QuantityType).LowerFirstChar(), (int)QuantityType.Unit)
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.QuantityLabel).LowerFirstChar(), "x")
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.DefaultStoreId).LowerFirstChar(), availability2.StoreId)
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.Availabilities).LowerFirstChar(),
                            new List<ItemAmountForOneServingAvailabilityContract>
                            {
                                availability1,
                                availability2
                            }.AsEnumerable())
                        .Create(),
                    // teaspoon to weight
                    new TestBuilder<ItemAmountForOneServingContract>()
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.ItemTypeId).LowerFirstChar(), (Guid?)null)
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.Quantity).LowerFirstChar(), 6f) // 2 teaspoons
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.QuantityType).LowerFirstChar(), (int)QuantityType.Weight)
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.QuantityLabel).LowerFirstChar(), "g")
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.DefaultStoreId).LowerFirstChar(), availability1.StoreId)
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.Availabilities).LowerFirstChar(), availability1.ToMonoList().AsEnumerable())
                        .Create(),
                    // fluid to unit
                    new TestBuilder<ItemAmountForOneServingContract>()
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.ItemName).LowerFirstChar(), fluidToUnitName)
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.Quantity).LowerFirstChar(), 1.25f) // 2.5 x 500 ml
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.QuantityType).LowerFirstChar(), (int)QuantityType.Unit)
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.QuantityLabel).LowerFirstChar(), "x")
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.DefaultStoreId).LowerFirstChar(), availability1.StoreId)
                        .FillConstructorWith(nameof(ItemAmountForOneServingContract.Availabilities).LowerFirstChar(), availability1.ToMonoList().AsEnumerable())
                        .Create(),
                });
            }

            public void SetupStores()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                var firstExpItem = ExpectedResult.Items.First();

                var store1 = StoreEntityMother.Initial()
                    .WithId(firstExpItem.Availabilities.First().StoreId)
                    .WithName(firstExpItem.Availabilities.First().StoreName)
                    .Create();
                var store2 = StoreEntityMother.Initial()
                    .WithId(firstExpItem.Availabilities.Last().StoreId)
                    .WithName(firstExpItem.Availabilities.Last().StoreName)
                    .Create();

                _stores.Add(store1);
                _stores.Add(store2);
            }

            public void SetupItems()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                var firstExpItem = ExpectedResult.Items.First();
                var secondExpItem = ExpectedResult.Items.Skip(1).First();
                var thirdExpItem = ExpectedResult.Items.Last();

                var unitToUnitAvailabilities = new List<AvailableAt>
                {
                    AvailableAtEntityMother.InitialForStore(firstExpItem.Availabilities.First().StoreId)
                        .WithPrice(firstExpItem.Availabilities.First().Price)
                        .Create(),
                    AvailableAtEntityMother.InitialForStore(firstExpItem.Availabilities.Last().StoreId)
                        .WithPrice(firstExpItem.Availabilities.Last().Price)
                        .Create(),
                };
                var unitToUnitItem = ItemEntityMother.Initial()
                    .WithId(firstExpItem.ItemId)
                    .WithName(firstExpItem.ItemName)
                    .WithAvailableAt(unitToUnitAvailabilities)
                    .WithQuantityType(QuantityType.Unit.ToInt())
                    .WithQuantityInPacket(3f)
                    .WithQuantityTypeInPacket(QuantityTypeInPacket.Unit.ToInt())
                    .Create();

                var teaspoonToWeightAvailabilities = new List<AvailableAt>
                {
                    AvailableAtEntityMother.InitialForStore(secondExpItem.Availabilities.First().StoreId)
                        .WithPrice(secondExpItem.Availabilities.First().Price)
                        .Create(),
                };
                var teaspoonToWeightItem = ItemEntityMother.Initial()
                    .WithId(secondExpItem.ItemId)
                    .WithName(secondExpItem.ItemName)
                    .WithAvailableAt(teaspoonToWeightAvailabilities)
                    .WithQuantityType(QuantityType.Weight.ToInt())
                    .WithoutQuantityInPacket()
                    .WithoutQuantityTypeInPacket()
                    .Create();

                var nameParts = thirdExpItem.ItemName!.Split(' ');
                var fluidToUnitAvailabilities = new List<ItemTypeAvailableAt>
                {
                    ItemTypeAvailableAtEntityMother.InitialForStore(thirdExpItem.Availabilities.First().StoreId)
                        .WithPrice(thirdExpItem.Availabilities.First().Price)
                        .Create(),
                };
                var fluidToUnitItemTypes = new List<ItemType>
                {
                    ItemTypeEntityMother.Initial()
                        .WithId(thirdExpItem.ItemTypeId!.Value)
                        .WithName(nameParts.Last())
                        .WithAvailableAt(fluidToUnitAvailabilities).Create(),
                };
                var fluidToUnitItem = ItemEntityMother.InitialWithTypes()
                    .WithId(thirdExpItem.ItemId)
                    .WithName(nameParts.First())
                    .WithItemTypes(fluidToUnitItemTypes)
                    .WithQuantityType(QuantityType.Unit.ToInt())
                    .WithQuantityInPacket(500f)
                    .WithQuantityTypeInPacket(QuantityTypeInPacket.Fluid.ToInt())
                    .Create();

                _items.Add(unitToUnitItem);
                _items.Add(teaspoonToWeightItem);
                _items.Add(fluidToUnitItem);
            }

            public void SetupRecipe()
            {
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                var firstExpItem = ExpectedResult.Items.First();
                var secondExpItem = ExpectedResult.Items.Skip(1).First();
                var thirdExpItem = ExpectedResult.Items.Last();

                var unitToUnitIngredient = new IngredientEntityBuilder()
                    .WithDefaultItemId(firstExpItem.ItemId)
                    .WithoutDefaultItemTypeId()
                    .WithDefaultStoreId(firstExpItem.DefaultStoreId)
                    .WithQuantity(3f)
                    .WithQuantityType(IngredientQuantityType.Unit.ToInt())
                    .WithAddToShoppingListByDefault(firstExpItem.AddToShoppingListByDefault)
                    .Create();

                var teaspoonToWeightIngredient = new IngredientEntityBuilder()
                    .WithDefaultItemId(secondExpItem.ItemId)
                    .WithoutDefaultItemTypeId()
                    .WithDefaultStoreId(secondExpItem.DefaultStoreId)
                    .WithQuantity(2f)
                    .WithQuantityType(IngredientQuantityType.Teaspoon.ToInt())
                    .WithAddToShoppingListByDefault(secondExpItem.AddToShoppingListByDefault)
                    .Create();

                var fluidToUnitIngredient = new IngredientEntityBuilder()
                    .WithDefaultItemId(thirdExpItem.ItemId)
                    .WithDefaultItemTypeId(thirdExpItem.ItemTypeId!.Value)
                    .WithDefaultStoreId(thirdExpItem.DefaultStoreId)
                    .WithQuantity(1250f)
                    .WithQuantityType(IngredientQuantityType.Fluid.ToInt())
                    .WithAddToShoppingListByDefault(thirdExpItem.AddToShoppingListByDefault)
                    .Create();

                _recipe = new RecipeEntityBuilder()
                    .WithId(RecipeId)
                    .WithIngredients(new List<Ingredient>
                    {
                        unitToUnitIngredient,
                        teaspoonToWeightIngredient,
                        fluidToUnitIngredient,
                    })
                    .WithNumberOfServings(2)
                    .WithoutSideDishId()
                    .Create();
            }

            public async Task SetupDatabase()
            {
                TestPropertyNotSetException.ThrowIfNull(_recipe);

                await ApplyMigrationsAsync(ArrangeScope);
                await using var storeDbContext = GetContextInstance<StoreContext>(ArrangeScope);
                await using var itemDbContext = GetContextInstance<ItemContext>(ArrangeScope);
                await using var recipeDbContext = GetContextInstance<RecipeContext>(ArrangeScope);

                await storeDbContext.Stores.AddRangeAsync(_stores);
                await itemDbContext.Items.AddRangeAsync(_items);
                await recipeDbContext.Recipes.AddAsync(_recipe);

                await storeDbContext.SaveChangesAsync();
                await itemDbContext.SaveChangesAsync();
                await recipeDbContext.SaveChangesAsync();
            }
        }
    }

    public abstract class RecipeEndpointFixture : DatabaseFixture
    {
        protected readonly IServiceScope ArrangeScope;

        protected RecipeEndpointFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            ArrangeScope = CreateServiceScope();
        }

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<StoreContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
            yield return scope.ServiceProvider.GetRequiredService<RecipeTagContext>();
            yield return scope.ServiceProvider.GetRequiredService<RecipeContext>();
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