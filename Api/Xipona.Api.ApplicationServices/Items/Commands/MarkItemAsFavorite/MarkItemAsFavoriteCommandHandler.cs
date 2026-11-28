using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Modifications;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.Items.Commands.MarkItemAsFavorite;

public record MarkItemAsFavoriteCommand(ItemId ItemId) : ICommand<bool>;
    
public class MarkItemAsFavoriteCommandHandler : ICommandHandler<MarkItemAsFavoriteCommand, bool>
{
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly Func<CancellationToken, IFavoriteItemService> _favoriteItemServiceDelegate;

    public MarkItemAsFavoriteCommandHandler(ITransactionGenerator transactionGenerator,
        Func<CancellationToken, IFavoriteItemService> favoriteItemServiceDelegate)
    {
        _transactionGenerator = transactionGenerator;
        _favoriteItemServiceDelegate = favoriteItemServiceDelegate;
    }
    
    public async Task<bool> HandleAsync(MarkItemAsFavoriteCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);
        
        var service = _favoriteItemServiceDelegate(cancellationToken);
        await service.MarkAsFavoriteAsync(command.ItemId);
        
        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}