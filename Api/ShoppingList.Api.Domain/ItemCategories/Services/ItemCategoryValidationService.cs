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
        private readonly IItemCategoryRepository itemCategoryRepository;

        public ItemCategoryValidationService(IItemCategoryRepository itemCategoryRepository)
        {
            this.itemCategoryRepository = itemCategoryRepository;
        }

        public async Task Validate(ItemCategoryId itemCategoryId, CancellationToken cancellationToken)
        {
            if (itemCategoryId is null)
                throw new System.ArgumentNullException(nameof(itemCategoryId));

            IItemCategory itemCategory = await itemCategoryRepository
                .FindByAsync(itemCategoryId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            if (itemCategory == null)
                throw new DomainException(new ItemCategoryNotFoundReason(itemCategoryId));
        }
    }
}