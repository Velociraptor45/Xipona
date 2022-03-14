using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Validations;

public class ItemCategoryValidationService : IItemCategoryValidationService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;

    public ItemCategoryValidationService(IItemCategoryRepository itemCategoryRepository)
    {
        _itemCategoryRepository = itemCategoryRepository;
    }

    public async Task ValidateAsync(ItemCategoryId itemCategoryId, CancellationToken cancellationToken)
    {
        IItemCategory? itemCategory = await _itemCategoryRepository
            .FindByAsync(itemCategoryId, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (itemCategory == null)
            throw new DomainException(new ItemCategoryNotFoundReason(itemCategoryId));
    }
}