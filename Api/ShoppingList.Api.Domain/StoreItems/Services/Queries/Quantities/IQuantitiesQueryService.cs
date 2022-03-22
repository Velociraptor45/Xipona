namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;

public interface IQuantitiesQueryService
{
    IEnumerable<QuantityTypeReadModel> GetAllQuantityTypes();

    IEnumerable<QuantityTypeInPacketReadModel> GetAllQuantityTypesInPacket();
}