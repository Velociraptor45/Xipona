using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem
{
    public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IStoreItemFactory storeItemFactory;

        public CreateItemCommandHandler(IItemRepository itemRepository, IManufacturerRepository manufacturerRepository,
            IItemCategoryRepository itemCategoryRepository, IStoreItemFactory storeItemFactory)
        {
            this.itemRepository = itemRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.storeItemFactory = storeItemFactory;
        }

        public async Task<bool> HandleAsync(CreateItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            IItemCategory itemCategory = await itemCategoryRepository
                .FindByAsync(command.ItemCreation.ItemCategoryId, cancellationToken);

            IManufacturer manufacturer = null;
            if (command.ItemCreation.ManufacturerId != null)
                manufacturer = await manufacturerRepository
                    .FindByAsync(command.ItemCreation.ManufacturerId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var storeItem = storeItemFactory.Create(command.ItemCreation, itemCategory, manufacturer);

            await itemRepository.StoreAsync(storeItem, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return true;
        }
    }
}