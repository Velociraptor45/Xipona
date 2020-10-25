﻿using System;

namespace ShoppingList.Domain.Exceptions
{
    public class ShoppingListNotFoundException : Exception
    {
        public ShoppingListNotFoundException(string message) : base(message)
        {
        }

        public ShoppingListNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}