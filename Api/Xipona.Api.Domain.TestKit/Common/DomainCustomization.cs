using AutoFixture.Kernel;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.RecipeTags.Models;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.TestKit.Common.AutoFixture.Selectors;
using Xipona.Api.Domain.TestKit.Items.Models;
using Xipona.Api.Domain.TestKit.Recipes.Models;
using Xipona.Api.Domain.TestKit.ShoppingLists.Models;
using Xipona.Api.Domain.TestKit.ShoppingLists.SpecimenBuilders;
using Xipona.Api.Domain.TestKit.Stores.Models;
using Xipona.Api.Domain.Users.Models;

namespace Xipona.Api.Domain.TestKit.Common;

public class DomainCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new EnumSpecimenBuilder<QuantityType>());
        fixture.Customizations.Add(new TypeRelay(typeof(IShoppingListSection), typeof(ShoppingListSection)));
        fixture.Customizations.Add(new TypeRelay(typeof(IItem), typeof(Item)));
        fixture.Customizations.Add(new TypeRelay(typeof(IItemType), typeof(ItemType)));
        fixture.Customizations.Add(new TypeRelay(typeof(IItemCategory), typeof(ItemCategory)));
        fixture.Customizations.Add(new TypeRelay(typeof(IManufacturer), typeof(Manufacturer)));
        fixture.Customizations.Add(new TypeRelay(typeof(IStore), typeof(Store)));
        fixture.Customizations.Add(new TypeRelay(typeof(ISection), typeof(Section)));
        fixture.Customizations.Add(new TypeRelay(typeof(IRecipe), typeof(Recipe)));
        fixture.Customizations.Add(new TypeRelay(typeof(IIngredient), typeof(Ingredient)));
        fixture.Customizations.Add(new TypeRelay(typeof(IPreparationStep), typeof(PreparationStep)));
        fixture.Customizations.Add(new TypeRelay(typeof(IRecipeTag), typeof(RecipeTag)));

        fixture.Customize<ItemCategoryId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<ManufacturerId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<ShoppingListId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<ItemId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<ItemTypeId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<TemporaryItemId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<StoreId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<SectionId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<RecipeId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<IngredientId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<PreparationStepId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<RecipeTagId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<UserId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<ListDiscountId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));
        fixture.Customize<GeneralSettingId>(c => c.FromFactory(new MethodInvoker(new IdConstructorQuery())));

        fixture.Customize<ItemDiscount>(c => c.FromFactory(new MethodInvoker(new BiggestConstructorQuery())));
        fixture.Customize<Percentage>(c => c.FromFactory(new MethodInvoker(new BiggestConstructorQuery())));

        fixture.Customize(new PriceCustomization());
        fixture.Customize(new QuantityCustomization());
        fixture.Customize(new QuantityInBasketCustomization());
        fixture.Customize(new ItemQuantityCustomization());
        fixture.Customize(new NumberOfServingsCustomization());
        fixture.Customize(new QuantityInBasketCustomization());
        fixture.Customize(new QuantityTypeReadModelCustomization());

        fixture.Customize(new SectionsCustomization());

        fixture.Customize(new ItemAvailabilityCustomization());
    }
}