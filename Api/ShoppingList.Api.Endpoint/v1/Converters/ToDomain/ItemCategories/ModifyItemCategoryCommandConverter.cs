using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Commands;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.ItemCategories;

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