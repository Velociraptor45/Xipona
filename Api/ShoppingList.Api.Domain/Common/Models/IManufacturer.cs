namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models
{
    public interface IManufacturer
    {
        ManufacturerId Id { get; }
        string Name { get; }
        bool IsDeleted { get; }
    }
}