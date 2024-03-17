using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Shared;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.ItemCategories;

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