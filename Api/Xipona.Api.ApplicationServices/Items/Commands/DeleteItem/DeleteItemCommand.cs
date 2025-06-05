using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.ApplicationServices.Items.Commands.DeleteItem;

public class DeleteItemCommand : ICommand<bool>
{
    public DeleteItemCommand(ItemId itemId)
    {
        ItemId = itemId;
    }

    public ItemId ItemId { get; }
}