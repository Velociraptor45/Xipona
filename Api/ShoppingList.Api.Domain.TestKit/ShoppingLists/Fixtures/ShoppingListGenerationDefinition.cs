using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures
{
    public class ShoppingListGenerationDefinition
    {
        private DateTime? completionDate;

        public ShoppingListId Id { get; set; }
        public IShoppingListStore Store { get; set; }
        public IEnumerable<IShoppingListSection> Sections { get; set; }
        public IEnumerable<ShoppingListSectionGenerationDefinition> SectionDefinitions { get; set; }

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
    }
}