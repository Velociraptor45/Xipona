using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToContract
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