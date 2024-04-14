using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Validations;

public interface IItemValidationService
{
    Task ValidateAsync(ItemId itemId, ItemTypeId? itemTypeId);
}