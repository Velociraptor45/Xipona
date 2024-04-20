using ProjectHermes.Xipona.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Recipes.ToContract;
using ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Converters.Recipes.ToContract;

public class CreateRecipeContractConverterTests
    : ToContractConverterBase<EditedRecipe, CreateRecipeContract, CreateRecipeContractConverter>
{
    protected override CreateRecipeContractConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<EditedRecipe, CreateRecipeContract> mapping)
    {
        mapping
            .ForCtorParam(nameof(CreateRecipeContract.Name), opt => opt.MapFrom(src => src.Name))
            .ForCtorParam(nameof(CreateRecipeContract.NumberOfServings), opt => opt.MapFrom(src => src.NumberOfServings))
            .ForCtorParam(nameof(CreateRecipeContract.Ingredients), opt => opt.MapFrom(src => src.Ingredients))
            .ForCtorParam(nameof(CreateRecipeContract.PreparationSteps), opt => opt.MapFrom(src => src.PreparationSteps))
            .ForCtorParam(nameof(CreateRecipeContract.RecipeTagIds), opt => opt.MapFrom(src => src.RecipeTagIds));
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<EditedIngredient, CreateIngredientContract>()
            .ForCtorParam(nameof(CreateIngredientContract.ItemCategoryId), opt => opt.MapFrom(src => src.ItemCategoryId))
            .ForCtorParam(nameof(CreateIngredientContract.QuantityType), opt => opt.MapFrom(src => src.QuantityTypeId))
            .ForCtorParam(nameof(CreateIngredientContract.Quantity), opt => opt.MapFrom(src => src.Quantity))
            .ForCtorParam(nameof(CreateIngredientContract.DefaultItemId), opt => opt.MapFrom(src => src.DefaultItemId))
            .ForCtorParam(nameof(CreateIngredientContract.DefaultItemTypeId), opt => opt.MapFrom(src => src.DefaultItemTypeId))
            .ForCtorParam(nameof(CreateIngredientContract.DefaultStoreId), opt => opt.MapFrom(src => src.DefaultStoreId))
            .ForCtorParam(nameof(CreateIngredientContract.AddToShoppingListByDefault),
                opt => opt.MapFrom(src => src.AddToShoppingListByDefault));

        cfg.CreateMap<EditedPreparationStep, CreatePreparationStepContract>()
            .ForCtorParam(nameof(CreatePreparationStepContract.Instruction), opt => opt.MapFrom(src => src.Name))
            .ForCtorParam(nameof(CreatePreparationStepContract.SortingIndex), opt => opt.MapFrom(src => src.SortingIndex));
    }

    protected override void Customize(IFixture fixture)
    {
        fixture.Customize(new EditedPreparationStepsCustomization());
    }
}