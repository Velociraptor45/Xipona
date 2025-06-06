using Xipona.Api.Contracts.Manufacturers.Commands;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Shared.Ports.Requests.Manufacturers;
using System;

namespace Xipona.Frontend.Infrastructure.Converters.Manufacturers.ToContract;

public class ModifyManufacturerContractConverter :
    IToContractConverter<ModifyManufacturerRequest, ModifyManufacturerContract>
{
    public ModifyManufacturerContract ToContract(ModifyManufacturerRequest source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new ModifyManufacturerContract(source.ManufacturerId, source.Name);
    }
}