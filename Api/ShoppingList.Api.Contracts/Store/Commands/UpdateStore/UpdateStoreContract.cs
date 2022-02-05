using System.Collections.Generic;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.Shared;

namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore
{
    public class UpdateStoreContract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<StoreSectionContract> Sections { get; set; }
    }
}