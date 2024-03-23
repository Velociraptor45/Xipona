using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.DomainEvents;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Reasons;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Modifications;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

public class ItemCategory : AggregateRoot, IItemCategory
{
    public ItemCategory(ItemCategoryId id, ItemCategoryName name, bool isDeleted, DateTimeOffset createdAt)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
        CreatedAt = createdAt;
    }

    public ItemCategoryId Id { get; }
    public ItemCategoryName Name { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; }

    public void Delete()
    {
        if (IsDeleted)
            return;

        IsDeleted = true;
        PublishDomainEvent(new ItemCategoryDeletedDomainEvent(Id));
    }

    public void Modify(ItemCategoryModification modification)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedItemCategoryReason(Id));

        Name = modification.Name;
    }
}