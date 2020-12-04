﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem
{
    public class ChangeItemCommandHandler : ICommandHandler<ChangeItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        public ChangeItemCommandHandler(IItemRepository itemRepository, IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task<bool> HandleAsync(ChangeItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var storeItem = await itemRepository.FindByAsync(command.ItemChange.Id, cancellationToken);

            if (storeItem == null)
                throw new ItemNotFoundException(command.ItemChange.Id);
            if (storeItem.IsTemporary)
                throw new TemporaryItemNotModifyableException(command.ItemChange.Id);

            cancellationToken.ThrowIfCancellationRequested();

            ItemCategory itemCategory = await itemCategoryRepository
                .FindByAsync(command.ItemChange.ItemCategoryId, cancellationToken);

            Manufacturer manufacturer = null;
            if (command.ItemChange.ManufacturerId != null)
                manufacturer = await manufacturerRepository
                .FindByAsync(command.ItemChange.ManufacturerId, cancellationToken);

            storeItem.Modify(command.ItemChange, itemCategory, manufacturer);

            await itemRepository.StoreAsync(storeItem, cancellationToken);

            return true;
        }
    }
}