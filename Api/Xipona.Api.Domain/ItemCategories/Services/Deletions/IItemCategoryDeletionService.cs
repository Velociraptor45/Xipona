using Xipona.Api.Domain.ItemCategories.Models;

namespace Xipona.Api.Domain.ItemCategories.Services.Deletions;

public interface IItemCategoryDeletionService
{
    Task DeleteAsync(ItemCategoryId itemCategoryId);
}