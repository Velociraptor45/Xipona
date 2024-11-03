using AutoMapper;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Converters.ToDomain;
using ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;
using Discount = ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models.Discount;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.ShoppingLists.Converters.ToDomain;

public class DiscountConverterTests :
    ToDomainConverterTestBase<Repositories.ShoppingLists.Entities.Discount, Discount, DiscountConverter>
{
    public override DiscountConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<Repositories.ShoppingLists.Entities.Discount, Discount> mapping)
    {
        mapping
            .ForCtorParam(nameof(Discount.ItemId), opt => opt.MapFrom(src => new ItemId(src.ItemId)))
            .ForCtorParam(nameof(Discount.ItemTypeId), opt => opt.MapFrom(src => new ItemTypeId(src.ItemTypeId!.Value)))
            .ForCtorParam(nameof(Discount.Price), opt => opt.MapFrom(src => new Price(src.DiscountPrice)));
    }

    protected override Repositories.ShoppingLists.Entities.Discount CreateSource()
    {
        return new DiscountEntityBuilder().Create();
    }
}