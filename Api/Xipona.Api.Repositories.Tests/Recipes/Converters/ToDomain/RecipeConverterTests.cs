using AutoMapper;
using ProjectHermes.Xipona.Api.Core.TestKit.Services;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models.Factories;
using ProjectHermes.Xipona.Api.Repositories.Recipes.Converters.ToDomain;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Recipes.Entities;
using ProjectHermes.Xipona.Api.TestTools.Extensions;
using Recipe = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Recipe;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Recipes.Converters.ToDomain;

public class RecipeConverterTests : ToDomainConverterTestBase<Recipe, IRecipe, RecipeConverter>
{
    private readonly DateTimeServiceMock _dateTimeServiceMock = new(MockBehavior.Strict);

    public override RecipeConverter CreateSut()
    {
        var validatorMock = new ValidatorMock(MockBehavior.Strict);
        return new RecipeConverter(
            t => new RecipeFactory(
                new IngredientFactory(validatorMock.Object),
                validatorMock.Object,
                new PreparationStepFactory(),
                _dateTimeServiceMock.Object),
            new IngredientConverter(t2 => new IngredientFactory(validatorMock.Object)),
            new PreparationStepConverter(new PreparationStepFactory()));
    }

    protected override Recipe CreateSource()
    {
        return new RecipeEntityBuilder().Create();
    }

    protected override void AddMapping(IMappingExpression<Recipe, IRecipe> mapping)
    {
        mapping.As<Domain.Recipes.Models.Recipe>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Recipe, Domain.Recipes.Models.Recipe>()
            .ForCtorParam(nameof(IRecipe.Id).LowerFirstChar(), opt => opt.MapFrom(src => new RecipeId(src.Id)))
            .ForCtorParam(nameof(IRecipe.Name).LowerFirstChar(), opt => opt.MapFrom(src => src.Name))
            .ForCtorParam(nameof(IRecipe.NumberOfServings).LowerFirstChar(), opt => opt.MapFrom(src => new NumberOfServings(src.NumberOfServings)))
            .ForCtorParam(nameof(IRecipe.CreatedAt).LowerFirstChar(), opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
            .ForCtorParam(nameof(IRecipe.Ingredients).LowerFirstChar(),
                opt => opt.MapFrom((src, ctx) =>
                    new Ingredients(
                        src.Ingredients.Select(i => ctx.Mapper.Map<Ingredient>(i)),
                        new IngredientFactoryMock(MockBehavior.Strict).Object)))
            .ForCtorParam("steps",
                opt => opt.MapFrom((src, ctx) =>
                    new PreparationSteps(
                        src.PreparationSteps.Select(p => ctx.Mapper.Map<PreparationStep>(p)),
                        new PreparationStepFactoryMock(MockBehavior.Strict).Object)))
            .ForCtorParam(nameof(IRecipe.Tags).LowerFirstChar(),
                opt => opt.MapFrom(src =>
                    new Domain.Recipes.Models.RecipeTags(
                        src.Tags.Select(r => new RecipeTagId(r.RecipeTagId)))))
            .ForMember(dest => dest.PreparationSteps, opt => opt.Ignore())
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

        new IngredientConverterTests().AddMapping(cfg);
        new PreparationStepConverterTests().AddMapping(cfg);
    }
}