using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services
{
    public class AvailabilityValidationService : IAvailabilityValidationService
    {
        private readonly IStoreRepository storeRepository;

        public AvailabilityValidationService(IStoreRepository storeRepository)
        {
            this.storeRepository = storeRepository;
        }

        public async Task ValidateAsync(IEnumerable<IStoreItemAvailability> availabilities, CancellationToken cancellationToken)
        {
            if (availabilities is null)
                throw new ArgumentNullException(nameof(availabilities));

            var availabilitiesList = availabilities.ToList();

            var storeIds = availabilitiesList.Select(av => av.StoreId);
            if (!storeIds.SequenceEqual(storeIds.Distinct()))
                throw new DomainException(new MultipleAvailabilitiesForStoreReason());

            var storesDict = (await storeRepository.FindByAsync(storeIds, cancellationToken))
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
}