using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validations;

public interface IAvailabilityValidationService
{
    Task ValidateAsync(IEnumerable<IStoreItemAvailability> availabilities, CancellationToken cancellationToken);
}