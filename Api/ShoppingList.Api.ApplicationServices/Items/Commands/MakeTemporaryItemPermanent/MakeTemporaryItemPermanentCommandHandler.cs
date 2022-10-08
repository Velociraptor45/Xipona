using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Infrastructure.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.MakeTemporaryItemPermanent;

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
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _temporaryItemService(cancellationToken);
        await service.MakePermanentAsync(command.PermanentItem);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}