namespace ShoppingList.Api.Domain.Models
{
    public class Manufacturer
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