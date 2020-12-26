using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemCommandHandler : ICommandHandler<CreateTemporaryItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IStoreRepository storeRepository;
        private readonly IStoreItemFactory storeItemFactory;

        public CreateTemporaryItemCommandHandler(IItemRepository itemRepository, IStoreRepository storeRepository,
            IStoreItemFactory storeItemFactory)
        {
            this.itemRepository = itemRepository;
            this.storeRepository = storeRepository;
            this.storeItemFactory = storeItemFactory;
        }

        public async Task<bool> HandleAsync(CreateTemporaryItemCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var store = await storeRepository.FindByAsync(command.TemporaryItemCreation.Availability.StoreId,
                cancellationToken);
            if (store == null || store.IsDeleted)
                throw new DomainException(new StoreNotFoundReason(command.TemporaryItemCreation.Availability.StoreId));

            var storeItem = storeItemFactory.Create(command.TemporaryItemCreation);

            await itemRepository.StoreAsync(storeItem, cancellationToken);
            return true;
        }
    }
}