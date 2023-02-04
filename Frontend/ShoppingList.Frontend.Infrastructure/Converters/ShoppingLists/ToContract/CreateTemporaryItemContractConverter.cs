using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class CreateTemporaryItemContractConverter :
        IToContractConverter<CreateTemporaryItemRequest, CreateTemporaryItemContract>
    {
        public CreateTemporaryItemContract ToContract(CreateTemporaryItemRequest source)
        {
            return new CreateTemporaryItemContract
            {
                ClientSideId = source.OfflineId,
                Name = source.Name,
                Availability = new ItemAvailabilityContract
                {
                    StoreId = source.StoreId,
                    Price = source.Price,
                    DefaultSectionId = source.DefaultSectionId
                }
            };
        }
    }
}