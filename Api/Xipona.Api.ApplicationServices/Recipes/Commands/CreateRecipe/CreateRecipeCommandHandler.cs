using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Repositories.Common.Transactions;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Commands.CreateRecipe;

public class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand, RecipeReadModel>
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

    public async Task<RecipeReadModel> HandleAsync(CreateRecipeCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        var service = _recipeCreationServiceDelegate(cancellationToken);
        var result = await service.CreateAsync(command.Creation);

        await transaction.CommitAsync(cancellationToken);

        return result;
    }
}