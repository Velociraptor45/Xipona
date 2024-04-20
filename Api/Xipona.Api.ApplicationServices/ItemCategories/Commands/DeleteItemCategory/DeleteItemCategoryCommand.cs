using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;

public class DeleteItemCategoryCommand : ICommand<bool>
{
    public DeleteItemCategoryCommand(ItemCategoryId itemCategoryId)
    {
        ItemCategoryId = itemCategoryId;
    }

    public ItemCategoryId ItemCategoryId { get; }
}