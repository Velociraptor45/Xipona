using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemUpdate;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ItemUpdateWithTypes;

public class UpdateItemWithTypesCommandHandler : ICommandHandler<UpdateItemWithTypesCommand, bool>
{
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly Func<CancellationToken, IItemUpdateService> _itemUpdateServiceDelegat;

    public UpdateItemWithTypesCommandHandler(ITransactionGenerator transactionGenerator,
        Func<CancellationToken, IItemUpdateService> itemUpdateServiceDelegat)
    {
        _transactionGenerator = transactionGenerator;
        _itemUpdateServiceDelegat = itemUpdateServiceDelegat;
    }

    public async Task<bool> HandleAsync(UpdateItemWithTypesCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemUpdateServiceDelegat(cancellationToken);
        await service.UpdateItemWithTypesAsync(command.ItemWithTypesUpdate);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}