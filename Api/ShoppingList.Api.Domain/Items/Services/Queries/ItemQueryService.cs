using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion.ItemReadModels;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

public class ItemQueryService : IItemQueryService
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemReadModelConversionService _itemReadModelConversionService;
    private readonly CancellationToken _cancellationToken;

    public ItemQueryService(
        IItemRepository itemRepository,
        IItemReadModelConversionService itemReadModelConversionService,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepository;
        _itemReadModelConversionService = itemReadModelConversionService;
        _cancellationToken = cancellationToken;
    }

    public async Task<ItemReadModel> GetAsync(ItemId itemId)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        var item = await _itemRepository.FindActiveByAsync(itemId, _cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        _cancellationToken.ThrowIfCancellationRequested();

        return await _itemReadModelConversionService.ConvertAsync(item, _cancellationToken);
    }
}