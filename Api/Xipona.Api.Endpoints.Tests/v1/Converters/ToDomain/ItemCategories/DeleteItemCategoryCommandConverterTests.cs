using AutoMapper;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;
using ProjectHermes.Xipona.Api.TestTools.Extensions;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Converters.ToDomain.ItemCategories;

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