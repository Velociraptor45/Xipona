using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.DeleteItem;

public class DeleteItemCommand : ICommand<bool>
{
    public DeleteItemCommand(ItemId itemId)
    {
        ItemId = itemId;
    }

    public ItemId ItemId { get; }
}