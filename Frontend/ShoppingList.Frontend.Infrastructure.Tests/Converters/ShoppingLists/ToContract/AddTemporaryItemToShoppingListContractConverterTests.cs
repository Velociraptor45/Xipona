using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Converters.ShoppingLists.ToContract;

public class AddTemporaryItemToShoppingListContractConverterTests : ToContractConverterBase<
    AddTemporaryItemToShoppingListRequest, AddTemporaryItemToShoppingListContract, AddTemporaryItemToShoppingListContractConverter>
{
    protected override AddTemporaryItemToShoppingListContractConverter CreateSut()
    {
        return new AddTemporaryItemToShoppingListContractConverter();
    }

    protected override void AddMapping(
        IMappingExpression<AddTemporaryItemToShoppingListRequest, AddTemporaryItemToShoppingListContract> mapping)
    {
        mapping
            .ForCtorParam(nameof(AddTemporaryItemToShoppingListContract.ItemName).LowerFirstChar(),
                opt => opt.MapFrom(src => src.ItemName))
            .ForCtorParam(nameof(AddTemporaryItemToShoppingListContract.QuantityType).LowerFirstChar(),
                opt => opt.MapFrom(src => src.QuantityType))
            .ForCtorParam(nameof(AddTemporaryItemToShoppingListContract.Quantity).LowerFirstChar(),
                opt => opt.MapFrom(src => src.Quantity))
            .ForCtorParam(nameof(AddTemporaryItemToShoppingListContract.Price).LowerFirstChar(),
                opt => opt.MapFrom(src => src.Price))
            .ForCtorParam(nameof(AddTemporaryItemToShoppingListContract.SectionId).LowerFirstChar(),
                opt => opt.MapFrom(src => src.SectionId))
            .ForCtorParam(nameof(AddTemporaryItemToShoppingListContract.TemporaryItemId).LowerFirstChar(),
                opt => opt.MapFrom(src => src.TemporaryId));
    }
}