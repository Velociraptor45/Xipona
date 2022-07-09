using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.AutoFixture.Selectors;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.SpecimenBuilders;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

public class DomainCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new EnumSpecimenBuilder<QuantityType>());
        fixture.Customizations.Add(new TypeRelay(typeof(IItemAvailability), typeof(ItemAvailability)));
        fixture.Customizations.Add(new TypeRelay(typeof(IShoppingListItem), typeof(ShoppingListItem)));
        fixture.Customizations.Add(new TypeRelay(typeof(IShoppingListSection), typeof(ShoppingListSection)));
        fixture.Customizations.Add(new TypeRelay(typeof(IItem), typeof(Item)));
        fixture.Customizations.Add(new TypeRelay(typeof(IItemType), typeof(ItemType)));
        fixture.Customizations.Add(new TypeRelay(typeof(IItemCategory), typeof(ItemCategory)));
        fixture.Customizations.Add(new TypeRelay(typeof(IManufacturer), typeof(Manufacturer)));
        fixture.Customizations.Add(new TypeRelay(typeof(IStore), typeof(Store)));
        fixture.Customizations.Add(new TypeRelay(typeof(ISection), typeof(Section)));

        fixture.Customize<ItemCategoryId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<ManufacturerId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<ShoppingListId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<ItemId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<ItemTypeId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<TemporaryItemId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<StoreId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<SectionId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));

        fixture.Customize(new PriceCustomization());
        fixture.Customize(new QuantityCustomization());
        fixture.Customize(new ItemQuantityCustomization());
    }
}