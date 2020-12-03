using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ShoppingList.Frontend.Infrastructure.Extensions.Models;
using ShoppingList.Frontend.Models.Shared.Requests;
using System.Linq;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class MakeTemporaryItemPermanentRequestExtensions
    {
        public static MakeTemporaryItemPermanentContract ToContract(this MakeTemporaryItemPermanentRequest request)
        {
            return new MakeTemporaryItemPermanentContract
            {
                Id = request.Id,
                Name = request.Name,
                Comment = request.Comment,
                QuantityType = request.QuantityType,
                QuantityInPacket = request.QuantityInPacket,
                QuantityTypeInPacket = request.QuantityTypeInPacket,
                ItemCategoryId = request.ItemCategoryId,
                ManufacturerId = request.ManufacturerId,
                Availabilities = request.Availabilities.Select(av => av.ToItemAvailabilityContract())
            };
        }
    }
}