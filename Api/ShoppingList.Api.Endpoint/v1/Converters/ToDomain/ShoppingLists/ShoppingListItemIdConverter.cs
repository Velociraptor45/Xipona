using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.ShoppingLists
{
    public class ShoppingListItemIdConverter : IToDomainConverter<ItemIdContract, ItemId>
    {
        public ItemId ToDomain(ItemIdContract source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (source.Actual != null)
            {
                return new ItemId(source.Actual.Value);
            }

            if (source.Offline != null)
            {
                return new ShoppingListItemId(source.Offline.Value);
            }

            throw new ArgumentException($"All values in {nameof(ItemIdContract)} are null.");
        }
    }
}