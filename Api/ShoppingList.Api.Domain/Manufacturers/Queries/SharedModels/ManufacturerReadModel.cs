using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels
{
    public class ManufacturerReadModel
    {
        public ManufacturerReadModel(ManufacturerId id, string name, bool isDeleted)
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