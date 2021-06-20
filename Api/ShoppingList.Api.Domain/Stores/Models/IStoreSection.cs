namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models
{
    public interface IStoreSection
    {
        SectionId Id { get; }
        string Name { get; }
        int SortingIndex { get; }
        bool IsDefaultSection { get; }
    }
}
