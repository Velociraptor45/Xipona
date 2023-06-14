using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeContract
    {
        public CreateRecipeContract(string name, int numberOfServings,
            IEnumerable<CreateIngredientContract> ingredients,
            IEnumerable<CreatePreparationStepContract> preparationSteps, IEnumerable<Guid> recipeTagIds)
        {
            Name = name;
            NumberOfServings = numberOfServings;
            Ingredients = ingredients;
            PreparationSteps = preparationSteps;
            RecipeTagIds = recipeTagIds;
        }

        public string Name { get; set; }
        public int NumberOfServings { get; set; }
        public IEnumerable<CreateIngredientContract> Ingredients { get; set; }
        public IEnumerable<CreatePreparationStepContract> PreparationSteps { get; set; }
        public IEnumerable<Guid> RecipeTagIds { get; set; }
    }
}