using ProjectHermes.ShoppingList.Api.Contracts.Manufacturers.Queries;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Manufacturers;

public class ManufacturerSearchResultContractConverter :
    IToContractConverter<ManufacturerSearchResultReadModel, ManufacturerSearchResultContract>
{
    public ManufacturerSearchResultContract ToContract(ManufacturerSearchResultReadModel source)
    {
        return new ManufacturerSearchResultContract(source.Id.Value, source.Name.Value);
    }
}