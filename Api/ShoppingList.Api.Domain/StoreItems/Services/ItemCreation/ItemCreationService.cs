using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemCreation
{
    public class ItemCreationService : IItemCreationService
    {
        private readonly IItemRepository _itemRepository;
        private readonly CancellationToken _cancellationToken;

        public ItemCreationService(IItemRepository itemRepository, CancellationToken cancellationToken)
        {
            _itemRepository = itemRepository;
            _cancellationToken = cancellationToken;
        }

        public async Task CreateItemWithTypesAsync(IStoreItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            await _itemRepository.StoreAsync(item, _cancellationToken);
        }
    }
}