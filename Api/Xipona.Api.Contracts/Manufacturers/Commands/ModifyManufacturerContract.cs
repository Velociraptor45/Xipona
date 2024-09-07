using System;

namespace ProjectHermes.Xipona.Api.Contracts.Manufacturers.Commands
{
    /// <summary>
    /// Represents the contract for modifying a manufacturer.
    /// </summary>
    public class ModifyManufacturerContract
    {
        /// <summary>
        /// </summary>
        /// <param name="manufacturerId"></param>
        /// <param name="name"></param>
        public ModifyManufacturerContract(Guid manufacturerId, string name)
        {
            ManufacturerId = manufacturerId;
            Name = name;
        }

        /// <summary>
        /// The ID of the manufacturer.
        /// </summary>
        public Guid ManufacturerId { get; set; }

        /// <summary>
        /// The new name of the manufacturer.
        /// </summary>
        public string Name { get; set; }
    }
}