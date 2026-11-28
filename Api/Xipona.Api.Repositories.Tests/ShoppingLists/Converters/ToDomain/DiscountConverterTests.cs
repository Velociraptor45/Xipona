using AutoMapper;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Repositories.ShoppingLists.Converters.ToDomain;
using Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;
using Xipona.Api.TestTools.Extensions;

namespace Xipona.Api.Repositories.Tests.ShoppingLists.Converters.ToDomain;

public class DiscountConverterTests :
    ToDomainConverterTestBase<Repositories.ShoppingLists.Entities.Discount, ItemDiscount, DiscountConverter>
{
    public override DiscountConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<Repositories.ShoppingLists.Entities.Discount, ItemDiscount> mapping)
    {
        mapping
            .ForCtorParam(nameof(ItemDiscount.ItemId).LowerFirstChar(), opt => opt.MapFrom(src => new ItemId(src.ItemId)))
            .ForCtorParam(nameof(ItemDiscount.ItemTypeId).LowerFirstChar(), opt => opt.MapFrom(src => new ItemTypeId(src.ItemTypeId!.Value)))
            .ForCtorParam(nameof(ItemDiscount.Price).LowerFirstChar(), opt => opt.MapFrom(src => new Price(src.DiscountPrice)));
    }

    protected override Repositories.ShoppingLists.Entities.Discount CreateSource()
    {
        return new DiscountEntityBuilder().Create();
    }
}