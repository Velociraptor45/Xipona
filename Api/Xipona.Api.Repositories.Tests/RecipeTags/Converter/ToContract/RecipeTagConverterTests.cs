using AutoMapper;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Repositories.RecipeTags.Converters.ToContract;
using RecipeTag = ProjectHermes.Xipona.Api.Repositories.RecipeTags.Entities.RecipeTag;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.RecipeTags.Converter.ToContract;

public class RecipeTagConverterTests : ToContractConverterTestBase<IRecipeTag, RecipeTag, RecipeTagConverter>
{
    protected override RecipeTagConverter CreateSut()
    {
        return new RecipeTagConverter();
    }

    protected override void AddMapping(IMappingExpression<IRecipeTag, RecipeTag> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => ((AggregateRoot)src).RowVersion));
    }
}