namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models
{
    public class ItemCategory
    {
        public ItemCategory(ItemCategoryId id, string name, bool isDeleted)
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