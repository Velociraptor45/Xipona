namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models
{
    public interface IItemCategory
    {
        ItemCategoryId Id { get; }
        string Name { get; }
        bool IsDeleted { get; }

        void Delete();
    }
}