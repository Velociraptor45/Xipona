using ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Recipes.Services;

public interface IRecipesApiService
{
    Task<IEnumerable<RecipeSearchResult>> SearchAsync(string searchInput);
    Task<Recipe> GetAsync(Guid recipeId);
    Task<Recipe> CreateAsync(Recipe recipe);
    Task<bool> ModifyAsync(Recipe recipe);
    Task<IEnumerable<IngredientQuantityType>> GetAllIngredientQuantityTypes();
}