using ShoppingList.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Commands.RemoveItemFromShoppingList
{
    public class RemoveItemFromShoppingListCommandHandler :
        ICommandHandler<RemoveItemFromShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;

        public RemoveItemFromShoppingListCommandHandler(IShoppingListRepository shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<bool> HandleAsync(RemoveItemFromShoppingListCommand query, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var list = await shoppingListRepository.FindByAsync(query.ShoppingListId);

            list.RemoveItem(query.ShoppingListItemId);

            await shoppingListRepository.StoreAsync(list);
            return true;
        }
    }
}