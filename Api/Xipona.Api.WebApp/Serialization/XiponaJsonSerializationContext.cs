using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.ItemCategories.Commands;
using ProjectHermes.Xipona.Api.Contracts.ItemCategories.Queries;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemPrice;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.GetItemTypePrices;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsForShoppingLists;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.Xipona.Api.Contracts.Manufacturers.Commands;
using ProjectHermes.Xipona.Api.Contracts.Manufacturers.Queries;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Commands;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemDiscount;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemDiscount;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.Xipona.Api.Contracts.Stores.Commands.CreateStore;
using ProjectHermes.Xipona.Api.Contracts.Stores.Commands.ModifyStore;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresOverview;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Shared;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;

namespace ProjectHermes.Xipona.Api.WebApp.Serialization;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(ErrorContract))]
// Store
[JsonSerializable(typeof(IReadOnlyCollection<SectionContract>))]
[JsonSerializable(typeof(StoreContract))]
[JsonSerializable(typeof(List<StoreSearchResultContract>))]
[JsonSerializable(typeof(SectionForShoppingContract))]
[JsonSerializable(typeof(List<StoreForShoppingContract>))]
[JsonSerializable(typeof(SectionForItemContract))]
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
[JsonSerializable(typeof(ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists.AddItemToShoppingListContract), TypeInfoPropertyName = "ProjectHermes00Xipona00Api00Contracts00ShoppingLists00Commands00AddItemsToShoppingLists00AddItemToShoppingListContract")]
[JsonSerializable(typeof(ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList.AddItemToShoppingListContract), TypeInfoPropertyName = "ProjectHermes00Xipona00Api00Contracts00ShoppingLists00Commands00AddItemToShoppingList00AddItemToShoppingListContract")]
// Recipe
[JsonSerializable(typeof(RecipeTagContract))]
[JsonSerializable(typeof(CreateRecipeTagContract))]
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
[JsonSerializable(typeof(ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get.ItemAvailabilityContract), TypeInfoPropertyName = "ProjectHermes00Xipona00Api00Contracts00Items00Queries00Get00ItemAvailabilityContract")]
[JsonSerializable(typeof(ItemContract))]
[JsonSerializable(typeof(ItemSectionContract))]
[JsonSerializable(typeof(ItemStoreContract))]
[JsonSerializable(typeof(ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared.ItemTypeContract), TypeInfoPropertyName = "ProjectHermes00Xipona00Api00Contracts00Items00Commands00Shared00ItemTypeContract")]
[JsonSerializable(typeof(ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get.ItemTypeContract), TypeInfoPropertyName = "ProjectHermes00Xipona00Api00Contracts00Items00Queries00Get00ItemTypeContract")]
[JsonSerializable(typeof(ItemTypePriceContract))]
[JsonSerializable(typeof(ItemTypePricesContract))]
[JsonSerializable(typeof(List<QuantityTypeContract>))]
[JsonSerializable(typeof(List<QuantityTypeInPacketContract>))]
[JsonSerializable(typeof(UpdateItemContract))]
[JsonSerializable(typeof(UpdateItemTypeContract))]
[JsonSerializable(typeof(UpdateItemWithTypesContract))]
[JsonSerializable(typeof(UpdateItemPriceContract))]
[JsonSerializable(typeof(IEnumerable<ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared.ItemAvailabilityContract>), TypeInfoPropertyName = "ListProjectHermes00Xipona00Api00Contracts00Items00Commands00Shared00ItemAvailabilityContract")]
[JsonSerializable(typeof(ModifyItemContract))]
[JsonSerializable(typeof(ModifyItemTypeContract))]
[JsonSerializable(typeof(ModifyItemWithTypesContract))]
[JsonSerializable(typeof(MakeTemporaryItemPermanentContract))]
[JsonSerializable(typeof(CreateItemContract))]
[JsonSerializable(typeof(CreateItemTypeContract))]
[JsonSerializable(typeof(CreateItemWithTypesContract))]
// ItemCategory
[JsonSerializable(typeof(List<ItemCategorySearchResultContract>))]
[JsonSerializable(typeof(ModifyItemCategoryContract))]
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
[JsonSerializable(typeof(DateTimeOffset?))]
[JsonSerializable(typeof(TimeSpan))]
[JsonSerializable(typeof(CancellationToken))]
internal partial class XiponaJsonSerializationContext : JsonSerializerContext
{
}
