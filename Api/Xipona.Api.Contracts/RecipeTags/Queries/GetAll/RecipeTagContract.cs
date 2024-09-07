using System;

namespace ProjectHermes.Xipona.Api.Contracts.RecipeTags.Queries.GetAll
{
    /// <summary>
    /// Represents a recipe tag.
    /// </summary>
    public class RecipeTagContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public RecipeTagContract(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// The ID of the recipe tag.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the recipe tag.
        /// </summary>
        public string Name { get; }
    }
}