using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.DeleteItemCategory
{
    public class DeleteItemCategoryCommandHandler : ICommandHandler<DeleteItemCategoryCommand, bool>
    {
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IItemRepository itemRepository;
        private readonly ITransactionGenerator transactionGenerator;

        public DeleteItemCategoryCommandHandler(IItemCategoryRepository itemCategoryRepository,
            IItemRepository itemRepository, ITransactionGenerator transactionGenerator)
        {
            this.itemCategoryRepository = itemCategoryRepository;
            this.itemRepository = itemRepository;
            this.transactionGenerator = transactionGenerator;
        }

        public async Task<bool> HandleAsync(DeleteItemCategoryCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new System.ArgumentNullException(nameof(command));
            }

            var category = await itemCategoryRepository.FindByAsync(command.ItemCategoryId, cancellationToken);
            if (category == null)
                throw new DomainException(new ItemCategoryNotFoundReason(command.ItemCategoryId));

            category.Delete();

            var storeItems = (await itemRepository.FindActiveByAsync(command.ItemCategoryId, cancellationToken))
                .ToList();

            using var transaction = await transactionGenerator.GenerateAsync(cancellationToken);
            await itemCategoryRepository.StoreAsync(category, cancellationToken);
            foreach (var item in storeItems)
            {
                item.Delete();
                await itemRepository.StoreAsync(item, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
            return true;
        }
    }
}