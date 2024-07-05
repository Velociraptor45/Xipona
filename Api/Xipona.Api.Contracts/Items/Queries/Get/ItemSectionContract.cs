using System;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get
{
    /// <summary>
    /// Represents a section where an item is located in a store.
    /// </summary>
    public class ItemSectionContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sortingIndex"></param>
        public ItemSectionContract(Guid id, string name, int sortingIndex)
        {
            Id = id;
            Name = name;
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
        /// The sorting index of the section. Indicates the order of the sections in a store.
        /// </summary>
        public int SortingIndex { get; }
    }
}