using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore
{
    public class UpdateStoreContract
    {
        public UpdateStoreContract(Guid id, string name, IEnumerable<UpdateSectionContract> sections)
        {
            Id = id;
            Name = name;
            Sections = sections;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<UpdateSectionContract> Sections { get; set; }
    }
}