using Xipona.Api.Contracts.Common.Queries;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Manufacturers.States;

namespace Xipona.Frontend.Infrastructure.Converters.Manufacturers.ToDomain;

public class ManufacturerConverter : IToDomainConverter<ManufacturerContract, EditedManufacturer>
{
    public EditedManufacturer ToDomain(ManufacturerContract source)
    {
        return new EditedManufacturer(source.Id, source.Name);
    }
}