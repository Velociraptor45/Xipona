using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.ChangeItem
{
    public class ChangeItemCommandHandler : ICommandHandler<ChangeItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        public ChangeItemCommandHandler(IItemRepository itemRepository, IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task<bool> HandleAsync(ChangeItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (!await itemRepository.IsValidIdAsync(command.ItemChange.Id, cancellationToken))
                throw new ItemNotFoundException(command.ItemChange.Id);

            cancellationToken.ThrowIfCancellationRequested();

            ItemCategory itemCategory = await itemCategoryRepository
                .FindByAsync(command.ItemChange.ItemCategoryId, cancellationToken);
            Manufacturer manufacturer = await manufacturerRepository
                .FindByAsync(command.ItemChange.ManufacturerId, cancellationToken);

            var storeItem = command.ItemChange.ToStoreItem(itemCategory, manufacturer);

            await itemRepository.StoreAsync(storeItem, cancellationToken);

            return true;
        }
    }
}