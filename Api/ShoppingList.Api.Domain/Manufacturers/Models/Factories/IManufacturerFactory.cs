using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories
{
    public interface IManufacturerFactory
    {
        IManufacturer Create(ManufacturerId id, string name, bool isDeleted);
    }
}