using FluentAssertions;
using Force.DeepCloner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Contexts;
using ProjectHermes.Xipona.Api.Repositories.ItemCategories.Entities;
using ProjectHermes.Xipona.Api.Repositories.Items.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.ItemCategories.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Recipes.Entities;
using ProjectHermes.Xipona.Api.TestTools.AutoFixture;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System;
using Xunit;

namespace ProjectHermes.Xipona.Api.Endpoint.IntegrationTests.v1.Controllers;

public class ItemCategoryControllerIntegrationTests
{
    public sealed class DeleteItemCategoryAsync : IAssemblyFixture<DockerFixture>
    {
        private readonly DeleteItemCategoryAsyncFixture _fixture;

        public DeleteItemCategoryAsync(DockerFixture dockerFixture)
        {
            _fixture = new DeleteItemCategoryAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task DeleteItemCategoryAsync_WithItemCategoryExisting_ShouldDeleteAndRemoveIngredientFromRecipe()
        {
            // Arrange
            _fixture.SetupItemCategory();
            _fixture.SetupRecipeContainingItemCategory();
            _fixture.SetupExpectedItemCategory();
            await _fixture.PrepareDatabaseAsync();

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContent>();

            using var assertionServiceScope = _fixture.CreateServiceScope();

            var itemCategories = (await _fixture.LoadAllItemCategoriesAsync(assertionServiceScope)).ToList();
            itemCategories.Should().HaveCount(1);
            var itemCategory = itemCategories.First();
            itemCategory.Should().BeEquivalentTo(_fixture.ExpectedItemCategory,
                opt => opt.ExcludeRowVersion().WithCreatedAtPrecision());

            var recipes = (await _fixture.LoadAllRecipesAsync(assertionServiceScope)).ToList();
            recipes.Should().HaveCount(1);
            var recipe = recipes.First();
            recipe.Ingredients.Should().BeEmpty();
        }

        private sealed class DeleteItemCategoryAsyncFixture : ItemCategoryControllerFixture
        {
            private ItemCategory? _itemCategory;
            private Recipe? _recipe;

            public DeleteItemCategoryAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public Guid ItemCategoryId { get; } = Guid.NewGuid();
            public ItemCategory? ExpectedItemCategory { get; private set; }

            public async Task<IResult> ActAsync()
            {
                var scope = CreateServiceScope();
                return await ItemCategoryEndpoints.DeleteItemCategory(
                    ItemCategoryId,
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    scope.ServiceProvider.GetRequiredService<IToDomainConverter<Guid, DeleteItemCategoryCommand>>(),
                    default);
            }

            public void SetupItemCategory()
            {
                _itemCategory = new ItemCategoryEntityBuilder()
                    .WithDeleted(false)
                    .WithId(ItemCategoryId)
                    .Create();
            }

            public void SetupExpectedItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);

                ExpectedItemCategory = _itemCategory.DeepClone();
                ExpectedItemCategory.Deleted = true;
            }

            public void SetupRecipeContainingItemCategory()
            {
                var ingredient = new IngredientEntityBuilder()
                    .WithItemCategoryId(ItemCategoryId)
                    .Create();

                _recipe = new RecipeEntityBuilder()
                    .WithoutSideDishId()
                    .WithIngredients([ingredient])
                    .Create();
            }

            public async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemCategory);
                TestPropertyNotSetException.ThrowIfNull(_recipe);

                await ApplyMigrationsAsync(ArrangeScope);

                await using var itemCategoryContext = ArrangeScope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
                await using var recipeContext = ArrangeScope.ServiceProvider.GetRequiredService<RecipeContext>();

                await itemCategoryContext.AddAsync(_itemCategory);
                await recipeContext.AddAsync(_recipe);

                await itemCategoryContext.SaveChangesAsync();
                await recipeContext.SaveChangesAsync();
            }
        }
    }

    private abstract class ItemCategoryControllerFixture : DatabaseFixture
    {
        protected ItemCategoryControllerFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            ArrangeScope = CreateServiceScope();
        }

        protected readonly IServiceScope ArrangeScope;

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<ItemCategoryContext>();
            yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
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