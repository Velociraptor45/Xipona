using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Items;

public class SearchItemByItemCategoryResultContractConverter :
    IToContractConverter<SearchItemByItemCategoryResult, SearchItemByItemCategoryResultContract>
{
    private readonly IToContractConverter<ItemAvailabilityReadModel, SearchItemByItemCategoryAvailabilityContract>
        _availabilityConverter;

    public SearchItemByItemCategoryResultContractConverter(
        IToContractConverter<ItemAvailabilityReadModel, SearchItemByItemCategoryAvailabilityContract> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public SearchItemByItemCategoryResultContract ToContract(SearchItemByItemCategoryResult source)
    {
        return new SearchItemByItemCategoryResultContract(
            source.Id,
            source.ItemTypeId,
            source.Name,
            source.Availabilities.Select(_availabilityConverter.ToContract));
    }
}