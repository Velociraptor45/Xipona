using AutoMapper;
using FluentAssertions.Equivalency;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Converters.ToDomain;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;
using ProjectHermes.Xipona.Api.TestTools.Extensions;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.ShoppingLists.Converters.ToDomain;

public class ListDiscountConverterTests
{
    public class WithPrice : ListDiscountConverterTestsBase
    {
        protected override void AddMapping(IMappingExpression<ShoppingListDiscount, ListDiscount> mapping)
        {
            mapping
                .ForCtorParam(nameof(ListDiscount.Id).LowerFirstChar(), opt => opt.MapFrom(src => new ListDiscountId(src.Id)))
                .ForCtorParam(nameof(ListDiscount.Price).LowerFirstChar(), opt => opt.MapFrom(src => src.DiscountPrice));
        }

        protected override ShoppingListDiscount CreateSource()
        {
            return new ShoppingListDiscountEntityBuilder().WithoutDiscountPercentage().Create();
        }
    }

    public class WithPercentage : ListDiscountConverterTestsBase
    {
        protected override void AddMapping(IMappingExpression<ShoppingListDiscount, ListDiscount> mapping)
        {
            mapping
                .ForCtorParam(nameof(ListDiscount.Id).LowerFirstChar(), opt => opt.MapFrom(src => new ListDiscountId(src.Id)))
                .ForCtorParam(nameof(ListDiscount.Percentage).LowerFirstChar(), opt => opt.MapFrom(src => new Percentage(src.DiscountPercentage!.Value)));
        }

        protected override ShoppingListDiscount CreateSource()
        {
            return new ShoppingListDiscountEntityBuilder().WithoutDiscountPrice().Create();
        }

        protected override void CustomizeAssertionOptions(EquivalencyOptions<ListDiscount> opt)
        {
            opt.Excluding(info => info.Path == "Percentage.Inverted");
        }
    }

    public abstract class ListDiscountConverterTestsBase :
        ToDomainConverterTestBase<ShoppingListDiscount, ListDiscount, ListDiscountConverter>
    {
        public override ListDiscountConverter CreateSut()
        {
            return new();
        }
    }
}