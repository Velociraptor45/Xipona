using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Ports;
using Xipona.Api.Domain.Items.Reasons;

namespace Xipona.Api.Domain.Items.Services.Modifications;

public interface IFavoriteItemService
{
    Task MarkAsFavoriteAsync(ItemId itemId);
    Task UnmarkAsFavoriteAsync(ItemId itemId);
}

internal class FavoriteItemService(IItemRepository itemRepository) : IFavoriteItemService
{
    public async Task MarkAsFavoriteAsync(ItemId itemId)
    {
        var item = await itemRepository.FindActiveByAsync(itemId);
        if (item is null)
            throw new DomainException(new ItemNotFoundReason(itemId));
        
        item.MarkAsFavoriteAsync();
        await itemRepository.StoreAsync(item);
    }
    
    public async Task UnmarkAsFavoriteAsync(ItemId itemId)
    {
        var item = await itemRepository.FindActiveByAsync(itemId);
        if (item is null)
            throw new DomainException(new ItemNotFoundReason(itemId));
        
        item.UnmarkAsFavoriteAsync();
        await itemRepository.StoreAsync(item);
    }
}