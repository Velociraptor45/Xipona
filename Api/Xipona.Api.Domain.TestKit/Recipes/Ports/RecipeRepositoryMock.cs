using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Ports;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Ports;

public class RecipeRepositoryMock : Mock<IRecipeRepository>
{
    public RecipeRepositoryMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupStoreAsync(IRecipe recipe, IRecipe returnValue)
    {
        Setup(m => m.StoreAsync(recipe))
            .ReturnsAsync(returnValue);
    }

    public void VerifyStoreAsync(IRecipe recipe, Func<Times> times)
    {
        Verify(m => m.StoreAsync(recipe), times);
    }

    public void SetupSearchByAsync(string searchInput, IEnumerable<RecipeSearchResult> expectedResult)
    {
        Setup(m => m.SearchByAsync(searchInput)).ReturnsAsync(expectedResult);
    }

    public void SetupFindByAsync(RecipeId recipeId, IRecipe? returnValue)
    {
        Setup(m => m.FindByAsync(recipeId)).ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(ItemId itemId, IEnumerable<IRecipe> returnValue)
    {
        Setup(m => m.FindByAsync(itemId)).ReturnsAsync(returnValue);
    }

    public void SetupFindByAsync(ItemId itemId, ItemTypeId? itemTypeId, StoreId storeId, IEnumerable<IRecipe> returnValue)
    {
        Setup(m => m.FindByAsync(itemId, itemTypeId, storeId)).ReturnsAsync(returnValue);
    }

    public void SetupFindByContainingAllAsync(IEnumerable<RecipeTagId> recipeTagIds, IEnumerable<IRecipe> returnValue)
    {
        Setup(m => m.FindByContainingAllAsync(recipeTagIds)).ReturnsAsync(returnValue);
    }
}