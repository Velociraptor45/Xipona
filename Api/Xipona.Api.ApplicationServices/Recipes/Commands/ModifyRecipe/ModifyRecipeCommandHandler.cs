﻿using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Commands.ModifyRecipe;

public class ModifyRecipeCommandHandler : ICommandHandler<ModifyRecipeCommand, bool>
{
    private readonly Func<CancellationToken, IRecipeModificationService> _recipeModificationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public ModifyRecipeCommandHandler(
        Func<CancellationToken, IRecipeModificationService> recipeModificationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _recipeModificationServiceDelegate = recipeModificationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(ModifyRecipeCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _recipeModificationServiceDelegate(cancellationToken);
        await service.ModifyAsync(command.Modification);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}