using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Creations;

public interface IItemCategoryCreationService
{
    Task<IItemCategory> CreateAsync(ItemCategoryName name);
}