using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.ModifyRecipe
{
    public class ModifyRecipeContract
    {
        public ModifyRecipeContract(string name, IEnumerable<ModifyIngredientContract> ingredients,
            IEnumerable<ModifyPreparationStepContract> preparationSteps, IEnumerable<Guid> recipeTagIds)
        {
            Name = name;
            Ingredients = ingredients;
            PreparationSteps = preparationSteps;
            RecipeTagIds = recipeTagIds;
        }

        public string Name { get; set; }
        public IEnumerable<ModifyIngredientContract> Ingredients { get; set; }
        public IEnumerable<ModifyPreparationStepContract> PreparationSteps { get; set; }
        public IEnumerable<Guid> RecipeTagIds { get; set; }
    }
}