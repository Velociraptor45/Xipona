using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get
{
    /// <summary>
    /// Represents a recipe.
    /// </summary>
    public class RecipeContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="numberOfServings"></param>
        /// <param name="ingredients"></param>
        /// <param name="preparationSteps"></param>
        /// <param name="recipeTagIds"></param>
        /// <param name="sideDish"></param>
        public RecipeContract(Guid id, string name, int numberOfServings, IEnumerable<IngredientContract> ingredients,
            IEnumerable<PreparationStepContract> preparationSteps, IEnumerable<Guid> recipeTagIds, SideDishContract sideDish)
        {
            Id = id;
            Name = name;
            NumberOfServings = numberOfServings;
            Ingredients = ingredients;
            PreparationSteps = preparationSteps;
            RecipeTagIds = recipeTagIds;
            SideDish = sideDish;
        }

        /// <summary>
        /// The ID of the recipe.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the recipe.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The number of servings the recipe is for.
        /// </summary>
        public int NumberOfServings { get; }

        /// <summary>
        /// The ingredients of the recipe.
        /// </summary>
        public IEnumerable<IngredientContract> Ingredients { get; }

        /// <summary>
        /// The preparation steps of the recipe.
        /// </summary>
        public IEnumerable<PreparationStepContract> PreparationSteps { get; }

        /// <summary>
        /// The IDs of the recipe's tags.
        /// </summary>
        public IEnumerable<Guid> RecipeTagIds { get; }

        /// <summary>
        /// The side dish of the recipe. <c>Null</c> if the recipe has no side dish.
        /// </summary>
        public SideDishContract SideDish { get; }
    }
}