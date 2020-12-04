using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.UpdateItem
{
    public class UpdateItemCommand : ICommand<bool>
    {
        public UpdateItemCommand(ItemUpdate itemUpdate)
        {
            ItemUpdate = itemUpdate ?? throw new ArgumentNullException(nameof(itemUpdate));
        }

        public ItemUpdate ItemUpdate { get; }
    }
}