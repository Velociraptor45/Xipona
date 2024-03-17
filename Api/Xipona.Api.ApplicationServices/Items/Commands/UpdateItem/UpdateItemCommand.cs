using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Updates;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.UpdateItem;

public class UpdateItemCommand : ICommand<bool>
{
    public UpdateItemCommand(ItemUpdate itemUpdate)
    {
        ItemUpdate = itemUpdate;
    }

    public ItemUpdate ItemUpdate { get; }
}