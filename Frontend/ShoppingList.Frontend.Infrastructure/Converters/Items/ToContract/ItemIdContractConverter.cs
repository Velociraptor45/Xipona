using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Shared;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class ItemIdContractConverter : IToContractConverter<ItemId, ItemIdContract>
    {
        public ItemIdContract ToContract(ItemId model)
        {
            return model.ActualId.HasValue ?
                new ItemIdContract { Actual = model.ActualId } :
                new ItemIdContract { Offline = model.OfflineId };
        }
    }
}