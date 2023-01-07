using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToContract
{
    public class ItemAvailabilityContractConverter :
        IToContractConverter<EditedItemAvailability, ItemAvailabilityContract>
    {
        public ItemAvailabilityContract ToContract(EditedItemAvailability model)
        {
            return new ItemAvailabilityContract()
            {
                StoreId = model.StoreId,
                Price = model.PricePerQuantity,
                DefaultSectionId = model.DefaultSectionId
            };
        }
    }
}