using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class ItemAvailabilityContractConverter :
        IToContractConverter<ItemAvailability, ItemAvailabilityContract>
    {
        public ItemAvailabilityContract ToContract(ItemAvailability model)
        {
            return new ItemAvailabilityContract()
            {
                StoreId = model.Store.Id,
                Price = model.PricePerQuantity,
                DefaultSectionId = model.DefaultSectionId
            };
        }
    }
}