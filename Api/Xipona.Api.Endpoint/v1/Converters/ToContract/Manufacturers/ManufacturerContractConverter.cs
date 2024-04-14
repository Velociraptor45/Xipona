using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Manufacturers;

public class ManufacturerContractConverter :
    IToContractConverter<ManufacturerReadModel, ManufacturerContract>,
    IToContractConverter<IManufacturer, ManufacturerContract>
{
    public ManufacturerContract ToContract(ManufacturerReadModel source)
    {
        return new ManufacturerContract(source.Id, source.Name, source.IsDeleted);
    }

    public ManufacturerContract ToContract(IManufacturer source)
    {
        return new ManufacturerContract(source.Id, source.Name, source.IsDeleted);
    }
}