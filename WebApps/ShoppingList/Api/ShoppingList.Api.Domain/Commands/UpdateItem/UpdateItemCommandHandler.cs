using ShoppingList.Api.Domain.Converters;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.UpdateItem
{
    public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IShoppingListRepository shoppingListRepository;

        public UpdateItemCommandHandler(IItemRepository itemRepository, IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository, IShoppingListRepository shoppingListRepository)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<bool> HandleAsync(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new System.ArgumentNullException(nameof(command));
            }

            // deactivate old item
            StoreItem oldItem = await itemRepository.FindByAsync(command.ItemUpdate.OldId, cancellationToken);
            oldItem.Delete();
            await itemRepository.StoreAsync(oldItem, cancellationToken);

            // create new Item
            ItemCategory itemCategory = await itemCategoryRepository
                .FindByAsync(command.ItemUpdate.ItemCategoryId, cancellationToken);
            Manufacturer manufacturer = await manufacturerRepository
                .FindByAsync(command.ItemUpdate.ManufacturerId, cancellationToken);

            StoreItem updatedItem = command.ItemUpdate.ToStoreItem(itemCategory, manufacturer);
            updatedItem = await itemRepository.StoreAsync(updatedItem, cancellationToken);

            // change existing item references on shopping lists
            var shoppingListsWithOldItem = (await shoppingListRepository
                .FindActiveByAsync(command.ItemUpdate.OldId, cancellationToken))
                .ToList();

            foreach (var list in shoppingListsWithOldItem)
            {
                ShoppingListItem shoppingListItem = list.Items
                    .First(i => i.Id == command.ItemUpdate.OldId.ToShoppingListItemId());
                list.RemoveItem(command.ItemUpdate.OldId.ToShoppingListItemId());
                if (updatedItem.IsAvailableInStore(list.Store.Id))
                {
                    list.AddItem(updatedItem, shoppingListItem.IsInBasket, shoppingListItem.Quantity);
                }

                await shoppingListRepository.StoreAsync(list, cancellationToken);
            }

            return true;
        }
    }
}