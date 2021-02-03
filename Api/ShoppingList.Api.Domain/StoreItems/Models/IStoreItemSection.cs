namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public interface IStoreItemSection
    {
        StoreItemSectionId Id { get; }
        string Name { get; }
        int SortIndex { get; }
    }
}
