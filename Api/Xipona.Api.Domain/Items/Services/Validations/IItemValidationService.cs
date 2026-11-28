using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Services.Validations;

public interface IItemValidationService
{
    Task ValidateAsync(ItemId itemId, ItemTypeId? itemTypeId);
}