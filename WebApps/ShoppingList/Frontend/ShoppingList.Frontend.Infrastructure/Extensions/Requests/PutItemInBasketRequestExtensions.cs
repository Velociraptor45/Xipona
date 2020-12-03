﻿using ShoppingList.Api.Contracts.Commands.PutItemInBasket;
using ShoppingList.Frontend.Infrastructure.Extensions.Models;
using ShoppingList.Frontend.Models.Shared.Requests;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Requests
{
    public static class PutItemInBasketRequestExtensions
    {
        public static PutItemInBasketContract ToContract(this PutItemInBasketRequest request)
        {
            return new PutItemInBasketContract()
            {
                ShopingListId = request.ShoppingListId,
                ItemId = request.ItemId.ToContract()
            };
        }
    }
}