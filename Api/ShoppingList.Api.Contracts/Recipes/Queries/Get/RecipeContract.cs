using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get
{
    public class RecipeContract
    {
        public RecipeContract(Guid id, string name, IEnumerable<IngredientContract> ingredients,
            IEnumerable<PreparationStepContract> preparationSteps)
        {
            Id = id;
            Name = name;
            Ingredients = ingredients;
            PreparationSteps = preparationSteps;
        }

        public Guid Id { get; }
        public string Name { get; }
        public IEnumerable<IngredientContract> Ingredients { get; }
        public IEnumerable<PreparationStepContract> PreparationSteps { get; }
    }
}