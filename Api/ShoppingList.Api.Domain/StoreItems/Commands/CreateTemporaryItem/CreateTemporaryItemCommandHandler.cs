using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System;
using System.Linq;
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

            var availability = command.TemporaryItemCreation.Availability;

            var store = await storeRepository.FindByAsync(availability.StoreId, cancellationToken);
            if (store == null || store.IsDeleted)
                throw new DomainException(new StoreNotFoundReason(availability.StoreId));

            if (!store.Sections.Any(s => s.Id == availability.DefaultSectionId))
            {
                throw new DomainException(
                    new SectionInStoreNotFoundReason(availability.DefaultSectionId, availability.StoreId));
            }

            var storeItem = storeItemFactory.Create(command.TemporaryItemCreation);

            await itemRepository.StoreAsync(storeItem, cancellationToken);
            return true;
        }
    }
}