﻿using ShoppingList.Domain.Models;
using System;

namespace ShoppingList.Domain.Commands.CreateShoppingList
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