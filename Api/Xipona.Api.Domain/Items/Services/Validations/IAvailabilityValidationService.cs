using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Validations;

public interface IAvailabilityValidationService
{
    Task ValidateAsync(IEnumerable<ItemAvailability> availabilities);
}