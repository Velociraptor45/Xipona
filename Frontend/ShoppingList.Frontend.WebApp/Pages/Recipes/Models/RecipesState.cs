using ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Recipes.Models;

public class RecipesState
{
    private List<IngredientQuantityType> _ingredientQuantityTypes;

    public IReadOnlyCollection<IngredientQuantityType> IngredientQuantityTypes => _ingredientQuantityTypes;
    public Recipe EditedRecipe { get; private set; }

    public void SetEditedRecipe(Recipe manufacturer)
    {
        EditedRecipe = manufacturer;
    }

    public void SetNewEditedRecipe()
    {
        SetEditedRecipe(new Recipe(Guid.Empty, "New Recipe", Enumerable.Empty<Ingredient>(),
            Enumerable.Empty<PreparationStep>()));
    }

    public void ResetEditedRecipe()
    {
        SetEditedRecipe(null);
    }

    public void RegisterIngredientQuantityTypes(IEnumerable<IngredientQuantityType> ingredientQuantityTypes)
    {
        _ingredientQuantityTypes = ingredientQuantityTypes.ToList();
    }

    public void UpdateRecipeSearchResultName(Guid recipeId, string recipeName)
    {
        //var recipe = _searchResults.FirstOrDefault(r => r.Id == recipeId);
        //if (recipe is null)
        //    return;

        //recipe.ChangeName(recipeName);
    }
}