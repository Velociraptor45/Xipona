namespace ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.Manufacturers
{
    public class ModifyManufacturerRequest
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