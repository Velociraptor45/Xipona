using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Services.Validations;

public interface IAvailabilityValidationService
{
    Task ValidateAsync(IEnumerable<ItemAvailability> availabilities);
}