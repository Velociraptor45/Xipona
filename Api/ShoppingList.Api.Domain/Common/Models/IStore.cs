namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models
{
    public interface IStore
    {
        StoreId Id { get; }
        bool IsDeleted { get; }
        string Name { get; }

        void ChangeName(string name);
    }
}