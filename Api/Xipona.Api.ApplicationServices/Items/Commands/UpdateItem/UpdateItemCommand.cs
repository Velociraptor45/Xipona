using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Services.Updates;

namespace Xipona.Api.ApplicationServices.Items.Commands.UpdateItem;

public class UpdateItemCommand : ICommand<bool>
{
    public UpdateItemCommand(ItemUpdate itemUpdate)
    {
        ItemUpdate = itemUpdate;
    }

    public ItemUpdate ItemUpdate { get; }
}