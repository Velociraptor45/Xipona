using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.ModifyRecipe
{
    /// <summary>
    /// Represents the contract for modifying a recipe.
    /// </summary>
    public class ModifyRecipeContract
    {
        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="numberOfServings"></param>
        /// <param name="ingredients"></param>
        /// <param name="preparationSteps"></param>
        /// <param name="recipeTagIds"></param>
        public ModifyRecipeContract(string name, int numberOfServings, IEnumerable<ModifyIngredientContract> ingredients,
            IEnumerable<ModifyPreparationStepContract> preparationSteps, IEnumerable<Guid> recipeTagIds)
        {
            Name = name;
            NumberOfServings = numberOfServings;
            Ingredients = ingredients;
            PreparationSteps = preparationSteps;
            RecipeTagIds = recipeTagIds;
        }

        /// <summary>
        /// The name of the recipe.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The number of servings the recipe is for.
        /// </summary>
        public int NumberOfServings { get; }

        /// <summary>
        /// The ingredients of the recipe.
        /// </summary>
        public IEnumerable<ModifyIngredientContract> Ingredients { get; set; }

        /// <summary>
        /// The preparation steps of the recipe.
        /// </summary>
        public IEnumerable<ModifyPreparationStepContract> PreparationSteps { get; set; }

        /// <summary>
        /// The IDs of the recipe's tags.
        /// </summary>
        public IEnumerable<Guid> RecipeTagIds { get; set; }
    }
}