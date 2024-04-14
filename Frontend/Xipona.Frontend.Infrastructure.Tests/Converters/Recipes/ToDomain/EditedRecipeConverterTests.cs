using ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Recipes.ToDomain;
using ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Converters.Recipes.ToDomain;

public class EditedRecipeConverterTests : ToDomainConverterBase<RecipeContract, EditedRecipe, EditedRecipeConverter>
{
    protected override EditedRecipeConverter CreateSut()
    {
        return new(
            new EditedIngredientConverter(),
            new EditedPreparationStepConverter());
    }

    protected override void AddMapping(IMappingExpression<RecipeContract, EditedRecipe> mapping)
    {
        mapping
            .ConvertUsing<Converter>();
        // Mapping a SortedSet fails, see https://stackoverflow.com/questions/76140294/automapper-fails-when-mapping-sortedset-as-ctor-argument
        //.ForCtorParam(nameof(EditedRecipe.Id), opt => opt.MapFrom(src => src.Id))
        //.ForCtorParam(nameof(EditedRecipe.Name), opt => opt.MapFrom(src => src.Name))
        //.ForCtorParam(nameof(EditedRecipe.NumberOfServings), opt => opt.MapFrom(src => src.NumberOfServings))
        //.ForCtorParam(nameof(EditedRecipe.Ingredients), opt => opt.MapFrom(src => src.Ingredients.ToList()))
        //.ForCtorParam(nameof(EditedRecipe.PreparationSteps), opt => opt.MapFrom((src, ctx) =>
        //    new SortedSet<EditedPreparationStep>(
        //        ctx.Mapper.Map<List<EditedPreparationStep>>(src.PreparationSteps.ToList()),
        //        new SortingIndexComparer())))
        //.ForCtorParam(nameof(EditedRecipe.RecipeTagIds), opt => opt.MapFrom(src => src.RecipeTagIds.ToList()));
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        new EditedIngredientConverterTests().AddMapping(cfg);
        new EditedPreparationStepConverterTests().AddMapping(cfg);
    }

    private sealed class Converter : ITypeConverter<RecipeContract, EditedRecipe>
    {
        public EditedRecipe Convert(RecipeContract source, EditedRecipe destination, ResolutionContext context)
        {
            return new EditedRecipe(
                source.Id,
                source.Name,
                source.NumberOfServings,
                context.Mapper.Map<List<EditedIngredient>>(source.Ingredients.ToList()),
                new SortedSet<EditedPreparationStep>(
                    context.Mapper.Map<List<EditedPreparationStep>>(source.PreparationSteps.ToList()),
                    new SortingIndexComparer()),
                source.RecipeTagIds.ToList());
        }
    }
}