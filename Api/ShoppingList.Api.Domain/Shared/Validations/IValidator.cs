using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

public interface IValidator
{
    Task ValidateAsync(IEnumerable<IItemAvailability> availabilities);

    Task ValidateAsync(ItemCategoryId itemCategoryId);

    Task ValidateAsync(ManufacturerId manufacturerId);
}