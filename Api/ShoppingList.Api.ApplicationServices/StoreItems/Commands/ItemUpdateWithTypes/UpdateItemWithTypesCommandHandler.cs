using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ItemUpdateWithTypes;

public class UpdateItemWithTypesCommandHandler : ICommandHandler<UpdateItemWithTypesCommand, bool>
{
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly Func<CancellationToken, IItemUpdateService> _itemExchangeServiceDelegat;

    public UpdateItemWithTypesCommandHandler(ITransactionGenerator transactionGenerator,
        Func<CancellationToken, IItemUpdateService> itemExchangeServiceDelegate)
    {
        _transactionGenerator = transactionGenerator;
        _itemExchangeServiceDelegat = itemExchangeServiceDelegate;
    }

    public async Task<bool> HandleAsync(UpdateItemWithTypesCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
            throw new ArgumentNullException(nameof(command));

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemExchangeServiceDelegat(cancellationToken);
        await service.UpdateItemWithTypesAsync(command.ItemWithTypesUpdate);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}