using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Users.Services.Update;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.Users.Commands.UpdateGeneralSettings;

public class UpdateGeneralSettingsCommandHandler : ICommandHandler<UpdateGeneralSettingsCommand, bool>
{
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly Func<CancellationToken, IGeneralSettingsUpdateService> _updateServiceDelegate;

    public UpdateGeneralSettingsCommandHandler(ITransactionGenerator transactionGenerator,
        Func<CancellationToken, IGeneralSettingsUpdateService> updateServiceDelegate)
    {
        _transactionGenerator = transactionGenerator;
        _updateServiceDelegate = updateServiceDelegate;
    }

    public async Task<bool> HandleAsync(UpdateGeneralSettingsCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var updateService = _updateServiceDelegate(cancellationToken);
        await updateService.UpdateAsync(command.Currency);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}
