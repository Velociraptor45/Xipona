using System;

namespace ProjectHermes.Xipona.Api.Contracts.ItemCategories.Commands
{
    /// <summary>
    /// Represents a command to modify an item category.
    /// </summary>
    public class ModifyItemCategoryContract
    {
        /// <summary>
        /// </summary>
        /// <param name="itemCategoryId"></param>
        /// <param name="name"></param>
        public ModifyItemCategoryContract(Guid itemCategoryId, string name)
        {
            ItemCategoryId = itemCategoryId;
            Name = name;
        }

        /// <summary>
        /// ID of the Item Category that should be modified.
        /// </summary>
        public Guid ItemCategoryId { get; set; }

        /// <summary>
        /// New name of the Item Category.
        /// </summary>
        public string Name { get; set; }
    }
}