using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Accounts.Models;
using ProjectHermes.Xipona.Api.Domain.Accounts.Services.Creations;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Accounts.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, IUser>
{
    private readonly Func<CancellationToken, IUserCreationService> _userCreationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public LoginCommandHandler(
        Func<CancellationToken, IUserCreationService> userCreationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _userCreationServiceDelegate = userCreationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<IUser> HandleAsync(LoginCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _userCreationServiceDelegate(cancellationToken);
        var result = service.CreateAsync(command.UserId);

        await transaction.CommitAsync(cancellationToken);

        return await result;
    }
}
