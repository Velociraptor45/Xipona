using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Comparer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;

public class Recipe : ISortable<PreparationStep>
{
    private readonly List<Ingredient> _ingredients;
    private SortedSet<PreparationStep> _preparationSteps;

    public Recipe(Guid id, string name, IEnumerable<Ingredient> ingredients,
        IEnumerable<PreparationStep> preparationSteps)
    {
        _ingredients = ingredients.ToList();
        _preparationSteps = new SortedSet<PreparationStep>(preparationSteps, new SortingIndexComparer());
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; set; }
    public IReadOnlyCollection<Ingredient> Ingredients => _ingredients;
    public IReadOnlyCollection<PreparationStep> PreparationSteps => _preparationSteps;
    public int MinSortingIndex => _preparationSteps.Any() ? _preparationSteps.Min(s => s.SortingIndex) : 0;
    public int MaxSortingIndex => _preparationSteps.Any() ? _preparationSteps.Max(s => s.SortingIndex) : 0;

    //public void AddIngredient()
    //{
    //    _ingredients.Add(Ingredient.New);
    //}

    //public void RemoveIngredient(Ingredient ingredient)
    //{
    //    _ingredients.Remove(ingredient);
    //}

    //public void AddPreparationStep()
    //{
    //    var nextSortingIndex = MaxSortingIndex + 1;

    //    _preparationSteps.Add(new PreparationStep(Guid.Empty, "", nextSortingIndex));
    //}

    //public void Remove(PreparationStep preparationStep)
    //{
    //    _preparationSteps.Remove(preparationStep);
    //}

    //public void Increment(PreparationStep model)
    //{
    //    var steps = _preparationSteps.ToList();

    //    int sectionIndex = steps.IndexOf(model);
    //    if (sectionIndex == -1 || sectionIndex >= steps.Count - 1)
    //        return;

    //    var tmp = steps[sectionIndex + 1];
    //    steps[sectionIndex + 1] = model;
    //    steps[sectionIndex] = tmp;

    //    UpdateSortingIndexes(steps);

    //    _preparationSteps = new SortedSet<PreparationStep>(steps, new SortingIndexComparer());
    //}

    //public void Decrement(PreparationStep model)
    //{
    //    var steps = _preparationSteps.ToList();

    //    int sectionIndex = steps.IndexOf(model);
    //    if (sectionIndex is -1 or <= 0)
    //        return;

    //    var tmp = steps[sectionIndex - 1];
    //    steps[sectionIndex - 1] = model;
    //    steps[sectionIndex] = tmp;

    //    UpdateSortingIndexes(steps);

    //    _preparationSteps = new SortedSet<PreparationStep>(steps, new SortingIndexComparer());
    //}

    //private static void UpdateSortingIndexes(IList<PreparationStep> steps)
    //{
    //    for (int i = 0; i < steps.Count; i++)
    //    {
    //        steps[i].SetSortingIndex(i);
    //    }
    //}
}