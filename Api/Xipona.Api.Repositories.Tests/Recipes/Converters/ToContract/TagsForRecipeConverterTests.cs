using AutoMapper;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Converters.ToContract;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Recipes.Converters.ToContract;

public class TagsForRecipeConverterTests : ToContractConverterTestBase<(RecipeId, RecipeTagId), TagsForRecipe, TagsForRecipeConverter>
{
    protected override TagsForRecipeConverter CreateSut()
    {
        return new TagsForRecipeConverter();
    }

    protected override void AddMapping(IMappingExpression<(RecipeId, RecipeTagId), TagsForRecipe> mapping)
    {
        mapping
            .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.Item1.Value))
            .ForMember(dest => dest.RecipeTagId, opt => opt.MapFrom(src => src.Item2.Value))
            .ForMember(dest => dest.Recipe, opt => opt.Ignore());
    }
}