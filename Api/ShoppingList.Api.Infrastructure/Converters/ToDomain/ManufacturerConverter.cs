using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain
{
    public class ManufacturerConverter : IToDomainConverter<Manufacturers.Entities.Manufacturer, IManufacturer>
    {
        private readonly IManufacturerFactory manufacturerFactory;

        public ManufacturerConverter(IManufacturerFactory manufacturerFactory)
        {
            this.manufacturerFactory = manufacturerFactory;
        }

        public IManufacturer ToDomain(Manufacturers.Entities.Manufacturer source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return manufacturerFactory.Create(
                new ManufacturerId(source.Id),
                source.Name,
                source.Deleted);
        }
    }
}