using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Endpoint.Extensions.ItemCategory;
using ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Manufacturer;
using ProjectHermes.ShoppingList.Api.Endpoint.Extensions.ShoppingList;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class StoreItemReadModelExtensions
    {
        public static StoreItemContract ToContract(this StoreItemReadModel readModel)
        {
            return new StoreItemContract(
                readModel.Id.Value,
                readModel.Name,
                readModel.IsDeleted,
                readModel.Comment,
                readModel.IsTemporary,
                readModel.QuantityType.ToContract(),
                readModel.QuantityInPacket,
                readModel.QuantityTypeInPacket.ToContract(),
                readModel.ItemCategory?.ToContract(),
                readModel.Manufacturer?.ToContract(),
                readModel.Availabilities.Select(av => av.ToContract()));
        }
    }
}