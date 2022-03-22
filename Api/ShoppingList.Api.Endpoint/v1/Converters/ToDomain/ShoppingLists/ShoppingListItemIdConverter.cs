using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.ShoppingLists;

public class ShoppingListItemIdConverter : IToDomainConverter<ItemIdContract, OfflineTolerantItemId>
{
    public OfflineTolerantItemId ToDomain(ItemIdContract source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

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