using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Items.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Conversion.ItemReadModels;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

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