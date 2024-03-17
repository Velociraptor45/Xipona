using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Modifications;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.ModifyItem;

public class ModifyItemCommand : ICommand<bool>
{
    public ModifyItemCommand(ItemModification itemModify)
    {
        ItemModify = itemModify;
    }

    public ItemModification ItemModify { get; }
}