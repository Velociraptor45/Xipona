using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Deletions;

public class ItemDeletionService : IItemDeletionService
{
    private readonly IItemRepository _itemRepository;

    public ItemDeletionService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
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