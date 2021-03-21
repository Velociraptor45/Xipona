namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Model.Factories
{
    public interface IStoreSectionFactory
    {
        IStoreSection Create(StoreSectionId id, string name, int sortingIndex, bool isDefaultSection);
    }
}