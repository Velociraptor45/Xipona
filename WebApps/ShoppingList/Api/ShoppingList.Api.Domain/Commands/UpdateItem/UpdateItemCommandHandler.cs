using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.UpdateItem
{
    public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        public UpdateItemCommandHandler(IItemRepository itemRepository, IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task<bool> HandleAsync(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (!await itemRepository.IsValidIdAsync(command.ItemUpdate.Id, cancellationToken))
                throw new ItemNotFoundException(command.ItemUpdate.Id);

            cancellationToken.ThrowIfCancellationRequested();

            var itemCategory = await itemCategoryRepository
                .FindByAsync(command.ItemUpdate.ItemCategoryId, cancellationToken);
            var manufacturer = await manufacturerRepository
                .FindByAsync(command.ItemUpdate.ManufacturerId, cancellationToken);

            var storeItem = command.ItemUpdate.ToStoreItem(itemCategory, manufacturer);

            await itemRepository.StoreAsync(storeItem, cancellationToken);

            return true;
        }
    }
}