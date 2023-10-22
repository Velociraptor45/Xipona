using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Validations;

public class AvailabilityValidationService : IAvailabilityValidationService
{
    private readonly IStoreRepository _storeRepository;

    public AvailabilityValidationService(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task ValidateAsync(IEnumerable<ItemAvailability> availabilities)
    {
        var availabilitiesList = availabilities.ToList();

        var storeIds = availabilitiesList.Select(av => av.StoreId).ToList();
        if (!storeIds.SequenceEqual(storeIds.Distinct()))
            throw new DomainException(new MultipleAvailabilitiesForStoreReason());

        var storesDict = (await _storeRepository.FindActiveByAsync(storeIds))
            .ToDictionary(s => s.Id);

        foreach (var availability in availabilitiesList)
        {
            if (!storesDict.TryGetValue(availability.StoreId, out IStore? store))
                throw new DomainException(new StoreNotFoundReason(availability.StoreId));
            if (!store.ContainsSection(availability.DefaultSectionId))
                throw new DomainException(new SectionInStoreNotFoundReason(availability.DefaultSectionId, store.Id));
        }
    }
}