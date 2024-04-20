using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Manufacturers.ToDomain
{
    public class ManufacturerConverter : IToDomainConverter<ManufacturerContract, EditedManufacturer>
    {
        public EditedManufacturer ToDomain(ManufacturerContract source)
        {
            return new EditedManufacturer(source.Id, source.Name);
        }
    }
}