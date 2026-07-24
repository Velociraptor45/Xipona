using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Ports;
using Xipona.Api.Domain.Items.Services.Conversion;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Manufacturers.Ports;
using Xipona.Api.Domain.Shared.Validations;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Items.Services.Searches;

public interface IItemSearchService
{
    Task<IEnumerable<SearchItemForShoppingResultReadModel>> SearchForShoppingListAsync(string name, StoreId storeId);

    Task<IEnumerable<SearchItemResultReadModel>> SearchAsync(string searchInput, int page, int pageSize);

    Task<IEnumerable<SearchItemResultReadModel>> SearchAsync(StoreId? storeId,
        ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId, int page, int pageSize);

    Task<IEnumerable<SearchItemByItemCategoryResult>> SearchAsync(ItemCategoryId itemCategoryId);
    Task<int> GetTotalSearchResultCountAsync(string searchInput);
}

public class ItemSearchService : IItemSearchService
{
    private const int _maxSearchResults = 20;

    private readonly IItemRepository _itemRepository;
    private readonly IItemReadRepository _itemReadRepository;
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IValidator _validator;
    private readonly IItemAvailabilityReadModelConversionService _availabilityConverter;

    public ItemSearchService(
        IItemRepository itemRepository,
        IItemReadRepository itemReadRepository,
        IManufacturerRepository manufacturerRepository,
        IValidator validator,
        IItemAvailabilityReadModelConversionService availabilityConverter)
    {
        _itemRepository = itemRepository;
        _itemReadRepository = itemReadRepository;
        _manufacturerRepository = manufacturerRepository;
        _validator = validator;
        _availabilityConverter = availabilityConverter;
    }

    public async Task<IEnumerable<SearchItemResultReadModel>> SearchAsync(StoreId? storeId,
        ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId, int page, int pageSize)
    {
        var items = (await _itemRepository.FindPermanentByAsync(storeId, itemCategoryId,
            manufacturerId, page, pageSize)).ToList();
        
        var manufacturerIds = items.Where(i => i.ManufacturerId is not null).Select(i => i.ManufacturerId!.Value);
        var manufacturers = (await _manufacturerRepository.FindByAsync(manufacturerIds)).ToDictionary(m => m.Id);

        return items
            .Select(i =>
            {
                var manufacturerName = i.ManufacturerId is null ? null : manufacturers[i.ManufacturerId!.Value].Name;
                return new SearchItemResultReadModel(i.Id, i.Name, manufacturerName);
            });
    }

    public async Task<IEnumerable<SearchItemResultReadModel>> SearchAsync(string searchInput, int page, int pageSize)
    {
        if (string.IsNullOrWhiteSpace(searchInput))
            return [];

        var items = (await _itemRepository.FindActiveByAsync(searchInput, page, pageSize)).ToList();

        var manufacturerIds = items.Where(i => i.ManufacturerId is not null).Select(i => i.ManufacturerId!.Value);
        var manufacturers = (await _manufacturerRepository.FindByAsync(manufacturerIds)).ToDictionary(m => m.Id);

        var searchResults = items
            .Select(i =>
            {
                var manufacturerName = i.ManufacturerId is null ? null : manufacturers[i.ManufacturerId!.Value].Name;
                return new SearchItemResultReadModel(i.Id, i.Name, manufacturerName);
            });

        return searchResults;
    }

    public async Task<IEnumerable<SearchItemByItemCategoryResult>> SearchAsync(ItemCategoryId itemCategoryId)
    {
        await _validator.ValidateAsync(itemCategoryId);

        var items = (await _itemRepository.FindActiveByAsync(itemCategoryId))
            .ToList();
        var itemsLookup = items.ToLookup(i => i.HasItemTypes);

        var manufacturerIds = items.Where(i => i.ManufacturerId is not null).Select(i => i.ManufacturerId!.Value);
        var manufacturers = (await _manufacturerRepository.FindByAsync(manufacturerIds)).ToDictionary(m => m.Id);

        var availabilitiesDict = await _availabilityConverter.ConvertAsync(items);

        var results = new List<SearchItemByItemCategoryResult>();
        foreach (var item in itemsLookup[true])
        {
            foreach (var type in item.ItemTypes)
            {
                if (type.IsDeleted)
                    continue;

                results.Add(new SearchItemByItemCategoryResult(
                    item.Id,
                    type.Id,
                    $"{item.Name} {type.Name}",
                    item.ManufacturerId is null ? null : manufacturers[item.ManufacturerId!.Value].Name,
                    availabilitiesDict[(item.Id, type.Id)].ToList()));
            }
        }

        foreach (var item in itemsLookup[false])
        {
            results.Add(new SearchItemByItemCategoryResult(
                item.Id,
                null,
                item.Name,
                item.ManufacturerId is null ? null : manufacturers[item.ManufacturerId!.Value].Name,
                availabilitiesDict[(item.Id, null)].ToList()));
        }

        return results;
    }

    public async Task<int> GetTotalSearchResultCountAsync(string searchInput)
    {
        if (string.IsNullOrWhiteSpace(searchInput))
            return 0;

        return await _itemRepository.GetTotalCountByAsync(searchInput);
    }

    public async Task<IEnumerable<SearchItemForShoppingResultReadModel>> SearchForShoppingListAsync(string name, StoreId storeId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return [];

        var nameTrimmed = name.Trim();

        return await _itemReadRepository.GetShoppingListResultsAsync(nameTrimmed, storeId, _maxSearchResults);
    }
}