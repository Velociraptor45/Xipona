using System;

namespace ProjectHermes.Xipona.Api.Contracts.ItemCategories.Queries
{
    /// <summary>
    /// Represents an item category search result.
    /// </summary>
    public class ItemCategorySearchResultContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public ItemCategorySearchResultContract(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Item Category ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Item Category Name.
        /// </summary>
        public string Name { get; set; }
    }
}