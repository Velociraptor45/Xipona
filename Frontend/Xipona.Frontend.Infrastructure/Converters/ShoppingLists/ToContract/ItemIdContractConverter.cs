using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class ItemIdContractConverter : IToContractConverter<ShoppingListItemId, ItemIdContract>
    {
        public ItemIdContract ToContract(ShoppingListItemId model)
        {
            return model.ActualId.HasValue ?
                ItemIdContract.FromActualId(model.ActualId) :
                ItemIdContract.FromOfflineId(model.OfflineId);
        }
    }
}