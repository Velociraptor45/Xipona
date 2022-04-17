using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models
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