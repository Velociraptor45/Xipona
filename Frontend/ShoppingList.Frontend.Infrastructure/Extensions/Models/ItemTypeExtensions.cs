using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Models
{
    public static class ItemTypeExtensions
    {
        public static CreateItemTypeContract ToCreateItemTypeContract(this ItemType itemType)
        {
            return new CreateItemTypeContract()
            {
                Name = itemType.Name,
                Availabilities = itemType.Availabilities.Select(av => av.ToItemAvailabilityContract())
            };
        }

        public static ItemTypeContract ToItemTypeContract(this ItemType itemType)
        {
            return new ItemTypeContract()
            {
                Id = itemType.Id,
                Name = itemType.Name,
                Availabilities = itemType.Availabilities.Select(av => av.ToItemAvailabilityContract())
            };
        }

        public static UpdateItemTypeContract ToUpdateItemTypeContract(this ItemType itemType)
        {
            return new UpdateItemTypeContract()
            {
                OldId = itemType.Id,
                Name = itemType.Name,
                Availabilities = itemType.Availabilities.Select(av => av.ToItemAvailabilityContract())
            };
        }
    }
}