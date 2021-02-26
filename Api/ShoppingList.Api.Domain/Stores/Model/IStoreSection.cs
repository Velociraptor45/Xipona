namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Model
{
    public interface IStoreSection
    {
        StoreSectionId Id { get; }
        string Name { get; }
        int SortingIndex { get; }
        bool IsDefaultSection { get; }
    }
}
