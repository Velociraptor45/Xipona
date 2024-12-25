using System;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get
{
    /// <summary>
    /// Represents a side dish to a recipe.
    /// </summary>
    public class SideDishContract
    {
        /// <summary> 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public SideDishContract(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// The recipe ID of the side dish.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the side dish.
        /// </summary>
        public string Name { get; }
    }
}
