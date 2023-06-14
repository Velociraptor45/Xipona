namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

public record ActiveStores(IReadOnlyCollection<ItemStore> Stores)
{
    public IReadOnlyCollection<ItemStoreSection> GetSections(Guid StoreId)
    {
        return Stores.FirstOrDefault(s => s.Id == StoreId)?.Sections ?? new List<ItemStoreSection>();
    }
}