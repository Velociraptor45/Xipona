using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Contexts;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Text.RegularExpressions;
using Xunit;
using ItemCategoryEntities = ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities;
using RecipeEntities = ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities;

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
            await _fixture.PrepareDatabase();

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
                info.SelectedMemberPath.EndsWith(".Id")));

            var recipeEntities = (await _fixture.LoadAllRecipesAsync()).ToList();
            recipeEntities.Should().HaveCount(1);
            recipeEntities.First().Should().BeEquivalentTo(_fixture.ExpectedEntity, opt => opt.Excluding(info =>
                info.SelectedMemberPath.EndsWith(".Id")
                || Regex.IsMatch(info.SelectedMemberPath, @"Ingredients\[\d+\]\.Recipe")
                || Regex.IsMatch(info.SelectedMemberPath, @"PreparationSteps\[\d+\]\.Recipe")
            ));
        }

        private sealed class CreateRecipeAsyncFixture : RecipeControllerFixture
        {
            private IRecipe? _model;

            public CreateRecipeAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public CreateRecipeContract? Contract { get; private set; }
            public RecipeContract? ExpectedResult { get; private set; }
            public RecipeEntities.Recipe? ExpectedEntity { get; private set; }

            public async Task PrepareDatabase()
            {
                TestPropertyNotSetException.ThrowIfNull(_model);

                await ApplyMigrationsAsync(ArrangeScope);
                await using var itemCategoryContext = GetContextInstance<ItemCategoryContext>(ArrangeScope);

                foreach (var ingredient in _model.Ingredients)
                {
                    var itemCategory = new TestBuilder<ItemCategoryEntities.ItemCategory>()
                        .FillPropertyWith(i => i.Id, ingredient.ItemCategoryId.Value)
                        .FillPropertyWith(i => i.Deleted, false)
                        .Create();

                    itemCategoryContext.Add(itemCategory);
                }

                await itemCategoryContext.SaveChangesAsync();
            }

            public void SetupContract()
            {
                TestPropertyNotSetException.ThrowIfNull(_model);

                Contract = new CreateRecipeContract(
                    _model.Name.Value,
                    _model.Ingredients.Select(i => new CreateIngredientContract(
                        i.ItemCategoryId.Value,
                        (int)i.QuantityType,
                        i.Quantity.Value)),
                    _model.PreparationSteps.Select(p => new CreatePreparationStepContract(
                        p.Instruction.Value,
                        p.SortingIndex)));
            }

            public void SetupExpectedResult()
            {
                TestPropertyNotSetException.ThrowIfNull(_model);

                ExpectedResult = new RecipeContract(
                    _model.Id.Value,
                    _model.Name.Value,
                    _model.Ingredients.Select(i => new IngredientContract(
                        i.Id.Value,
                        i.ItemCategoryId.Value,
                        (int)i.QuantityType,
                        i.Quantity.Value)),
                    _model.PreparationSteps.Select(p => new PreparationStepContract(
                        p.Id.Value,
                        p.Instruction.Value,
                        p.SortingIndex)));
            }

            public void SetupExpectedEntity()
            {
                TestPropertyNotSetException.ThrowIfNull(_model);

                ExpectedEntity = new RecipeEntities.Recipe
                {
                    Id = _model.Id.Value,
                    Name = _model.Name.Value,
                    Ingredients = _model.Ingredients.Select(i => new RecipeEntities.Ingredient
                    {
                        Id = i.Id.Value,
                        ItemCategoryId = i.ItemCategoryId.Value,
                        Quantity = i.Quantity.Value,
                        QuantityType = (int)i.QuantityType,
                        RecipeId = _model.Id.Value
                    }).ToList(),
                    PreparationSteps = _model.PreparationSteps.Select(p => new RecipeEntities.PreparationStep
                    {
                        Id = p.Id.Value,
                        Instruction = p.Instruction.Value,
                        SortingIndex = p.SortingIndex,
                        RecipeId = _model.Id.Value
                    }).ToList()
                };
            }

            public void SetupModel()
            {
                _model = new RecipeBuilder().Create();
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
            yield return scope.ServiceProvider.GetRequiredService<RecipeContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
        }

        public async Task<IEnumerable<RecipeEntities.Recipe>> LoadAllRecipesAsync()
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