using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels
{
    public class ItemCategoryReadModel
    {
        public ItemCategoryReadModel(ItemCategoryId id, string name, bool isDeleted)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
        }

        public ItemCategoryId Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
    }
}