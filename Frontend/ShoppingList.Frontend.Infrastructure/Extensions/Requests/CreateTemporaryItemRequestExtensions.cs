using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class CreateTemporaryItemRequestExtensions
    {
        public static CreateTemporaryItemContract ToContract(this CreateTemporaryItemRequest request)
        {
            return new CreateTemporaryItemContract
            {
                ClientSideId = request.OfflineId,
                Name = request.Name,
                Availability = new ItemAvailabilityContract
                {
                    StoreId = request.StoreId,
                    Price = request.Price,
                    DefaultSectionId = request.DefaultSectionId
                }
            };
        }
    }
}