using System;

namespace ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping
{
    /// <summary>
    /// Represents a store's section.
    /// </summary>
    public class SectionForShoppingContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="isDefaultSection"></param>
        /// <param name="sortingIndex"></param>
        public SectionForShoppingContract(Guid id, string name, bool isDefaultSection, int sortingIndex)
        {
            Id = id;
            Name = name;
            IsDefaultSection = isDefaultSection;
            SortingIndex = sortingIndex;
        }

        /// <summary>
        /// The ID of the section.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the section.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Whether the section is the store's default section.
        /// </summary>
        public bool IsDefaultSection { get; }

        /// <summary>
        /// The sorting index of the section.
        /// </summary>
        public int SortingIndex { get; }
    }
}