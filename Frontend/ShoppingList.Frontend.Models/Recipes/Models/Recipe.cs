using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;

public class Recipe
{
    private readonly List<Ingredient> _ingredients;
    private readonly List<PreparationStep> _preparationSteps;

    public Recipe(Guid id, string name, IEnumerable<Ingredient> ingredients,
        IEnumerable<PreparationStep> preparationSteps)
    {
        _ingredients = ingredients.ToList();
        _preparationSteps = preparationSteps.ToList();
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; set; }
    public IReadOnlyCollection<Ingredient> Ingredients => _ingredients;
    public IReadOnlyCollection<PreparationStep> PreparationSteps => _preparationSteps;

    public void AddIngredient()
    {
        _ingredients.Add(Ingredient.New);
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        _ingredients.Remove(ingredient);
    }
}