using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
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