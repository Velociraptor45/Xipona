using AutoMapper;
using Xipona.Api.Core.TestKit.Services;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Domain.RecipeTags.Models;
using Xipona.Api.Domain.RecipeTags.Models.Factories;
using Xipona.Api.Repositories.RecipeTags.Converters.ToDomain;
using Xipona.Api.TestTools.Extensions;
using RecipeTag = Xipona.Api.Repositories.RecipeTags.Entities.RecipeTag;

namespace Xipona.Api.Repositories.Tests.RecipeTags.Converter.ToDomain;

public class RecipeTagConverterTests : ToDomainConverterTestBase<RecipeTag, IRecipeTag, RecipeTagConverter>
{
    private readonly DateTimeServiceMock _dateTimeServiceMock = new(MockBehavior.Strict);

    public override RecipeTagConverter CreateSut()
    {
        return new RecipeTagConverter(new RecipeTagFactory(_dateTimeServiceMock.Object));
    }

    protected override void AddMapping(IMappingExpression<RecipeTag, IRecipeTag> mapping)
    {
        mapping.As<Domain.RecipeTags.Models.RecipeTag>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<RecipeTag, Domain.RecipeTags.Models.RecipeTag>()
            .ForCtorParam(nameof(IRecipeTag.Id).LowerFirstChar(), opt => opt.MapFrom(src => new RecipeTagId(src.Id)))
            .ForCtorParam(nameof(IRecipeTag.Name).LowerFirstChar(), opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());
    }

    protected override void Setup(RecipeTag source)
    {
        _dateTimeServiceMock.SetupUtcNow(source.CreatedAt);
    }
}