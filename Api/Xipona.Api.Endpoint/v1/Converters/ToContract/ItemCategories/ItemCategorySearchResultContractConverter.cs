using ProjectHermes.Xipona.Api.Contracts.ItemCategories.Queries;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Queries;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.ItemCategories;

public class ItemCategorySearchResultContractConverter :
    IToContractConverter<ItemCategorySearchResultReadModel, ItemCategorySearchResultContract>
{
    public ItemCategorySearchResultContract ToContract(ItemCategorySearchResultReadModel source)
    {
        return new ItemCategorySearchResultContract(source.Id, source.Name);
    }
}