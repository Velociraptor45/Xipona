using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Modifications;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.Items.Commands.UnmarkItemAsFavorite;

public record UnmarkItemAsFavoriteCommand(ItemId ItemId) : ICommand<bool>;
    
public class UnmarkItemAsFavoriteCommandHandler : ICommandHandler<UnmarkItemAsFavoriteCommand, bool>
{
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly Func<CancellationToken, IFavoriteItemService> _favoriteItemServiceDelegate;

    public UnmarkItemAsFavoriteCommandHandler(ITransactionGenerator transactionGenerator,
        Func<CancellationToken, IFavoriteItemService> favoriteItemServiceDelegate)
    {
        _transactionGenerator = transactionGenerator;
        _favoriteItemServiceDelegate = favoriteItemServiceDelegate;
    }
    
    public async Task<bool> HandleAsync(UnmarkItemAsFavoriteCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);
        
        var service = _favoriteItemServiceDelegate(cancellationToken);
        await service.UnmarkAsFavoriteAsync(command.ItemId);
        
        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}