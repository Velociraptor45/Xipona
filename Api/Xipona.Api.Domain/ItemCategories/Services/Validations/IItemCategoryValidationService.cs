using Xipona.Api.Domain.ItemCategories.Models;

namespace Xipona.Api.Domain.ItemCategories.Services.Validations;

public interface IItemCategoryValidationService
{
    Task ValidateAsync(ItemCategoryId itemCategoryId);
}