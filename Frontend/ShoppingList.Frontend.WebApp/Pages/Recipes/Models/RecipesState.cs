using ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Recipes.Models;

public class RecipesState
{
    private List<RecipeSearchResult> _searchResults;
    private List<IngredientQuantityType> _ingredientQuantityTypes;

    public RecipesState()
    {
        _searchResults = new List<RecipeSearchResult>();
    }

    public IReadOnlyCollection<RecipeSearchResult> SearchResults => _searchResults;
    public IReadOnlyCollection<IngredientQuantityType> IngredientQuantityTypes => _ingredientQuantityTypes;
    public Recipe EditedRecipe { get; private set; }

    public void RegisterSearchResults(IEnumerable<RecipeSearchResult> searchResults)
    {
        _searchResults = searchResults.ToList();
    }

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
}