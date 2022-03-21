using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Manufacturers.ToDomain
{
    public class ManufacturerConverter : IToDomainConverter<ManufacturerContract, Manufacturer>
    {
        public Manufacturer ToDomain(ManufacturerContract source)
        {
            return new Manufacturer(source.Id, source.Name);
        }
    }
}