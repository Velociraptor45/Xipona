using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

public class ItemCategory : AggregateRoot, IItemCategory
{
    public ItemCategory(ItemCategoryId id, ItemCategoryName name, bool isDeleted)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
    }

    public ItemCategoryId Id { get; }
    public ItemCategoryName Name { get; private set; }
    public bool IsDeleted { get; private set; }

    public void Delete()
    {
        IsDeleted = true;
    }

    public void Modify(ItemCategoryModification modification)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedItemCategoryReason(Id));

        Name = modification.Name;
    }
}