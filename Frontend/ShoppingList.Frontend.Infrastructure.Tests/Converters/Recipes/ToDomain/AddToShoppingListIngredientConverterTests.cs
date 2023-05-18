using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Converters.Recipes.ToDomain;

public class AddToShoppingListIngredientConverterTests
    : ToDomainConverterBase<ItemAmountForOneServingContract, AddToShoppingListItem, AddToShoppingListIngredientConverter>
{
    protected override AddToShoppingListIngredientConverter CreateSut()
    {
        return new(new AddToShoppingListAvailabilityConverter());
    }

    protected override void AddMapping(IMappingExpression<ItemAmountForOneServingContract, AddToShoppingListItem> mapping)
    {
        mapping
            .ConstructUsing((src, ctx) => new AddToShoppingListItem(
                Guid.Empty,
                src.ItemId,
                src.ItemName,
                src.ItemTypeId,
                src.QuantityType,
                src.QuantityLabel,
                src.Quantity,
                src.DefaultStoreId,
                src.AddToShoppingListByDefault,
                src.Availabilities.Select(av => ctx.Mapper.Map<AddToShoppingListAvailability>(av)).ToList()))
            .ForMember(nameof(AddToShoppingListItem.ItemId), opt => opt.MapFrom(src => src.ItemId))
            .ForMember(nameof(AddToShoppingListItem.ItemName), opt => opt.MapFrom(src => src.ItemName))
            .ForMember(nameof(AddToShoppingListItem.ItemTypeId), opt => opt.MapFrom(src => src.ItemTypeId))
            .ForMember(nameof(AddToShoppingListItem.QuantityType), opt => opt.MapFrom(src => src.QuantityType))
            .ForMember(nameof(AddToShoppingListItem.QuantityLabel), opt => opt.MapFrom(src => src.QuantityLabel))
            .ForMember(nameof(AddToShoppingListItem.Quantity), opt => opt.MapFrom(src => src.Quantity))
            .ForMember(nameof(AddToShoppingListItem.SelectedStoreId), opt => opt.MapFrom(src => src.DefaultStoreId))
            .ForMember(nameof(AddToShoppingListItem.AddToShoppingList), opt => opt.MapFrom(src => src.AddToShoppingListByDefault))
            .ForMember(dest => dest.Key, opt => opt.Ignore());
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        new AddToShoppingListAvailabilityConverterTests().AddMapping(cfg);
    }
}