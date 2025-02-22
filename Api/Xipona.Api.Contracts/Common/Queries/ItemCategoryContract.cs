using System;

namespace ProjectHermes.Xipona.Api.Contracts.Common.Queries
{
    /// <summary>
    /// Represents an item category.
    /// </summary>
    public class ItemCategoryContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="isDeleted"></param>
        public ItemCategoryContract(Guid id, string name, bool isDeleted)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
        }

        /// <summary>
        /// Item Category ID.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Item Category Name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Whether the item category is deleted. If deleted, true, otherwise false.
        /// </summary>
        public bool IsDeleted { get; }
    }
}