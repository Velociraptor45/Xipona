using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.RecipeTags.Queries.GetAll
{
    public class RecipeTagContract
    {
        public RecipeTagContract(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }
}