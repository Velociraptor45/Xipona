﻿using System;

namespace ShoppingList.Domain.Exceptions
{
    public class ItemAlreadyOnShoppingListException : Exception
    {
        public ItemAlreadyOnShoppingListException()
        {
        }

        public ItemAlreadyOnShoppingListException(string message) : base(message)
        {
        }

        public ItemAlreadyOnShoppingListException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}