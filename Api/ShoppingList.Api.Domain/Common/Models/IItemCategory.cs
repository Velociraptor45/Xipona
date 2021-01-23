namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models
{
    public interface IItemCategory
    {
        ItemCategoryId Id { get; }
        string Name { get; }
        bool IsDeleted { get; }

        void Delete();
    }
}