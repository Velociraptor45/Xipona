using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Users.Models;
using Xipona.Api.Domain.Users.Services.Creations;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.Users.Commands.Login;

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
        var result = await service.CreateAsync(command.UserId);

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}
