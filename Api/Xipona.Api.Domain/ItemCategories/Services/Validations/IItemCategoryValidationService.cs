using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Validations;

public interface IItemCategoryValidationService
{
    Task ValidateAsync(ItemCategoryId itemCategoryId);
}