using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Stores.Entities;

public class SectionEntityBuilder : TestBuilder<Section>
{
    public SectionEntityBuilder()
    {
        FillPropertyWith(s => s.Store, null);
    }

    public SectionEntityBuilder WithStoreId(Guid storeId)
    {
        FillPropertyWith(s => s.StoreId, storeId);
        return this;
    }
}