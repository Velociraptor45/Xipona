using System.Collections.Generic;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.Shared;

namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore
{
    public class CreateStoreContract
    {
        public string Name { get; set; }
        public IEnumerable<StoreSectionContract> Sections { get; set; }
    }
}