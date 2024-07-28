using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.ShoppingLists.ToDomain;
using ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Converters.ShoppingLists.ToDomain;

public class TemporaryShoppingListItemConverterTests :
    ToDomainConverterBase<TemporaryShoppingListItemContract, TemporaryShoppingListItem, TemporaryShoppingListItemConverter>
{
    protected override TemporaryShoppingListItemConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<TemporaryShoppingListItemContract, TemporaryShoppingListItem> mapping)
    {
        mapping
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.IsInBasket, opt => opt.MapFrom(src => src.IsInBasket))
            .ForMember(dest => dest.QuantityInBasket, opt => opt.MapFrom(src => src.QuantityInBasket));
    }
}