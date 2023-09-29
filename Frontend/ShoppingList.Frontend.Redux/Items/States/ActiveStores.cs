namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

public record ActiveStores(IReadOnlyCollection<ItemStore> Stores)
{
    public IReadOnlyCollection<ItemStoreSection> GetSections(Guid storeId)
    {
        return Stores
                   .FirstOrDefault(s => s.Id == storeId)?
                   .Sections
                   .OrderBy(s => s.SortingIndex)
                   .ToList() 
               ?? new List<ItemStoreSection>();
    }
}