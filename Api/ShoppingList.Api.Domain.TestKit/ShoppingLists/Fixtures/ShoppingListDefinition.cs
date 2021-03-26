using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListDefinition
    {
        private DateTime? completionDate;

        public ShoppingListId Id { get; set; }
        public StoreId StoreId { get; set; }
        public IEnumerable<IShoppingListSection> Sections { get; set; }

        public DateTime? CompletionDate
        {
            get => completionDate;
            set
            {
                completionDate = value;
                UseCompletionDate = true;
            }
        }

        public bool UseCompletionDate { get; private set; } = false;

        public ShoppingListDefinition Clone()
        {
            return new ShoppingListDefinition
            {
                Id = Id,
                StoreId = StoreId,
                Sections = Sections,
                CompletionDate = CompletionDate
            };
        }

        public static ShoppingListDefinition FromId(int id)
        {
            return FromId(new ShoppingListId(id));
        }

        public static ShoppingListDefinition FromId(ShoppingListId id)
        {
            return new ShoppingListDefinition
            {
                Id = id
            };
        }
    }
}