using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get
{
    public class StoreItemSectionContract
    {
        public StoreItemSectionContract(Guid id, string name, int sortingIndex)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
        }

        public Guid Id { get; }
        public string Name { get; }
        public int SortingIndex { get; }
    }
}