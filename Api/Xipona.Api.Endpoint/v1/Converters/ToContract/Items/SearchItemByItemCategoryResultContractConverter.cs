﻿using ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Items;

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
            source.ManufacturerName?.Value,
            source.Availabilities.Select(_availabilityConverter.ToContract));
    }
}