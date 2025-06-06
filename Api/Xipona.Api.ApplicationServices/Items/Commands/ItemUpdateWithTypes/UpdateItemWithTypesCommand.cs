using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Services.Updates;

namespace Xipona.Api.ApplicationServices.Items.Commands.ItemUpdateWithTypes;

public class UpdateItemWithTypesCommand : ICommand<bool>
{
    public UpdateItemWithTypesCommand(ItemWithTypesUpdate itemWithTypesUpdate)
    {
        ItemWithTypesUpdate = itemWithTypesUpdate;
    }

    public ItemWithTypesUpdate ItemWithTypesUpdate { get; }
}