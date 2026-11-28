using Xipona.Api.Contracts.ItemCategories.Queries;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.ItemCategories.Services.Queries;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.ItemCategories;

public class ItemCategorySearchResultContractConverter :
    IToContractConverter<ItemCategorySearchResultReadModel, ItemCategorySearchResultContract>
{
    public ItemCategorySearchResultContract ToContract(ItemCategorySearchResultReadModel source)
    {
        return new ItemCategorySearchResultContract(source.Id, source.Name);
    }
}