using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.Manufacturer.Queries.AllActiveManufacturers;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Endpoint.Extensions.Manufacturer
{
    public static class ManufacturerReadModelExtensions
    {
        public static ActiveManufacturerContract ToActiveContract(this ManufacturerReadModel readModel)
        {
            return new ActiveManufacturerContract(readModel.Id.Value, readModel.Name);
        }

        public static ManufacturerContract ToContract(this ManufacturerReadModel readModel)
        {
            return new ManufacturerContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted);
        }
    }
}