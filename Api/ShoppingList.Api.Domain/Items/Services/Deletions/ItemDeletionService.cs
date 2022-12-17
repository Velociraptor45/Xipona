using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Deletions;

public class ItemDeletionService : IItemDeletionService
{
    private readonly IItemRepository _itemRepository;
    private readonly CancellationToken _cancellationToken;

    public ItemDeletionService(
        IItemRepository itemRepository,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepository;
        _cancellationToken = cancellationToken;
    }

    public async Task DeleteAsync(ItemId itemId)
    {
        var item = await _itemRepository.FindActiveByAsync(itemId, _cancellationToken);
        if (item == null)
            return;

        item.Delete();
        await _itemRepository.StoreAsync(item, _cancellationToken);
    }
}