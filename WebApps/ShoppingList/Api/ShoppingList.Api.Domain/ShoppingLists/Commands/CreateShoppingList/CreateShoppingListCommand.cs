using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.CreateShoppingList
{
    public class CreateShoppingListCommand : ICommand<bool>
    {
        public CreateShoppingListCommand(StoreId storeId)
        {
            StoreId = storeId ?? throw new ArgumentNullException(nameof(storeId));
        }

        public StoreId StoreId { get; }
    }
}