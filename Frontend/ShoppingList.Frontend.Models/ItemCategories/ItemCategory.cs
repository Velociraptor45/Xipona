using System;

namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class ItemCategory
    {
        public ItemCategory(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}