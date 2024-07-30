using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.CreateRecipe
{
    /// <summary>
    /// Represents the contract for creating a recipe.
    /// </summary>
    public class CreateRecipeContract
    {
        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="numberOfServings"></param>
        /// <param name="ingredients"></param>
        /// <param name="preparationSteps"></param>
        /// <param name="recipeTagIds"></param>
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

        /// <summary>
        /// The name of the recipe.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The number of servings the recipe is for.
        /// </summary>
        public int NumberOfServings { get; set; }

        /// <summary>
        /// The ingredients of the recipe.
        /// </summary>
        public IEnumerable<CreateIngredientContract> Ingredients { get; set; }

        /// <summary>
        /// The preparation steps of the recipe.
        /// </summary>
        public IEnumerable<CreatePreparationStepContract> PreparationSteps { get; set; }

        /// <summary>
        /// The IDs of the recipe's tags.
        /// </summary>
        public IEnumerable<Guid> RecipeTagIds { get; set; }
    }
}