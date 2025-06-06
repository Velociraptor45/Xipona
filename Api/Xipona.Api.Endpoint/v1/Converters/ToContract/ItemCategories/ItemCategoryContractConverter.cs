using Xipona.Api.Contracts.Common.Queries;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.ItemCategories.Services.Shared;

namespace Xipona.Api.Endpoint.v1.Converters.ToContract.ItemCategories;

public class ItemCategoryContractConverter :
    IToContractConverter<ItemCategoryReadModel, ItemCategoryContract>,
    IToContractConverter<IItemCategory, ItemCategoryContract>
{
    public ItemCategoryContract ToContract(ItemCategoryReadModel source)
    {
        return new ItemCategoryContract(source.Id, source.Name, source.IsDeleted);
    }

    public ItemCategoryContract ToContract(IItemCategory source)
    {
        return new ItemCategoryContract(source.Id, source.Name, source.IsDeleted);
    }
}