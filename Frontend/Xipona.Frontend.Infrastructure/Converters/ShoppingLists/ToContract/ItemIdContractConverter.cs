using Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;

public class ItemIdContractConverter : IToContractConverter<ShoppingListItemId, ItemIdContract>
{
    public ItemIdContract ToContract(ShoppingListItemId model)
    {
        return model.ActualId.HasValue ?
            ItemIdContract.FromActualId(model.ActualId) :
            ItemIdContract.FromOfflineId(model.OfflineId);
    }
}