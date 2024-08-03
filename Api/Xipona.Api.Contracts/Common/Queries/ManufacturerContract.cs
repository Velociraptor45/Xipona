using System;

namespace ProjectHermes.Xipona.Api.Contracts.Common.Queries
{
    /// <summary>
    /// Represents a manufacturer.
    /// </summary>
    public class ManufacturerContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="isDeleted"></param>
        public ManufacturerContract(Guid id, string name, bool isDeleted)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
        }

        /// <summary>
        /// Manufacturer ID.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Manufacturer Name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Whether the manufacturer is deleted. If deleted, true, otherwise false.
        /// </summary>
        public bool IsDeleted { get; }
    }
}