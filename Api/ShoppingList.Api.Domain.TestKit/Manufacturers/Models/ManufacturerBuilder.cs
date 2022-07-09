using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Manufacturers.Models;

public class ManufacturerBuilder : DomainTestBuilderBase<Manufacturer>
{
    public ManufacturerBuilder WithId(ManufacturerId id)
    {
        FillConstructorWith("id", id);
        return this;
    }

    public ManufacturerBuilder WithIsDeleted(bool isDeleted)
    {
        FillConstructorWith("isDeleted", isDeleted);
        return this;
    }
}