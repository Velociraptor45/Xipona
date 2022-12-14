using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Commands.CreateRecipe;

public class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand, IRecipe>
{
    private readonly Func<CancellationToken, IRecipeCreationService> _recipeCreationServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public CreateRecipeCommandHandler(
        Func<CancellationToken, IRecipeCreationService> recipeCreationServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _recipeCreationServiceDelegate = recipeCreationServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<IRecipe> HandleAsync(CreateRecipeCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _recipeCreationServiceDelegate(cancellationToken);
        var result = await service.CreateAsync(command.Creation);

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}