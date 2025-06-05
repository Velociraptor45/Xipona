using Xipona.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;
using Xipona.Api.Contracts.ItemCategories.Commands;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.ItemCategories.Services.Modifications;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;

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