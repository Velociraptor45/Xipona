using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services
{
    public interface IAvailabilityValidationService
    {
        Task ValidateAsync(IEnumerable<IStoreItemAvailability> availabilities, CancellationToken cancellationToken);
    }
}