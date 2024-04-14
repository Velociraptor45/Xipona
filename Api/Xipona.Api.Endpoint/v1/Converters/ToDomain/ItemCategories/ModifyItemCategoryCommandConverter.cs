using ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;
using ProjectHermes.Xipona.Api.Contracts.ItemCategories.Commands;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;

public class ModifyItemCategoryCommandConverter :
    IToDomainConverter<ModifyItemCategoryContract, ModifyItemCategoryCommand>
{
    public ModifyItemCategoryCommand ToDomain(ModifyItemCategoryContract source)
    {
        return new ModifyItemCategoryCommand(
            new ItemCategoryModification(
                new ItemCategoryId(source.ItemCategoryId),
                new ItemCategoryName(source.Name)));
    }
}