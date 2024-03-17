using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Shared;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.ShoppingLists;

public class ShoppingListItemIdConverter : IToDomainConverter<ItemIdContract, OfflineTolerantItemId>
{
    public OfflineTolerantItemId ToDomain(ItemIdContract source)
    {
        if (source.Actual != null)
        {
            return OfflineTolerantItemId.FromActualId(source.Actual.Value);
        }

        if (source.Offline != null)
        {
            return OfflineTolerantItemId.FromOfflineId(source.Offline.Value);
        }

        throw new ArgumentException($"All values in {nameof(ItemIdContract)} are null.");
    }
}