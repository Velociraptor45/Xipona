using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.ItemCategories.Services.Creations;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;

public class CreateItemCategoryCommandHandler : ICommandHandler<CreateItemCategoryCommand, IItemCategory>
{
    private readonly Func<CancellationToken, IItemCategoryCreationService> _itemCategoryCreationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public CreateItemCategoryCommandHandler(
        Func<CancellationToken, IItemCategoryCreationService> itemCategoryCreationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _itemCategoryCreationServiceDelegate = itemCategoryCreationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<IItemCategory> HandleAsync(CreateItemCategoryCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _itemCategoryCreationServiceDelegate(cancellationToken);
        var result = await service.CreateAsync(command.Name);

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}