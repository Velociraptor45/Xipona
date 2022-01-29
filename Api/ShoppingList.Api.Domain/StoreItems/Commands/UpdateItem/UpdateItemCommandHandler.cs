using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.UpdateItem
{
    public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly ITransactionGenerator transactionGenerator;
        private readonly IStoreItemFactory storeItemFactory;
        private readonly IItemCategoryValidationService itemCategoryValidationService;
        private readonly IManufacturerValidationService manufacturerValidationService;
        private readonly IAvailabilityValidationService availabilityValidationService;
        private readonly IShoppingListUpdateService shoppingListUpdateService;

        public UpdateItemCommandHandler(IItemRepository itemRepository,
            ITransactionGenerator transactionGenerator, IStoreItemFactory storeItemFactory,
            IItemCategoryValidationService itemCategoryValidationService,
            IManufacturerValidationService manufacturerValidationService,
            IAvailabilityValidationService availabilityValidationService,
            IShoppingListUpdateService shoppingListUpdateService)
        {
            this.itemRepository = itemRepository;
            this.transactionGenerator = transactionGenerator;
            this.storeItemFactory = storeItemFactory;
            this.itemCategoryValidationService = itemCategoryValidationService;
            this.manufacturerValidationService = manufacturerValidationService;
            this.availabilityValidationService = availabilityValidationService;
            this.shoppingListUpdateService = shoppingListUpdateService;
        }

        public async Task<bool> HandleAsync(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new System.ArgumentNullException(nameof(command));
            }

            IStoreItem? oldItem = await itemRepository.FindByAsync(command.ItemUpdate.OldId, cancellationToken);
            if (oldItem == null)
                throw new DomainException(new ItemNotFoundReason(command.ItemUpdate.OldId));
            if (oldItem.IsTemporary)
                throw new DomainException(new TemporaryItemNotUpdateableReason(command.ItemUpdate.OldId));

            oldItem.Delete();

            var itemCategoryId = command.ItemUpdate.ItemCategoryId;
            var manufacturerId = command.ItemUpdate.ManufacturerId;

            await itemCategoryValidationService.ValidateAsync(itemCategoryId, cancellationToken);

            if (manufacturerId != null)
            {
                await manufacturerValidationService.ValidateAsync(manufacturerId.Value, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();

            var availabilities = command.ItemUpdate.Availabilities;
            await availabilityValidationService.ValidateAsync(availabilities, cancellationToken);

            using ITransaction transaction = await transactionGenerator.GenerateAsync(cancellationToken);
            await itemRepository.StoreAsync(oldItem, cancellationToken);

            // create new Item
            IStoreItem updatedItem = storeItemFactory.Create(command.ItemUpdate, oldItem);
            updatedItem = await itemRepository.StoreAsync(updatedItem, cancellationToken);

            // change existing item references on shopping lists
            await shoppingListUpdateService.ExchangeItemAsync(oldItem.Id, updatedItem, cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return true;
        }
    }
}