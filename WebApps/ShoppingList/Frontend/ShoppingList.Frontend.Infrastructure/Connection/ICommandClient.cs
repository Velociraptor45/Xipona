﻿using ShoppingList.Frontend.Models.Shared.Requests;
using System.Threading.Tasks;

namespace ShoppingList.Frontend.Infrastructure.Connection
{
    public interface ICommandClient
    {
        Task ChangeItemQuantityOnShoppingListAsync(ChangeItemQuantityOnShoppingListRequest request);

        Task FinishListRequestAsync(FinishListRequest request);

        Task IsAliveAsync();

        Task PutItemInBasketAsync(PutItemInBasketRequest request);

        Task RemoveItemFromBasketAsync(RemoveItemFromBasketRequest request);
    }
}