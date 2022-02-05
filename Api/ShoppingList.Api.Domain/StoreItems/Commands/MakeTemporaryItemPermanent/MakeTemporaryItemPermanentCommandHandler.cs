using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;

public class MakeTemporaryItemPermanentCommandHandler : ICommandHandler<MakeTemporaryItemPermanentCommand, bool>
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemCategoryValidationService _itemCategoryValidationService;
    private readonly IManufacturerValidationService _manufacturerValidationService;
    private readonly IAvailabilityValidationService _availabilityValidationService;

    public MakeTemporaryItemPermanentCommandHandler(IItemRepository itemRepository,
        IItemCategoryValidationService itemCategoryValidationService,
        IManufacturerValidationService manufacturerValidationService,
        IAvailabilityValidationService availabilityValidationService)
    {
        _itemRepository = itemRepository;
        _itemCategoryValidationService = itemCategoryValidationService;
        _manufacturerValidationService = manufacturerValidationService;
        _availabilityValidationService = availabilityValidationService;
    }

    public async Task<bool> HandleAsync(MakeTemporaryItemPermanentCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        IStoreItem? storeItem = await _itemRepository.FindByAsync(command.PermanentItem.Id, cancellationToken);
        if (storeItem == null)
            throw new DomainException(new ItemNotFoundReason(command.PermanentItem.Id));
        if (!storeItem.IsTemporary)
            throw new DomainException(new ItemNotTemporaryReason(command.PermanentItem.Id));

        var itemCategoryId = command.PermanentItem.ItemCategoryId;
        var manufacturerId = command.PermanentItem.ManufacturerId;

        await _itemCategoryValidationService.ValidateAsync(itemCategoryId, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (manufacturerId != null)
        {
            await _manufacturerValidationService.ValidateAsync(manufacturerId.Value, cancellationToken);
        }

        var availabilities = command.PermanentItem.Availabilities;
        await _availabilityValidationService.ValidateAsync(availabilities, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        storeItem.MakePermanent(command.PermanentItem, availabilities);
        await _itemRepository.StoreAsync(storeItem, cancellationToken);

        return true;
    }
}