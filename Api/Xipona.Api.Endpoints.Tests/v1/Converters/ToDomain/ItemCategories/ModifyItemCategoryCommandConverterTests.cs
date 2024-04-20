using AutoMapper;
using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;
using ProjectHermes.Xipona.Api.Contracts.ItemCategories.Commands;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Modifications;
using ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.v1.Converters.ToDomain.ItemCategories;

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