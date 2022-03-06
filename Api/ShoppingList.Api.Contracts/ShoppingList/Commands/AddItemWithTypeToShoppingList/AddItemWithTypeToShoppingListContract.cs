﻿using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList
{
    public class AddItemWithTypeToShoppingListContract
    {
        public int ShoppingListId { get; set; }
        public Guid ItemId { get; set; }
        public Guid ItemTypeId { get; set; }
        public int? SectionId { get; set; }
        public float Quantity { get; set; }
    }
}