using ProjectHermes.Xipona.Api.Contracts.Manufacturers.Queries;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Queries;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Manufacturers;

public class ManufacturerSearchResultContractConverter :
    IToContractConverter<ManufacturerSearchResultReadModel, ManufacturerSearchResultContract>
{
    public ManufacturerSearchResultContract ToContract(ManufacturerSearchResultReadModel source)
    {
        return new ManufacturerSearchResultContract(source.Id, source.Name);
    }
}