using System;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName
{
    /// <summary>
    /// Represents a recipe search result.
    /// </summary>
    public class RecipeSearchResultContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public RecipeSearchResultContract(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// The ID of the recipe.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the recipe.
        /// </summary>
        public string Name { get; set; }
    }
}