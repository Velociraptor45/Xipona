using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Core.TestKit;
using ShoppingList.Api.Domain.TestKit.Common.AutoFixture.Selectors;
using ShoppingList.Api.Domain.TestKit.ShoppingLists.SpecimenBuilders;

namespace ShoppingList.Api.Domain.TestKit.Common;

public class DomainTestBuilderBase<TModel> : TestBuilderBase<TModel>
{
    public DomainTestBuilderBase()
        : base()
    {
        Customizations.Add(new EnumSpecimenBuilder<QuantityType>());
        Customizations.Add(new TypeRelay(typeof(IStoreItemAvailability), typeof(StoreItemAvailability)));
        Customizations.Add(new TypeRelay(typeof(IShoppingListItem), typeof(ShoppingListItem)));
        Customizations.Add(new TypeRelay(typeof(IShoppingListSection), typeof(ShoppingListSection)));

        Customize<ItemCategoryId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        Customize<ManufacturerId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        Customize<ShoppingListId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        Customize<ItemId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        Customize<ItemTypeId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        Customize<TemporaryItemId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        Customize<StoreId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        Customize<SectionId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
    }
}