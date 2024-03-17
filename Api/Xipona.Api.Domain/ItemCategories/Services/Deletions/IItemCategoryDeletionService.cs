using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Deletions;

public interface IItemCategoryDeletionService
{
    Task DeleteAsync(ItemCategoryId itemCategoryId);
}