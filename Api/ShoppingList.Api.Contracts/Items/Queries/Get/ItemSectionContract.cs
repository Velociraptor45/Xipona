using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get
{
    public class ItemSectionContract
    {
        public ItemSectionContract(Guid id, string name, int sortingIndex)
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