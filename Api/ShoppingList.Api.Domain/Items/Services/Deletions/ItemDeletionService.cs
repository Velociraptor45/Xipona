using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Deletions;

public class ItemDeletionService : IItemDeletionService
{
    private readonly IItemRepository _itemRepository;

    public ItemDeletionService(
        Func<CancellationToken, IItemRepository> itemRepositoryDelegate,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepositoryDelegate(cancellationToken);
    }

    public async Task DeleteAsync(ItemId itemId)
    {
        var item = await _itemRepository.FindActiveByAsync(itemId);
        if (item == null)
            return;

        item.Delete();
        await _itemRepository.StoreAsync(item);
    }
}