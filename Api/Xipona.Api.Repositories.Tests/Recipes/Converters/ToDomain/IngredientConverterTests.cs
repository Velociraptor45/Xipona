using AutoMapper;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Models.Factories;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.TestKit.Items.Services.Validation;
using Xipona.Api.Repositories.Recipes.Converters.ToDomain;
using Xipona.Api.Repositories.TestKit.Recipes.Entities;
using Xipona.Api.TestTools.Extensions;
using Ingredient = Xipona.Api.Repositories.Recipes.Entities.Ingredient;

namespace Xipona.Api.Repositories.Tests.Recipes.Converters.ToDomain;

public class IngredientConverterTests : ToDomainConverterTestBase<Ingredient, IIngredient, IngredientConverter>
{
    public override IngredientConverter CreateSut()
    {
        return new(_ => new IngredientFactory(new ValidatorMock(MockBehavior.Strict).Object));
    }

    protected override Ingredient CreateSource()
    {
        return new IngredientEntityBuilder().Create();
    }

    protected override void AddMapping(IMappingExpression<Ingredient, IIngredient> mapping)
    {
        mapping.As<Domain.Recipes.Models.Ingredient>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Ingredient, Domain.Recipes.Models.Ingredient>()
            .ForCtorParam(nameof(IIngredient.Id).LowerFirstChar(), opt => opt.MapFrom(src => new IngredientId(src.Id)))
            .ForCtorParam(nameof(IIngredient.ItemCategoryId).LowerFirstChar(), opt => opt.MapFrom(src => new ItemCategoryId(src.ItemCategoryId)))
            .ForCtorParam(nameof(IIngredient.QuantityType).LowerFirstChar(), opt => opt.MapFrom(src => src.QuantityType.ToEnum<IngredientQuantityType>()))
            .ForCtorParam(nameof(IIngredient.Quantity).LowerFirstChar(), opt => opt.MapFrom(src => new IngredientQuantity(src.Quantity)))
            .ForCtorParam(nameof(IIngredient.ShoppingListProperties).LowerFirstChar(),
                opt => opt.MapFrom(src => new IngredientShoppingListProperties(
                        new ItemId(src.DefaultItemId!.Value),
                        new ItemTypeId(src.DefaultItemTypeId!.Value),
                        new StoreId(src.DefaultStoreId!.Value),
                        src.AddToShoppingListByDefault!.Value)));
    }
}