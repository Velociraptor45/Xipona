using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Manufacturers
{
    public class ModifyManufacturerRequest : IApiRequest
    {
        public ModifyManufacturerRequest(Guid manufacturerId, string name)
        {
            RequestId = Guid.NewGuid();
            ManufacturerId = manufacturerId;
            Name = name;
        }

        public Guid ManufacturerId { get; }
        public string Name { get; }
        public Guid RequestId { get; }
    }
}