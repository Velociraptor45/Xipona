using Xipona.Api.Contracts.Manufacturers.Queries;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Manufacturers.Services.Queries;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.Manufacturers;

public class ManufacturerSearchResultContractConverter :
    IToContractConverter<ManufacturerSearchResultReadModel, ManufacturerSearchResultContract>
{
    public ManufacturerSearchResultContract ToContract(ManufacturerSearchResultReadModel source)
    {
        return new ManufacturerSearchResultContract(source.Id, source.Name);
    }
}