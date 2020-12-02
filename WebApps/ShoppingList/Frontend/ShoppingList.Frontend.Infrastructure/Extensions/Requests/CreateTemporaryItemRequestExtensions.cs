using ShoppingList.Api.Contracts.Commands.CreateTemporaryItem;
using ShoppingList.Api.Contracts.Commands.SharedContracts;
using ShoppingList.Frontend.Models.Shared.Requests;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Requests
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
                    Price = request.Price
                }
            };
        }
    }
}