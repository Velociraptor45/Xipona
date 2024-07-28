namespace ProjectHermes.Xipona.Api.Contracts.Stores.Commands.CreateStore
{
    /// <summary>
    /// Represents a request for creating a store's section.
    /// </summary>
    public class CreateSectionContract
    {
        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sortingIndex"></param>
        /// <param name="isDefaultSection"></param>
        public CreateSectionContract(string name, int sortingIndex, bool isDefaultSection)
        {
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
        }

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