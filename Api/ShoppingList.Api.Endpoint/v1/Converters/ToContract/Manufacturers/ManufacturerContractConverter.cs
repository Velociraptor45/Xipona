using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Manufacturers;

public class ManufacturerContractConverter : IToContractConverter<ManufacturerReadModel, ManufacturerContract>
{
    public ManufacturerContract ToContract(ManufacturerReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new ManufacturerContract(source.Id.Value, source.Name.Value, source.IsDeleted);
    }
}