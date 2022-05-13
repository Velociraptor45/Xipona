using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Manufacturers.Commands
{
    public class ModifyManufacturerContract
    {
        public ModifyManufacturerContract(Guid manufacturerId, string name)
        {
            ManufacturerId = manufacturerId;
            Name = name;
        }

        public Guid ManufacturerId { get; set; }
        public string Name { get; set; }
    }
}