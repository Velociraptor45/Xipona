using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemById;

public class ItemByIdQueryHandler : IQueryHandler<ItemByIdQuery, StoreItemReadModel>
{
    private readonly IItemRepository _itemRepository;
    private readonly IStoreItemReadModelConversionService _storeItemReadModelConversionService;

    public ItemByIdQueryHandler(IItemRepository itemRepository,
        IStoreItemReadModelConversionService storeItemReadModelConversionService)
    {
        _itemRepository = itemRepository;
        _storeItemReadModelConversionService = storeItemReadModelConversionService;
    }

    public async Task<StoreItemReadModel> HandleAsync(ItemByIdQuery query, CancellationToken cancellationToken)
    {
        if (query is null)
            throw new ArgumentNullException(nameof(query));

        cancellationToken.ThrowIfCancellationRequested();

        var item = await _itemRepository.FindByAsync(query.ItemId, cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(query.ItemId));

        cancellationToken.ThrowIfCancellationRequested();

        return await _storeItemReadModelConversionService.ConvertAsync(item, cancellationToken);
    }
}