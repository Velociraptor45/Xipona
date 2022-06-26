using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models
{
    public class ShoppingListStore
    {
        public ShoppingListStore(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }
}