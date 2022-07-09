using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.Stores.Commands.CreateStore
{
    public class CreateStoreContract
    {
        public CreateStoreContract(string name, IEnumerable<CreateSectionContract> sections)
        {
            Name = name;
            Sections = sections;
        }

        public string Name { get; set; }
        public IEnumerable<CreateSectionContract> Sections { get; set; }
    }
}