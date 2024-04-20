using AutoMapper;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Converters.ToDomain;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Recipes.Entities;
using ProjectHermes.Xipona.Api.TestTools.Extensions;
using Ingredient = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Ingredient;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Recipes.Converters.ToDomain;

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