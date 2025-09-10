using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Contracts.Common.Queries;
using Xipona.Api.Contracts.ItemCategories.Commands;
using Xipona.Api.Contracts.ItemCategories.Queries;
using Xipona.Api.Contracts.Items.Commands.CreateItem;
using Xipona.Api.Contracts.Items.Commands.CreateItemWithTypes;
using Xipona.Api.Contracts.Items.Commands.MakeTemporaryItemPermanent;
using Xipona.Api.Contracts.Items.Commands.MergeItems;
using Xipona.Api.Contracts.Items.Commands.ModifyItem;
using Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using Xipona.Api.Contracts.Items.Commands.UpdateItem;
using Xipona.Api.Contracts.Items.Commands.UpdateItemPrice;
using Xipona.Api.Contracts.Items.Commands.UpdateItemWithTypes;
using Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using Xipona.Api.Contracts.Items.Queries.Get;
using Xipona.Api.Contracts.Items.Queries.GetItemTypePrices;
using Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using Xipona.Api.Contracts.Items.Queries.SearchItemsForMerge;
using Xipona.Api.Contracts.Items.Queries.SearchItemsForShoppingLists;
using Xipona.Api.Contracts.Items.Queries.Shared;
using Xipona.Api.Contracts.Manufacturers.Commands;
using Xipona.Api.Contracts.Manufacturers.Queries;
using Xipona.Api.Contracts.Recipes.Commands.CreateRecipe;
using Xipona.Api.Contracts.Recipes.Commands.ModifyRecipe;
using Xipona.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using Xipona.Api.Contracts.Recipes.Queries.Get;
using Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using Xipona.Api.Contracts.RecipeTags.Commands;
using Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using Xipona.Api.Contracts.ShoppingLists.Commands.AddItemDiscount;
using Xipona.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using Xipona.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using Xipona.Api.Contracts.ShoppingLists.Commands.AddShoppingListDiscount;
using Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using Xipona.Api.Contracts.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using Xipona.Api.Contracts.ShoppingLists.Commands.PutItemInBasket;
using Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemDiscount;
using Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromBasket;
using Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList;
using Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using Xipona.Api.Contracts.Stores.Commands.CreateStore;
using Xipona.Api.Contracts.Stores.Commands.ModifyStore;
using Xipona.Api.Contracts.Stores.Queries.Get;
using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using Xipona.Api.Contracts.Stores.Queries.Shared;
using Xipona.Api.Contracts.Users.Commands.AllCurrencies;
using Xipona.Api.Contracts.Users.Commands.Login;
using Xipona.Api.Contracts.Users.Commands.UpdateGeneralSettings;

