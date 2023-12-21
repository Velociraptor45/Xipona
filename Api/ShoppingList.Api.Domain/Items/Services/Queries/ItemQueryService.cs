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

    public ItemQueryService(IItemRepository itemRepository,
        IItemReadModelConversionService itemReadModelConversionService)
    {
        _itemRepository = itemRepository;
        _itemReadModelConversionService = itemReadModelConversionService;
    }

    public async Task<ItemReadModel> GetAsync(ItemId itemId)
    {
        var item = await _itemRepository.FindActiveByAsync(itemId);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        return await _itemReadModelConversionService.ConvertAsync(item);
    }
}