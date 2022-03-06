using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ItemCategory.Commands
{
    public class DeleteItemCategoryContract
    {
        public Guid ItemCategoryId { get; set; }
    }
}