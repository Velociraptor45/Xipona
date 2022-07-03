using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Validations;

public interface IAvailabilityValidationService
{
    Task ValidateAsync(IEnumerable<IItemAvailability> availabilities, CancellationToken cancellationToken);
}