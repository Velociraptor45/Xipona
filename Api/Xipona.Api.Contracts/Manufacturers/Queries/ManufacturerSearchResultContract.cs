using System;

namespace ProjectHermes.Xipona.Api.Contracts.Manufacturers.Queries
{
    public class ManufacturerSearchResultContract
    {
        public ManufacturerSearchResultContract(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
    }
}