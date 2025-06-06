using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Manufacturers.Services.Creations;
using Xipona.Api.Repositories.Common.Transactions;

namespace Xipona.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommandHandler : ICommandHandler<CreateManufacturerCommand, IManufacturer>
{
    private readonly Func<CancellationToken, IManufacturerCreationService> _manufacturerCreationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public CreateManufacturerCommandHandler(
        Func<CancellationToken, IManufacturerCreationService> manufacturerCreationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _manufacturerCreationServiceDelegate = manufacturerCreationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<IManufacturer> HandleAsync(CreateManufacturerCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _manufacturerCreationServiceDelegate(cancellationToken);
        var result = await service.CreateAsync(command.Name);

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}