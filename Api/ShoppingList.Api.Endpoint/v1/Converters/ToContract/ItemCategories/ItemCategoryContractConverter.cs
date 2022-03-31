using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ItemCategories;

public class ItemCategoryContractConverter :
    IToContractConverter<ItemCategoryReadModel, ItemCategoryContract>,
    IToContractConverter<IItemCategory, ItemCategoryContract>
{
    public ItemCategoryContract ToContract(ItemCategoryReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new ItemCategoryContract(source.Id.Value, source.Name.Value, source.IsDeleted);
    }

    public ItemCategoryContract ToContract(IItemCategory source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new ItemCategoryContract(source.Id.Value, source.Name.Value, source.IsDeleted);
    }
}