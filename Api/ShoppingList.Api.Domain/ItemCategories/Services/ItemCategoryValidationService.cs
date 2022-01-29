using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services
{
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
}