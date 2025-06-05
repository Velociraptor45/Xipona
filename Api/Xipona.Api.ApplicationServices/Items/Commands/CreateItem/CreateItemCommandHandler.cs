using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Services.Creations;
using Xipona.Api.Domain.Items.Services.Queries;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.Items.Commands.CreateItem;

public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, ItemReadModel>
{
    private readonly Func<CancellationToken, IItemCreationService> _itemCreationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public CreateItemCommandHandler(Func<CancellationToken, IItemCreationService> itemCreationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _itemCreationServiceDelegate = itemCreationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<ItemReadModel> HandleAsync(CreateItemCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var itemCreationService = _itemCreationServiceDelegate(cancellationToken);
        var result = await itemCreationService.CreateAsync(command.ItemCreation);

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}