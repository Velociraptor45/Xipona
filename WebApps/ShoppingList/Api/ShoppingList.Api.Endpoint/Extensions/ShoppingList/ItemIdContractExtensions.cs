using ShoppingList.Api.Contracts.Commands.Shared;
using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Endpoint.Extensions.ShoppingList
{
    public static class ItemIdContractExtensions
    {
        public static ShoppingListItemId ToShoppingListItemId(this ItemIdContract contract)
        {
            if (contract.Actual != null)
            {
                return new ShoppingListItemId(contract.Actual.Value);
            }
            else if (contract.Offline != null)
            {
                return new ShoppingListItemId(contract.Offline.Value);
            }

            throw new ArgumentException($"All values in {nameof(ItemIdContract)} are null.");
        }
    }
}