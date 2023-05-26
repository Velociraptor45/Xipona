using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Validations;

public class ItemCategoryValidationService : IItemCategoryValidationService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;

    public ItemCategoryValidationService(
        Func<CancellationToken, IItemCategoryRepository> itemCategoryRepositoryDelegate,
        CancellationToken cancellationToken)
    {
        _itemCategoryRepository = itemCategoryRepositoryDelegate(cancellationToken);
    }

    public async Task ValidateAsync(ItemCategoryId itemCategoryId)
    {
        IItemCategory? itemCategory = await _itemCategoryRepository
            .FindActiveByAsync(itemCategoryId);

        if (itemCategory == null)
            throw new DomainException(new ItemCategoryNotFoundReason(itemCategoryId));
    }
}