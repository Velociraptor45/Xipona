using System;

namespace ProjectHermes.Xipona.Api.Contracts.Manufacturers.Queries
{
    /// <summary>
    /// The search result for a manufacturer.
    /// </summary>
    public class ManufacturerSearchResultContract
    {
        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public ManufacturerSearchResultContract(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// The ID of the manufacturer.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The name of the manufacturer.
        /// </summary>
        public string Name { get; }
    }
}