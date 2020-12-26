using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions
{
    public static class ManufacturerExtensions
    {
        public static ManufacturerReadModel ToReadModel(this IManufacturer model)
        {
            return new ManufacturerReadModel(model.Id, model.Name, model.IsDeleted);
        }
    }
}