using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore
{
    public class CreateStoreContract
    {
        public string Name { get; set; }
        public IEnumerable<CreateSectionContract> Sections { get; set; }
    }
}