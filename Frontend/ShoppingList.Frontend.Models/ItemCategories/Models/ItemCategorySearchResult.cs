using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models
{
    public class ItemCategorySearchResult
    {
        public ItemCategorySearchResult(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; private set; }

        public void ChangeName(string name)
        {
            Name = name;
        }
    }
}