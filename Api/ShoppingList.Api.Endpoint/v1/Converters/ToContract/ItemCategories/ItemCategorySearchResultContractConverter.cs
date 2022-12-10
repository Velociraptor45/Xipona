using ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Queries;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ItemCategories;

public class ItemCategorySearchResultContractConverter :
    IToContractConverter<ItemCategorySearchResultReadModel, ItemCategorySearchResultContract>
{
    public ItemCategorySearchResultContract ToContract(ItemCategorySearchResultReadModel source)
    {
        return new ItemCategorySearchResultContract(source.Id, source.Name);
    }
}