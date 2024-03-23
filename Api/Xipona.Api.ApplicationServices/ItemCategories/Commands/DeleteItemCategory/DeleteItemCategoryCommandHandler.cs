using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Deletions;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;

public class DeleteItemCategoryCommandHandler : ICommandHandler<DeleteItemCategoryCommand, bool>
{
    private readonly Func<CancellationToken, IItemCategoryDeletionService> _itemCategoryDeletionServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public DeleteItemCategoryCommandHandler(
        Func<CancellationToken, IItemCategoryDeletionService> itemCategoryDeletionServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _itemCategoryDeletionServiceDelegate = itemCategoryDeletionServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(DeleteItemCategoryCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemCategoryDeletionServiceDelegate(cancellationToken);
        await service.DeleteAsync(command.ItemCategoryId);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }
}