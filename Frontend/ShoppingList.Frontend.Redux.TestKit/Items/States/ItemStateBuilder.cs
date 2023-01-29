using ProjectHermes.ShoppingList.Frontend.TestTools;
using ShoppingList.Frontend.Redux.Items.States;
using ShoppingList.Frontend.Redux.Shared.States;
using System.Collections.Generic;

namespace ShoppingList.Frontend.Redux.TestKit.Items.States;
public class ItemStateBuilder : TestBuilderBase<ItemState>
{
    public ItemStateBuilder WithQuantityTypes(IReadOnlyCollection<QuantityType> quantityTypes)
    {
        FillConstructorWith("QuantityTypes", quantityTypes);
        return this;
    }

    public ItemStateBuilder WithQuantityTypesInPacket(IReadOnlyCollection<QuantityTypeInPacket> quantityTypesInPacket)
    {
        FillConstructorWith("QuantityTypesInPacket", quantityTypesInPacket);
        return this;
    }

    public ItemStateBuilder WithStores(ActiveStores stores)
    {
        FillConstructorWith("Stores", stores);
        return this;
    }

    public ItemStateBuilder WithSearch(ItemSearch search)
    {
        FillConstructorWith("Search", search);
        return this;
    }

    public ItemStateBuilder WithEditor(ItemEditor editor)
    {
        FillConstructorWith("Editor", editor);
        return this;
    }
}