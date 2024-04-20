using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Services.TemporaryItems;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.MakeTemporaryItemPermanent;

public class MakeTemporaryItemPermanentCommand : ICommand<bool>
{
    public MakeTemporaryItemPermanentCommand(PermanentItem permanentItem)
    {
        PermanentItem = permanentItem;
    }

    public PermanentItem PermanentItem { get; }
}