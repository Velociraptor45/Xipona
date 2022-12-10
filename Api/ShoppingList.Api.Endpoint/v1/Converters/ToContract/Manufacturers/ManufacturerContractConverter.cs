using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Manufacturers;

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