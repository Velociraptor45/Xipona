namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Model.Factories
{
    public class StoreSectionFactory : IStoreSectionFactory
    {
        public IStoreSection Create(StoreSectionId id, string name, int sortingIndex, bool isDefaultSection)
        {
            return new StoreSection(id, name, sortingIndex, isDefaultSection);
        }
    }
}