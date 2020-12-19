using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent
{
    public class MakeTemporaryItemPermanentCommandHandler : ICommandHandler<MakeTemporaryItemPermanentCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IStoreRepository storeRepository;

        public MakeTemporaryItemPermanentCommandHandler(IItemRepository itemRepository,
            IItemCategoryRepository itemCategoryRepository, IManufacturerRepository manufacturerRepository,
            IStoreRepository storeRepository)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.storeRepository = storeRepository;
        }

        public async Task<bool> HandleAsync(MakeTemporaryItemPermanentCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            IStoreItem storeItem = await itemRepository.FindByAsync(command.PermanentItem.Id, cancellationToken);
            if (storeItem == null)
                throw new ItemNotFoundException(command.PermanentItem.Id);
            if (!storeItem.IsTemporary)
                throw new ItemIsNotTemporaryException(command.PermanentItem.Id);

            var itemCategory = await itemCategoryRepository
                .FindByAsync(command.PermanentItem.ItemCategoryId, cancellationToken);
            if (itemCategory == null)
                throw new ItemCategoryNotFoundException(command.PermanentItem.ItemCategoryId);

            cancellationToken.ThrowIfCancellationRequested();

            IManufacturer manufacturer = null;
            if (command.PermanentItem.ManufacturerId != null)
            {
                manufacturer = await manufacturerRepository
                    .FindByAsync(command.PermanentItem.ManufacturerId, cancellationToken);
                if (manufacturer == null)
                    throw new ManufacturerNotFoundException(command.PermanentItem.ManufacturerId);
            }

            IEnumerable<IStore> activeStores = await storeRepository.FindActiveStoresAsync(cancellationToken);
            foreach (var availability in command.PermanentItem.Availabilities)
            {
                if (!activeStores.Any(s => s.Id == availability.StoreId))
                    throw new StoreNotFoundException(availability.StoreId);
            }

            cancellationToken.ThrowIfCancellationRequested();

            storeItem.MakePermanent(command.PermanentItem, itemCategory, manufacturer);

            await itemRepository.StoreAsync(storeItem, cancellationToken);

            return true;
        }
    }
}