using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Services.TemporaryItems;

namespace Xipona.Api.ApplicationServices.Items.Commands.MakeTemporaryItemPermanent;

public class MakeTemporaryItemPermanentCommand : ICommand<bool>
{
    public MakeTemporaryItemPermanentCommand(PermanentItem permanentItem)
    {
        PermanentItem = permanentItem;
    }

    public PermanentItem PermanentItem { get; }
}