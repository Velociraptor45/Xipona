using ProjectHermes.ShoppingList.Api.Contracts.Manufacturers.Queries;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Manufacturers;

public class ManufacturerSearchContractConverter :
    IToContractConverter<ManufacturerSearchReadModel, ManufacturerSearchContract>
{
    public ManufacturerSearchContract ToContract(ManufacturerSearchReadModel source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ManufacturerSearchContract(source.Id.Value, source.Name.Value);
    }
}