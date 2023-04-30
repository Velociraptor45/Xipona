using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Converters.Recipes.ToDomain;

public class EditedIngredientConverterTests
    : ToDomainConverterBase<IngredientContract, EditedIngredient, EditedIngredientConverter>
{
    protected override EditedIngredientConverter CreateSut()
    {
        return new EditedIngredientConverter();
    }

    protected override void AddMapping(IMappingExpression<IngredientContract, EditedIngredient> mapping)
    {
        mapping
            .ForCtorParam(nameof(EditedIngredient.Key), opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForCtorParam(nameof(EditedIngredient.Id), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(EditedIngredient.ItemCategoryId), opt => opt.MapFrom(src => src.ItemCategoryId))
            .ForCtorParam(nameof(EditedIngredient.QuantityTypeId), opt => opt.MapFrom(src => src.QuantityType))
            .ForCtorParam(nameof(EditedIngredient.Quantity), opt => opt.MapFrom(src => src.Quantity))
            .ForCtorParam(nameof(EditedIngredient.DefaultItemId), opt => opt.MapFrom(src => src.DefaultItemId))
            .ForCtorParam(nameof(EditedIngredient.DefaultItemTypeId), opt => opt.MapFrom(src => src.DefaultItemTypeId))
            .ForCtorParam(nameof(EditedIngredient.DefaultStoreId), opt => opt.MapFrom(src => src.DefaultStoreId))
            .ForCtorParam(nameof(EditedIngredient.AddToShoppingListByDefault),
                opt => opt.MapFrom(src => src.AddToShoppingListByDefault))
            .ForCtorParam(nameof(EditedIngredient.ItemCategorySelector),
                opt => opt.MapFrom(src =>
                    new ItemCategorySelector(new List<ItemCategorySearchResult>(0), string.Empty)))
            .ForCtorParam(nameof(EditedIngredient.ItemSelector),
                opt => opt.MapFrom(src => new ItemSelector(new List<SearchItemByItemCategoryResult>(0))))
            .ForMember(dest => dest.AvailableStoresOfItem, opt => opt.Ignore())
            .ForMember(dest => dest.SelectedItemCategoryName, opt => opt.Ignore());
    }
}