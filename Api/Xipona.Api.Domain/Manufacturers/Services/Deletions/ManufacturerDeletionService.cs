using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Ports;

namespace ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Deletions;

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