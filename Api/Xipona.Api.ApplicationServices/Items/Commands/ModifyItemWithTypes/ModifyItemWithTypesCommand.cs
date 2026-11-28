using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Services.Modifications;

namespace Xipona.Api.ApplicationServices.Items.Commands.ModifyItemWithTypes;

public class ModifyItemWithTypesCommand : ICommand<bool>
{
    public ModifyItemWithTypesCommand(ItemWithTypesModification itemWithTypesModification)
    {
        ItemWithTypesModification = itemWithTypesModification;
    }

    public ItemWithTypesModification ItemWithTypesModification { get; }
}