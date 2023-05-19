using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.ShoppingLists.Entities;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.ShoppingLists.Converters.ToDomain;

public class ShoppingListItemConverterTests
    : ToDomainConverterTestBase<ItemsOnList, IShoppingListItem, ShoppingListItemConverter>
{
    public override ShoppingListItemConverter CreateSut()
    {
        return new(new ShoppingListItemFactory());
    }

    protected override void AddMapping(IMappingExpression<ItemsOnList, IShoppingListItem> mapping)
    {
        mapping.As<ShoppingListItem>();
    }

    protected override ItemsOnList CreateSource()
    {
        return new ItemsOnListEntityBuilder().Create();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ItemsOnList, ShoppingListItem>()
            .ForCtorParam(nameof(ShoppingListItem.Id).LowerFirstChar(), opt => opt.MapFrom(src => src.ItemId))
            .ForCtorParam(nameof(ShoppingListItem.TypeId).LowerFirstChar(), opt => opt.MapFrom(src => new ItemTypeId(src.ItemTypeId!.Value)))
            .ForCtorParam(nameof(ShoppingListItem.IsInBasket).LowerFirstChar(), opt => opt.MapFrom(src => src.InBasket))
            .ForCtorParam(nameof(ShoppingListItem.Quantity).LowerFirstChar(), opt => opt.MapFrom(src => src.Quantity));
    }
}