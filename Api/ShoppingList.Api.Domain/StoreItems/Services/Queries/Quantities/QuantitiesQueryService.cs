using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;

public class QuantitiesQueryService : IQuantitiesQueryService
{
    public IEnumerable<QuantityTypeReadModel> GetAllQuantityTypes()
    {
        var values = Enum.GetValues(typeof(QuantityType))
            .Cast<QuantityType>()
            .ToList();
        var readModels = values.Select(v => v.ToReadModel());

        return readModels;
    }

    public IEnumerable<QuantityTypeInPacketReadModel> GetAllQuantityTypesInPacket()
    {
        var values = Enum.GetValues(typeof(QuantityTypeInPacket))
            .Cast<QuantityTypeInPacket>()
            .ToList();
        var readModels = values.Select(v => v.ToReadModel());

        return readModels;
    }
}