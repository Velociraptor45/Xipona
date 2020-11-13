using System;

namespace ShoppingList.Api.Domain.Commands.UpdateItem
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