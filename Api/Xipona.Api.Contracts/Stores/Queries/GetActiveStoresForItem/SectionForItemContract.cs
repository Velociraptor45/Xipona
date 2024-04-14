using System;

namespace ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem
{
    public class SectionForItemContract
    {
        public SectionForItemContract(Guid id, string name, bool isDefaultSection, int sortingIndex)
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