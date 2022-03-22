using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class ItemAvailabilityContractConverter :
        IToContractConverter<StoreItemAvailability, ItemAvailabilityContract>
    {
        public ItemAvailabilityContract ToContract(StoreItemAvailability model)
        {
            return new ItemAvailabilityContract()
            {
                StoreId = model.Store.Id,
                Price = model.PricePerQuantity,
                DefaultSectionId = model.DefaultSection.Id
            };
        }
    }
}