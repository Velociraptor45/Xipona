using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.TestTools;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Items.States;
public class ActiveStoresBuilder : TestBuilderBase<ActiveStores>
{
    public ActiveStoresBuilder WithStores(IReadOnlyCollection<ItemStore> stores)
    {
        FillConstructorWith("Stores", stores);
        return this;
    }
}