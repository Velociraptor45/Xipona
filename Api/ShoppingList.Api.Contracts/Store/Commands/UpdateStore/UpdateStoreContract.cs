using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore
{
    public class UpdateStoreContract
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UpdateSectionContract> Sections { get; set; }
    }
}