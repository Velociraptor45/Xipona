using System;

namespace ProjectHermes.Xipona.Api.Contracts.Stores.Commands.ModifyStore
{
    /// <summary>
    /// Represents a contract for modifying a store's section.
    /// </summary>
    public class ModifySectionContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sortingIndex"></param>
        /// <param name="isDefaultSection"></param>
        public ModifySectionContract(Guid? id, string name, int sortingIndex, bool isDefaultSection)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
        }

        /// <summary>
        /// The ID of the section.
        /// Null if the section is new.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// The name of the section.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The sorting index of the section.
        /// </summary>
        public int SortingIndex { get; set; }

        /// <summary>
        /// Whether the section is the store's default section.
        /// </summary>
        public bool IsDefaultSection { get; set; }
    }
}