using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Stores.Models;

public class StoreIdBuilder : DomainTestBuilderBase<StoreId>
{
    public StoreIdBuilder WithValue(Guid value)
    {
        FillConstructorWith("value", value);
        return this;
    }
}