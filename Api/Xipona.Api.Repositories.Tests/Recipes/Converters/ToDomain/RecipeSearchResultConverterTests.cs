using AutoMapper;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Converters.ToDomain;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Recipes.Entities;
using ProjectHermes.Xipona.Api.TestTools.Extensions;
using Recipe = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Recipe;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Recipes.Converters.ToDomain;

public class RecipeSearchResultConverterTests
    : ToDomainConverterTestBase<Recipe, RecipeSearchResult, RecipeSearchResultConverter>
{
    public override RecipeSearchResultConverter CreateSut()
    {
        return new();
    }

    protected override Recipe CreateSource()
    {
        return new RecipeEntityBuilder().Create();
    }

    protected override void AddMapping(IMappingExpression<Recipe, RecipeSearchResult> mapping)
    {
        mapping
            .ForCtorParam(nameof(RecipeSearchResult.Id).LowerFirstChar(), opt => opt.MapFrom(src => new RecipeId(src.Id)))
            .ForCtorParam(nameof(RecipeSearchResult.Name).LowerFirstChar(), opt => opt.MapFrom(src => new RecipeName(src.Name)));
    }
}