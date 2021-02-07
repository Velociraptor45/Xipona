using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures
{
    public class ShoppingListGenerationDefinition
    {
        public ShoppingListId Id { get; set; }
        public IStore Store { get; set; }
        public IEnumerable<IShoppingListSection> Sections { get; set; }
        public IEnumerable<ShoppingListSectionGenerationDefinition> SectionDefinitions { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool UseCompletionDate { get; set; } = false;
    }
}