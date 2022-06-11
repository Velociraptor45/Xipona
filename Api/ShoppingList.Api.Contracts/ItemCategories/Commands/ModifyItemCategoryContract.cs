using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Commands
{
    public class ModifyItemCategoryContract
    {
        public ModifyItemCategoryContract(Guid itemCategoryId, string name)
        {
            ItemCategoryId = itemCategoryId;
            Name = name;
        }

        public Guid ItemCategoryId { get; set; }
        public string Name { get; set; }
    }
}