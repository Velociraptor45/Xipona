using Xipona.Api.Domain.Items.Ports;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Manufacturers.Ports;

namespace Xipona.Api.Domain.Manufacturers.Services.Deletions;

public class ManufacturerDeletionService : IManufacturerDeletionService
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IItemRepository _itemRepository;

    public ManufacturerDeletionService(IManufacturerRepository manufacturerRepository, IItemRepository itemRepository)
    {
        _manufacturerRepository = manufacturerRepository;
        _itemRepository = itemRepository;
    }

    public async Task DeleteAsync(ManufacturerId manufacturerId)
    {
        var manufacturer = await _manufacturerRepository.FindActiveByAsync(manufacturerId);
        if (manufacturer == null)
            return;

        var items = await _itemRepository.FindActiveByAsync(manufacturerId);

        foreach (var item in items)
        {
            item.RemoveManufacturer();
            await _itemRepository.StoreAsync(item);
        }

        manufacturer.Delete();

        await _manufacturerRepository.StoreAsync(manufacturer);
    }
}