using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Deletions;

public class ManufacturerDeletionService : IManufacturerDeletionService
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IItemRepository _itemRepository;

    public ManufacturerDeletionService(
        Func<CancellationToken, IManufacturerRepository> manufacturerRepositoryDelegate,
        Func<CancellationToken, IItemRepository> itemRepositoryDelegate,
        CancellationToken cancellationToken)
    {
        _manufacturerRepository = manufacturerRepositoryDelegate(cancellationToken);
        _itemRepository = itemRepositoryDelegate(cancellationToken);
    }

    public async Task DeleteAsync(ManufacturerId manufacturerId)
    {
        var manufacturer = await _manufacturerRepository.FindActiveByAsync(manufacturerId);
        if (manufacturer == null)
            return;

        var items = await _itemRepository.FindByAsync(manufacturerId);

        foreach (var item in items)
        {
            item.RemoveManufacturer();
            await _itemRepository.StoreAsync(item);
        }

        manufacturer.Delete();

        await _manufacturerRepository.StoreAsync(manufacturer);
    }
}