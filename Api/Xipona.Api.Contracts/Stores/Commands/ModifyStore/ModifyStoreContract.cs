using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Stores.Commands.ModifyStore
{
    public class ModifyStoreContract
    {
        public ModifyStoreContract(Guid id, string name, IEnumerable<ModifySectionContract> sections)
        {
            Id = id;
            Name = name;
            Sections = sections;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ModifySectionContract> Sections { get; set; }
    }
}