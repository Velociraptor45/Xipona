using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.TemporaryItems;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.MakeTemporaryItemPermanent;

public class MakeTemporaryItemPermanentCommandHandler : ICommandHandler<MakeTemporaryItemPermanentCommand, bool>
{
    private readonly Func<CancellationToken, ITemporaryItemService> _temporaryItemService;
    private readonly ITransactionGenerator _transactionGenerator;

    public MakeTemporaryItemPermanentCommandHandler(
        Func<CancellationToken, ITemporaryItemService> temporaryItemService,
        ITransactionGenerator transactionGenerator)
    {
        _temporaryItemService = temporaryItemService;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(MakeTemporaryItemPermanentCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _temporaryItemService(cancellationToken);
        await service.MakePermanentAsync(command.PermanentItem);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}