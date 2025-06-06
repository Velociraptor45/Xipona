using Xipona.Api.Domain.ItemCategories.Models;

namespace Xipona.Api.Domain.ItemCategories.Services.Creations;

public interface IItemCategoryCreationService
{
    Task<IItemCategory> CreateAsync(ItemCategoryName name);
}