using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ItemCategories.Models;

namespace Xipona.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;

public class DeleteItemCategoryCommand : ICommand<bool>
{
    public DeleteItemCategoryCommand(ItemCategoryId itemCategoryId)
    {
        ItemCategoryId = itemCategoryId;
    }

    public ItemCategoryId ItemCategoryId { get; }
}