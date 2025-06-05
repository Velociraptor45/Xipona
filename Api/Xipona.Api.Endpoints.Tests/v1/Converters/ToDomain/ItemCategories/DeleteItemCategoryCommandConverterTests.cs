using AutoMapper;
using Xipona.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;
using Xipona.Api.TestTools.Extensions;

namespace Xipona.Api.Endpoints.Tests.v1.Converters.ToDomain.ItemCategories;

public class DeleteItemCategoryCommandConverterTests :
    ToDomainConverterTestBase<Guid, DeleteItemCategoryCommand, DeleteItemCategoryCommandConverter>
{
    public override DeleteItemCategoryCommandConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<Guid, DeleteItemCategoryCommand> mapping)
    {
        mapping
            .ForCtorParam(nameof(DeleteItemCategoryCommand.ItemCategoryId).LowerFirstChar(),
                opt => opt.MapFrom(src => src));
    }
}