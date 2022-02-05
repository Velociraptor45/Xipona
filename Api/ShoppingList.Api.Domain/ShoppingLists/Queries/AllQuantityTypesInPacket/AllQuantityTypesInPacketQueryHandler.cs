using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypesInPacket;

public class AllQuantityTypesInPacketQueryHandler : IQueryHandler<AllQuantityTypesInPacketQuery, IEnumerable<QuantityTypeInPacketReadModel>>
{
    public Task<IEnumerable<QuantityTypeInPacketReadModel>> HandleAsync(AllQuantityTypesInPacketQuery query, CancellationToken cancellationToken)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var values = Enum.GetValues(typeof(QuantityTypeInPacket))
            .Cast<QuantityTypeInPacket>()
            .ToList();
        var readModels = values.Select(v => v.ToReadModel());

        return Task.FromResult(readModels);
    }
}