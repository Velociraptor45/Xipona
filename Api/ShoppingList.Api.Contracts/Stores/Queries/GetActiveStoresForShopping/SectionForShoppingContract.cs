using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForShopping
{
    public class SectionForShoppingContract
    {
        public SectionForShoppingContract(Guid id, string name, bool isDefaultSection, int sortingIndex)
        {
            Id = id;
            Name = name;
            IsDefaultSection = isDefaultSection;
            SortingIndex = sortingIndex;
        }

        public Guid Id { get; }
        public string Name { get; }
        public bool IsDefaultSection { get; }
        public int SortingIndex { get; }
    }
}