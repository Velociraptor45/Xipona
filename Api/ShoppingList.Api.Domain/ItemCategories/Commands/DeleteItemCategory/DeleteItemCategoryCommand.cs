using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.DeleteItemCategory
{
    public class DeleteItemCategoryCommand : ICommand<bool>
    {
        public DeleteItemCategoryCommand(ItemCategoryId itemCategoryId)
        {
            ItemCategoryId = itemCategoryId;
        }

        public ItemCategoryId ItemCategoryId { get; }
    }
}