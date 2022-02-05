using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;

public class MakeTemporaryItemPermanentCommand : ICommand<bool>
{
    public MakeTemporaryItemPermanentCommand(PermanentItem permanentItem)
    {
        PermanentItem = permanentItem ?? throw new ArgumentNullException(nameof(permanentItem));
    }

    public PermanentItem PermanentItem { get; }
}