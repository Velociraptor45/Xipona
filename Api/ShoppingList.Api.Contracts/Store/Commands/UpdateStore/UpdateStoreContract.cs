using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.Shared;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore
{
    public class UpdateStoreContract
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<StoreSectionContract> Sections { get; set; }
    }
}