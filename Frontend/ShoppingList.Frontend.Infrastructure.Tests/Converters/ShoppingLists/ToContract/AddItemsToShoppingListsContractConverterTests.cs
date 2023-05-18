using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Converters.ShoppingLists.ToContract;

public class AddItemsToShoppingListsContractConverterTests
    : ToContractConverterBase<IEnumerable<AddToShoppingListItem>, AddItemsToShoppingListsContract,
        AddItemsToShoppingListsContractConverter>
{
    protected override AddItemsToShoppingListsContractConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<IEnumerable<AddToShoppingListItem>, AddItemsToShoppingListsContract> mapping)
    {
        mapping
            .ForCtorParam(nameof(AddItemsToShoppingListsContract.Items).LowerFirstChar(),
                opt => opt.MapFrom((src, ctx) => src.Select(s => ctx.Mapper.Map<AddItemToShoppingListContract>(s))));
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<AddToShoppingListItem, AddItemToShoppingListContract>()
            .ForCtorParam(nameof(AddItemToShoppingListContract.ItemId).LowerFirstChar(),
                opt => opt.MapFrom(src => src.ItemId))
            .ForCtorParam(nameof(AddItemToShoppingListContract.ItemTypeId).LowerFirstChar(),
                opt => opt.MapFrom(src => src.ItemTypeId))
            .ForCtorParam(nameof(AddItemToShoppingListContract.StoreId).LowerFirstChar(),
                opt => opt.MapFrom(src => src.SelectedStoreId))
            .ForCtorParam(nameof(AddItemToShoppingListContract.Quantity).LowerFirstChar(),
                opt => opt.MapFrom(src => src.Quantity));
    }
}