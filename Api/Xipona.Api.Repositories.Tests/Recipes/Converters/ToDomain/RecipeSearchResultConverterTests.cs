using AutoMapper;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Services.Queries;
using Xipona.Api.Repositories.Recipes.Converters.ToDomain;
using Xipona.Api.Repositories.TestKit.Recipes.Entities;
using Xipona.Api.TestTools.Extensions;
using Recipe = Xipona.Api.Repositories.Recipes.Entities.Recipe;

namespace Xipona.Api.Repositories.Tests.Recipes.Converters.ToDomain;

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