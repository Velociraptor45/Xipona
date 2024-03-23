using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get
{
    public class RecipeContract
    {
        public RecipeContract(Guid id, string name, int numberOfServings, IEnumerable<IngredientContract> ingredients,
            IEnumerable<PreparationStepContract> preparationSteps, IEnumerable<Guid> recipeTagIds)
        {
            Id = id;
            Name = name;
            NumberOfServings = numberOfServings;
            Ingredients = ingredients;
            PreparationSteps = preparationSteps;
            RecipeTagIds = recipeTagIds;
        }

        public Guid Id { get; }
        public string Name { get; }
        public int NumberOfServings { get; }
        public IEnumerable<IngredientContract> Ingredients { get; }
        public IEnumerable<PreparationStepContract> PreparationSteps { get; }
        public IEnumerable<Guid> RecipeTagIds { get; }
    }
}