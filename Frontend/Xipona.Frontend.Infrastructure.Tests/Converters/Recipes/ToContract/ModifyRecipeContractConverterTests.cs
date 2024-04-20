using ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Recipes.ToContract;
using ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Recipes.States;
using ProjectHermes.Xipona.Frontend.TestTools.Extensions;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Converters.Recipes.ToContract;

public class ModifyRecipeContractConverterTests
    : ToContractConverterBase<EditedRecipe, ModifyRecipeContract, ModifyRecipeContractConverter>
{
    protected override ModifyRecipeContractConverter CreateSut()
    {
        return new();
    }

    protected override void Customize(IFixture fixture)
    {
        fixture.Customize(new EditedPreparationStepsCustomization());
    }

    protected override void AddMapping(IMappingExpression<EditedRecipe, ModifyRecipeContract> mapping)
    {
        mapping
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.NumberOfServings, opt => opt.MapFrom(src => src.NumberOfServings))
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients))
            .ForMember(dest => dest.PreparationSteps, opt => opt.MapFrom(src => src.PreparationSteps.ToList()))
            .ForMember(dest => dest.RecipeTagIds, opt => opt.MapFrom(src => src.RecipeTagIds));
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<EditedPreparationStep, ModifyPreparationStepContract>()
            .ForCtorParam(nameof(ModifyPreparationStepContract.Id).LowerFirstChar(), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(ModifyPreparationStepContract.Instruction).LowerFirstChar(), opt => opt.MapFrom(src => src.Name))
            .ForCtorParam(nameof(ModifyPreparationStepContract.SortingIndex).LowerFirstChar(), opt => opt.MapFrom(src => src.SortingIndex));

        cfg.CreateMap<EditedIngredient, ModifyIngredientContract>()
            .ForCtorParam(nameof(ModifyIngredientContract.Id).LowerFirstChar(), opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(ModifyIngredientContract.ItemCategoryId).LowerFirstChar(), opt => opt.MapFrom(src => src.ItemCategoryId))
            .ForCtorParam(nameof(ModifyIngredientContract.QuantityType).LowerFirstChar(), opt => opt.MapFrom(src => src.QuantityTypeId))
            .ForCtorParam(nameof(ModifyIngredientContract.Quantity).LowerFirstChar(), opt => opt.MapFrom(src => src.Quantity))
            .ForCtorParam(nameof(ModifyIngredientContract.DefaultItemId).LowerFirstChar(), opt => opt.MapFrom(src => src.DefaultItemId))
            .ForCtorParam(nameof(ModifyIngredientContract.DefaultItemTypeId).LowerFirstChar(), opt => opt.MapFrom(src => src.DefaultItemTypeId))
            .ForCtorParam(nameof(ModifyIngredientContract.DefaultStoreId).LowerFirstChar(), opt => opt.MapFrom(src => src.DefaultStoreId))
            .ForCtorParam(nameof(ModifyIngredientContract.AddToShoppingListByDefault).LowerFirstChar(), opt => opt.MapFrom(src => src.AddToShoppingListByDefault));
    }
}