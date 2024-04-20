using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Updates;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.ItemUpdateWithTypes;

public class UpdateItemWithTypesCommand : ICommand<bool>
{
    public UpdateItemWithTypesCommand(ItemWithTypesUpdate itemWithTypesUpdate)
    {
        ItemWithTypesUpdate = itemWithTypesUpdate;
    }

    public ItemWithTypesUpdate ItemWithTypesUpdate { get; }
}