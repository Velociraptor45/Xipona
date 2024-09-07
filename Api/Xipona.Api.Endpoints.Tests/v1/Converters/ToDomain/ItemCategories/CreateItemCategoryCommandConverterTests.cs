using AutoMapper;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;
using ProjectHermes.Xipona.Api.TestTools.Extensions;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Converters.ToDomain.ItemCategories;

public class CreateItemCategoryCommandConverterTests :
    ToDomainConverterTestBase<string, CreateItemCategoryCommand, CreateItemCategoryCommandConverter>
{
    public override CreateItemCategoryCommandConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<string, CreateItemCategoryCommand> mapping)
    {
        mapping
            .ForCtorParam(nameof(CreateItemCategoryCommand.Name).LowerFirstChar(), opt => opt.MapFrom(src => src));
    }
}