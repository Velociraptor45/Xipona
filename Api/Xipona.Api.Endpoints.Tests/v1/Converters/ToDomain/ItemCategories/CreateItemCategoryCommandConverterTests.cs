using AutoMapper;
using Xipona.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;
using Xipona.Api.TestTools.Extensions;

namespace Xipona.Api.Endpoints.Tests.v1.Converters.ToDomain.ItemCategories;

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