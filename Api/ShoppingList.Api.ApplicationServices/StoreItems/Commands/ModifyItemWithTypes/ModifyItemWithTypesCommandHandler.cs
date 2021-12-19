﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModification;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ModifyItemWithTypes
{
    public class ModifyItemWithTypesCommandHandler : ICommandHandler<ModifyItemWithTypesCommand, bool>
    {
        private readonly ITransactionGenerator _transactionGenerator;
        private readonly Func<CancellationToken, IItemModificationService> _itemModificationServiceDelegate;

        public ModifyItemWithTypesCommandHandler(ITransactionGenerator transactionGenerator,
            Func<CancellationToken, IItemModificationService> itemModificationServiceDelegate)
        {
            _transactionGenerator = transactionGenerator;
            _itemModificationServiceDelegate = itemModificationServiceDelegate;
        }

        public async Task<bool> HandleAsync(ModifyItemWithTypesCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            var service = _itemModificationServiceDelegate(cancellationToken);

            using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);
            await service.ModifyItemWithTypesAsync(command.ItemWithTypesModification);
            await transaction.CommitAsync(cancellationToken);

            return true;
        }
    }
}