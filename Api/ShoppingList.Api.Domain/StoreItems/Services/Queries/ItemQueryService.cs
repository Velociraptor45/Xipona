using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

public class ItemQueryService : IItemQueryService
{
    private readonly IItemRepository _itemRepository;
    private readonly IStoreItemReadModelConversionService _storeItemReadModelConversionService;
    private readonly CancellationToken _cancellationToken;

    public ItemQueryService(
        IItemRepository itemRepository,
        IStoreItemReadModelConversionService storeItemReadModelConversionService,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepository;
        _storeItemReadModelConversionService = storeItemReadModelConversionService;
        _cancellationToken = cancellationToken;
    }

    public async Task<StoreItemReadModel> GetAsync(ItemId itemId)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        var item = await _itemRepository.FindByAsync(itemId, _cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        _cancellationToken.ThrowIfCancellationRequested();

        return await _storeItemReadModelConversionService.ConvertAsync(item, _cancellationToken);
    }
}