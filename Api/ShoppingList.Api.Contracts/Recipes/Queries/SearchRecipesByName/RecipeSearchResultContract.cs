using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.SearchRecipesByName
{
    public class RecipeSearchResultContract
    {
        public RecipeSearchResultContract(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}