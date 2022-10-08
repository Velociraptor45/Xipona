using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Validations;

public interface IItemValidationService
{
    Task ValidateAsync(ItemId itemId, ItemTypeId? itemTypeId);
}