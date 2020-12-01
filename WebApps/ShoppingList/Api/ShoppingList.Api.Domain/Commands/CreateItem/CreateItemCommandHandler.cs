using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.CreateItem
{
    public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;

        public CreateItemCommandHandler(IItemRepository itemRepository, IManufacturerRepository manufacturerRepository,
            IItemCategoryRepository itemCategoryRepository)
        {
            this.itemRepository = itemRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.itemCategoryRepository = itemCategoryRepository;
        }

        public async Task<bool> HandleAsync(CreateItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            ItemCategory itemCategory = await itemCategoryRepository
                .FindByAsync(command.ItemCreation.ItemCategoryId, cancellationToken);

            Manufacturer manufacturer = null;
            if (command.ItemCreation.ManufacturerId != null)
                manufacturer = await manufacturerRepository
                    .FindByAsync(command.ItemCreation.ManufacturerId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var storeItem = command.ItemCreation.ToStoreItem(itemCategory, manufacturer);

            await itemRepository.StoreAsync(storeItem, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return true;
        }
    }
}