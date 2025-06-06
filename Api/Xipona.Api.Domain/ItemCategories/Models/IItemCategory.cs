using Xipona.Api.Domain.ItemCategories.Services.Modifications;

namespace Xipona.Api.Domain.ItemCategories.Models;

public interface IItemCategory
{
    ItemCategoryId Id { get; }
    ItemCategoryName Name { get; }
    bool IsDeleted { get; }
    DateTimeOffset CreatedAt { get; }

    void Delete();

    void Modify(ItemCategoryModification modification);
}