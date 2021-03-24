using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services
{
    public class AvailabilityValidationService : IAvailabilityValidationService
    {
        public async Task Validate(IEnumerable<IStoreItemAvailability> availabilities, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}