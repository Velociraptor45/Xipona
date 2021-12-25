using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemUpdate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ItemUpdateWithTypes
{
    public class UpdateItemWithTypesCommandHandler : ICommandHandler<UpdateItemWithTypesCommand, bool>
    {
        private readonly ITransactionGenerator _transactionGenerator;
        private readonly IItemUpdateService _itemUpdateService;

        public UpdateItemWithTypesCommandHandler(ITransactionGenerator transactionGenerator,
            IItemUpdateService itemUpdateService)
        {
            _transactionGenerator = transactionGenerator;
            _itemUpdateService = itemUpdateService;
        }

        public async Task<bool> HandleAsync(UpdateItemWithTypesCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

            await _itemUpdateService.UpdateItemWithTypesAsync(command.ItemWithTypesUpdate);

            await transaction.CommitAsync(cancellationToken);

            return true;
        }
    }
}