﻿using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Stores.Ports;
using ProjectHermes.Xipona.Api.Domain.Stores.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Conversion;

public class ItemAvailabilityReadModelConversionService : IItemAvailabilityReadModelConversionService
{
    private readonly IStoreRepository _storeRepository;

    public ItemAvailabilityReadModelConversionService(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task<IDictionary<(ItemId, ItemTypeId?), IEnumerable<ItemAvailabilityReadModel>>> ConvertAsync(
        IEnumerable<IItem> items)
    {
        var availabilities = new Dictionary<(ItemId, ItemTypeId?), IEnumerable<ItemAvailability>>();
        foreach (IItem item in items)
        {
            if (item.HasItemTypes)
            {
                foreach (IItemType type in item.ItemTypes)
                {
                    availabilities.Add((item.Id, type.Id), type.Availabilities);
                }
            }
            else
            {
                availabilities.Add((item.Id, null), item.Availabilities);
            }
        }

        return await ConvertAsync(availabilities);
    }

    public async Task<IDictionary<(ItemId, ItemTypeId?), IEnumerable<ItemAvailabilityReadModel>>> ConvertAsync(
        IDictionary<(ItemId, ItemTypeId?), IEnumerable<ItemAvailability>> availabilitiesDict)
    {
        var availabilitiesList = availabilitiesDict.ToList();
        var storeIds = availabilitiesDict.Values
            .SelectMany(av => av)
            .Select(av => av.StoreId)
            .Distinct()
            .ToList();

        var stores = (await _storeRepository.FindActiveByAsync(storeIds)).ToDictionary(s => s.Id);

        var missingStoreIds = storeIds.Except(stores.Select(s => s.Value.Id)).ToList();
        if (missingStoreIds.Count != 0)
        {
            throw new DomainException(new StoresNotFoundReason(missingStoreIds));
        }

        var results = new Dictionary<(ItemId, ItemTypeId?), IEnumerable<ItemAvailabilityReadModel>>();
        foreach (var availabilityPair in availabilitiesList)
        {
            var (key, availabilities) = availabilityPair;
            var readModels = new List<ItemAvailabilityReadModel>();
            foreach (var availability in availabilities)
            {
                var store = stores[availability.StoreId];
                var section = store.Sections.First(s => s.Id == availability.DefaultSectionId);

                readModels.Add(new ItemAvailabilityReadModel(availability, store, section));
            }

            results.Add(key, readModels);
        }

        return results;
    }
}