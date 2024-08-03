using System;

namespace ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Shared
{
    /// <summary>
    /// Represents a section of a store.
    /// </summary>
    public class SectionContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sortingIndex"></param>
        /// <param name="isDefaultSection"></param>
        public SectionContract(Guid id, string name, int sortingIndex, bool isDefaultSection)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
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
        /// The sorting index of the section.
        /// </summary>
        public int SortingIndex { get; }

        /// <summary>
        /// Whether the section is the store's default section.
        /// </summary>
        public bool IsDefaultSection { get; }
    }
}