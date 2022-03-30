﻿using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList
{
    public class AddItemWithTypeToShoppingListContract
    {
        public AddItemWithTypeToShoppingListContract(Guid? sectionId, float quantity)
        {
            SectionId = sectionId;
            Quantity = quantity;
        }

        public Guid? SectionId { get; set; }
        public float Quantity { get; set; }
    }
}