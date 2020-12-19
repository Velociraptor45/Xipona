namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models
{
    public class Manufacturer : IManufacturer
    {
        public Manufacturer(ManufacturerId id, string name, bool isDeleted)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
        }

        public ManufacturerId Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
    }
}