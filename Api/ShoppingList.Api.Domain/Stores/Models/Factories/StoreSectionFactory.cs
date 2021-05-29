namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories
{
    public class StoreSectionFactory : IStoreSectionFactory
    {
        public IStoreSection Create(SectionId id, string name, int sortingIndex, bool isDefaultSection)
        {
            return new StoreSection(id, name, sortingIndex, isDefaultSection);
        }
    }
}