namespace Xipona.Api.WebApp.Serialization;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(ErrorContract))]
// Store
[JsonSerializable(typeof(IReadOnlyCollection<SectionContract>))]
[JsonSerializable(typeof(StoreContract))]
[JsonSerializable(typeof(List<StoreSearchResultContract>))]
[JsonSerializable(typeof(SectionForShoppingContract))]
[JsonSerializable(typeof(List<StoreForShoppingContract>))]
[JsonSerializable(typeof(List<StoreForItemContract>))]
[JsonSerializable(typeof(List<ModifySectionContract>))]
[JsonSerializable(typeof(ModifyStoreContract))]
[JsonSerializable(typeof(List<CreateSectionContract>))]
[JsonSerializable(typeof(CreateStoreContract))]
// ShoppingList
[JsonSerializable(typeof(ShoppingListContract))]
[JsonSerializable(typeof(ShoppingListItemContract))]
[JsonSerializable(typeof(ShoppingListSectionContract))]
[JsonSerializable(typeof(ShoppingListStoreContract))]
[JsonSerializable(typeof(ItemIdContract))]
[JsonSerializable(typeof(RemoveItemFromShoppingListContract))]
[JsonSerializable(typeof(RemoveItemFromBasketContract))]
[JsonSerializable(typeof(RemoveItemDiscountContract))]
[JsonSerializable(typeof(PutItemInBasketContract))]
[JsonSerializable(typeof(ChangeItemQuantityOnShoppingListContract))]
[JsonSerializable(typeof(AddTemporaryItemToShoppingListContract))]
[JsonSerializable(typeof(TemporaryShoppingListItemContract))]
[JsonSerializable(typeof(AddItemWithTypeToShoppingListContract))]
[JsonSerializable(typeof(AddItemsToShoppingListsContract))]
[JsonSerializable(typeof(AddItemDiscountContract))]
[JsonSerializable(typeof(Xipona.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists.AddItemToShoppingListContract), TypeInfoPropertyName = "Xipona00Api00Contracts00ShoppingLists00Commands00AddItemsToShoppingLists00AddItemToShoppingListContract")]
[JsonSerializable(typeof(Xipona.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList.AddItemToShoppingListContract), TypeInfoPropertyName = "Xipona00Api00Contracts00ShoppingLists00Commands00AddItemToShoppingList00AddItemToShoppingListContract")]
[JsonSerializable(typeof(List<AddShoppingListDiscountContract>))]
// Recipe
[JsonSerializable(typeof(List<RecipeSearchResultContract>))]
[JsonSerializable(typeof(IngredientContract))]
[JsonSerializable(typeof(PreparationStepContract))]
[JsonSerializable(typeof(RecipeContract))]
[JsonSerializable(typeof(SideDishContract))]
[JsonSerializable(typeof(ItemAmountForOneServingAvailabilityContract))]
[JsonSerializable(typeof(ItemAmountForOneServingContract))]
[JsonSerializable(typeof(ItemAmountsForOneServingContract))]
[JsonSerializable(typeof(List<IngredientQuantityTypeContract>))]
[JsonSerializable(typeof(ModifyIngredientContract))]
[JsonSerializable(typeof(ModifyPreparationStepContract))]
[JsonSerializable(typeof(ModifyRecipeContract))]
[JsonSerializable(typeof(CreateIngredientContract))]
[JsonSerializable(typeof(CreatePreparationStepContract))]
[JsonSerializable(typeof(CreateRecipeContract))]
// Manufacturer
[JsonSerializable(typeof(List<ManufacturerSearchResultContract>))]
[JsonSerializable(typeof(ModifyManufacturerContract))]
// Item
[JsonSerializable(typeof(List<SearchItemResultContract>))]
[JsonSerializable(typeof(List<SearchItemForShoppingListResultContract>))]
[JsonSerializable(typeof(SearchItemByItemCategoryAvailabilityContract))]
[JsonSerializable(typeof(List<SearchItemByItemCategoryResultContract>))]
[JsonSerializable(typeof(Xipona.Api.Contracts.Items.Queries.Get.ItemAvailabilityContract), TypeInfoPropertyName = "Xipona00Api00Contracts00Items00Queries00Get00ItemAvailabilityContract")]
[JsonSerializable(typeof(ItemContract))]
[JsonSerializable(typeof(ItemSectionContract))]
[JsonSerializable(typeof(ItemStoreContract))]
[JsonSerializable(typeof(Xipona.Api.Contracts.Items.Commands.Shared.ItemTypeContract), TypeInfoPropertyName = "Xipona00Api00Contracts00Items00Commands00Shared00ItemTypeContract")]
[JsonSerializable(typeof(Xipona.Api.Contracts.Items.Queries.Get.ItemTypeContract), TypeInfoPropertyName = "Xipona00Api00Contracts00Items00Queries00Get00ItemTypeContract")]
[JsonSerializable(typeof(ItemTypePriceContract))]
[JsonSerializable(typeof(ItemTypePricesContract))]
[JsonSerializable(typeof(List<QuantityTypeContract>))]
[JsonSerializable(typeof(List<QuantityTypeInPacketContract>))]
[JsonSerializable(typeof(UpdateItemContract))]
[JsonSerializable(typeof(UpdateItemTypeContract))]
[JsonSerializable(typeof(UpdateItemWithTypesContract))]
[JsonSerializable(typeof(UpdateItemPriceContract))]
[JsonSerializable(typeof(IEnumerable<Xipona.Api.Contracts.Items.Commands.Shared.ItemAvailabilityContract>), TypeInfoPropertyName = "ListXipona00Api00Contracts00Items00Commands00Shared00ItemAvailabilityContract")]
[JsonSerializable(typeof(ModifyItemContract))]
[JsonSerializable(typeof(ModifyItemTypeContract))]
[JsonSerializable(typeof(ModifyItemWithTypesContract))]
[JsonSerializable(typeof(MakeTemporaryItemPermanentContract))]
[JsonSerializable(typeof(CreateItemContract))]
[JsonSerializable(typeof(CreateItemTypeContract))]
[JsonSerializable(typeof(CreateItemWithTypesContract))]
[JsonSerializable(typeof(MergeItemsContract))]
[JsonSerializable(typeof(MergedItemContract))]
[JsonSerializable(typeof(IEnumerable<MergedItemTypeContract>))]
[JsonSerializable(typeof(List<SearchItemsForMergeResultContract>))]
// ItemCategory
[JsonSerializable(typeof(List<ItemCategorySearchResultContract>))]
[JsonSerializable(typeof(ModifyItemCategoryContract))]
// Recipe Tag
[JsonSerializable(typeof(RecipeTagContract))]
[JsonSerializable(typeof(List<RecipeTagContract>))]
[JsonSerializable(typeof(CreateRecipeTagContract))]
// User
[JsonSerializable(typeof(UserInfoContract))]
[JsonSerializable(typeof(GeneralSettingsContract), TypeInfoPropertyName = "Xipona00Api00Contracts00Users00Commands00UpdateGeneralSettings00GeneralSettingsContract")]
[JsonSerializable(typeof(CurrencyContract), TypeInfoPropertyName = "Xipona00Api00Contracts00Users00Commands00AllCurrencies00CurrencyContract")]
[JsonSerializable(typeof(List<CurrencyContract>))]
[JsonSerializable(typeof(Contracts.Users.Queries.GetGeneralSettings.GeneralSettingsContract), TypeInfoPropertyName = "Xipona00Api00Contracts00Users00Queries00GetGeneralSettings00GeneralSettingsContract")]
[JsonSerializable(typeof(Contracts.Users.Queries.GetGeneralSettings.CurrencyContract), TypeInfoPropertyName = "Xipona00Api00Contracts00Users00Queries00GetGeneralSettings00CurrencyContract")]
// Common
[JsonSerializable(typeof(List<ItemCategoryContract>))]
[JsonSerializable(typeof(List<ManufacturerContract>))]
// Primitive
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(double))]
[JsonSerializable(typeof(decimal))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(char))]
[JsonSerializable(typeof(Guid))]
[JsonSerializable(typeof(Guid[]))]
[JsonSerializable(typeof(DateTimeOffset?))]
[JsonSerializable(typeof(TimeSpan))]
[JsonSerializable(typeof(CancellationToken))]
internal partial class XiponaJsonSerializationContext : JsonSerializerContext
{
}
