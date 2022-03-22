using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores
{
    public class StoreSectionContract
    {
        public StoreSectionContract(Guid id, string name, int sortingIndex, bool isDefaultSection)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
        }

        public Guid Id { get; }
        public string Name { get; }
        public int SortingIndex { get; }
        public bool IsDefaultSection { get; }
    }
}