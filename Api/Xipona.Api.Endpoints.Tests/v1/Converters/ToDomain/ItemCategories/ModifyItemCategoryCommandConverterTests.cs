using AutoMapper;
using Xipona.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;
using Xipona.Api.Contracts.ItemCategories.Commands;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.ItemCategories.Services.Modifications;
using Xipona.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;

namespace Xipona.Api.Endpoints.Tests.v1.Converters.ToDomain.ItemCategories;

public class ModifyItemCategoryCommandConverterTests :
    ToDomainConverterTestBase<ModifyItemCategoryContract, ModifyItemCategoryCommand, ModifyItemCategoryCommandConverter>
{
    public override ModifyItemCategoryCommandConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<ModifyItemCategoryContract, ModifyItemCategoryCommand> mapping)
    {
        mapping
            .ForCtorParam(nameof(ModifyItemCategoryCommand.Modification),
                opt => opt.MapFrom(src => new ItemCategoryModification(
                    new ItemCategoryId(src.ItemCategoryId),
                    new ItemCategoryName(src.Name))));
    }
}