using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.TemporaryItems;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.MakeTemporaryItemPermanent;

public class MakeTemporaryItemPermanentCommand : ICommand<bool>
{
    public MakeTemporaryItemPermanentCommand(PermanentItem permanentItem)
    {
        PermanentItem = permanentItem ?? throw new ArgumentNullException(nameof(permanentItem));
    }

    public PermanentItem PermanentItem { get; }
}