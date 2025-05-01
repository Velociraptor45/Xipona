using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Users.Services.Update;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Users.Commands.UpdateGeneralSettings;

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
