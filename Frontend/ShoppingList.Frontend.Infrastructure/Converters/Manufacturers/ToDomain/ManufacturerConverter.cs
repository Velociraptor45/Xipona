using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ShoppingList.Frontend.Redux.Manufacturers.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Manufacturers.ToDomain
{
    public class ManufacturerConverter : IToDomainConverter<ManufacturerContract, EditedManufacturer>
    {
        public EditedManufacturer ToDomain(ManufacturerContract source)
        {
            return new EditedManufacturer(source.Id, source.Name);
        }
    }
}