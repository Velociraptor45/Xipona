using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem
{
    public class ModifyItemCommandHandler : ICommandHandler<ModifyItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly ITransactionGenerator transactionGenerator;
        private readonly IItemCategoryValidationService itemCategoryValidationService;
        private readonly IManufacturerValidationService manufacturerValidationService;
        private readonly IAvailabilityValidationService availabilityValidationService;

        public ModifyItemCommandHandler(IItemRepository itemRepository, IShoppingListRepository shoppingListRepository,
            ITransactionGenerator transactionGenerator, IItemCategoryValidationService itemCategoryValidationService,
            IManufacturerValidationService manufacturerValidationService,
            IAvailabilityValidationService availabilityValidationService)
        {
            this.itemRepository = itemRepository;
            this.shoppingListRepository = shoppingListRepository;
            this.transactionGenerator = transactionGenerator;
            this.itemCategoryValidationService = itemCategoryValidationService;
            this.manufacturerValidationService = manufacturerValidationService;
            this.availabilityValidationService = availabilityValidationService;
        }

        public async Task<bool> HandleAsync(ModifyItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var storeItem = await itemRepository.FindByAsync(command.ItemModify.Id, cancellationToken);

            if (storeItem == null)
                throw new DomainException(new ItemNotFoundReason(command.ItemModify.Id));
            if (storeItem.IsTemporary)
                throw new DomainException(new TemporaryItemNotModifyableReason(command.ItemModify.Id));

            var itemCategoryId = command.ItemModify.ItemCategoryId;
            var manufacturerId = command.ItemModify.ManufacturerId;

            await itemCategoryValidationService.Validate(itemCategoryId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            if (manufacturerId != null)
            {
                await manufacturerValidationService.Validate(manufacturerId, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();

            var availabilities = command.ItemModify.Availabilities;
            await availabilityValidationService.Validate(availabilities, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            storeItem.Modify(command.ItemModify, availabilities);

            var availableAtStoreIds = storeItem.Availabilities.Select(av => av.StoreId);
            var shoppingListsWithItem = (await shoppingListRepository.FindByAsync(storeItem.Id, cancellationToken))
                .Where(list => !availableAtStoreIds.Any(storeId => storeId == list.StoreId))
                .ToList();

            using var transaction = await transactionGenerator.GenerateAsync(cancellationToken);

            await itemRepository.StoreAsync(storeItem, cancellationToken);
            foreach (var list in shoppingListsWithItem)
            {
                cancellationToken.ThrowIfCancellationRequested();
                // remove items from all shopping lists where item is not available anymore
                list.RemoveItem(storeItem.Id);
                await shoppingListRepository.StoreAsync(list, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);

            return true;
        }
    }
}