using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.MakeTemporaryItemPermanent
{
    public class MakeTemporaryItemPermanentCommandHandler : ICommandHandler<MakeTemporaryItemPermanentCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        public MakeTemporaryItemPermanentCommandHandler(IItemRepository itemRepository,
            IItemCategoryRepository itemCategoryRepository, IManufacturerRepository manufacturerRepository)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task<bool> HandleAsync(MakeTemporaryItemPermanentCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var storeItem = await itemRepository.FindByAsync(command.PermanentItem.Id, cancellationToken);
            if (storeItem == null)
                throw new ItemNotFoundException(command.PermanentItem.Id);
            if (!storeItem.IsTemporary)
                throw new ItemIsNotTemporaryException(command.PermanentItem.Id);

            var itemCategory = await itemCategoryRepository
                .FindByAsync(command.PermanentItem.ItemCategoryId, cancellationToken);
            if (itemCategory == null)
                throw new ItemCategoryNotFoundException(command.PermanentItem.ItemCategoryId);

            var manufacturer = await manufacturerRepository
                .FindByAsync(command.PermanentItem.ManufacturerId, cancellationToken);
            if (manufacturer == null)
                throw new ManufacturerNotFoundException(command.PermanentItem.ManufacturerId);

            var permanentItem = command.PermanentItem.ToStoreItem(itemCategory, manufacturer);
            await itemRepository.StoreAsync(permanentItem, cancellationToken);

            return true;
        }
    }
}