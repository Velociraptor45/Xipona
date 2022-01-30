using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common;

namespace ShoppingList.Api.Domain.TestKit.Stores.Models
{
    public class StoreIdBuilder : DomainTestBuilderBase<StoreId>
    {
        public StoreIdBuilder WithValue(int value)
        {
            FillConstructorWith("value", value);
            return this;
        }
    }
}