using System;

namespace ProjectHermes.Xipona.Api.Contracts.Common.Queries
{
    public class ManufacturerContract
    {
        public ManufacturerContract(Guid id, string name, bool isDeleted)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
        }

        public Guid Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
    }
}