using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Creation;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;

public class CreateRecipeTagCommandHandler : ICommandHandler<CreateRecipeTagCommand, IRecipeTag>
{
    private readonly Func<CancellationToken, IRecipeTagCreationService> _recipeTagCreateServiceDelegate;
    private readonly ITransactionGenerator _transactionGenerator;

    public CreateRecipeTagCommandHandler(
        Func<CancellationToken, IRecipeTagCreationService> recipeTagCreateServiceDelegate,
        ITransactionGenerator transactionGenerator)
    {
        _recipeTagCreateServiceDelegate = recipeTagCreateServiceDelegate;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<IRecipeTag> HandleAsync(CreateRecipeTagCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);
        var recipeTagCreateService = _recipeTagCreateServiceDelegate(cancellationToken);
        var result = await recipeTagCreateService.CreateAsync(command.Name);

        await transaction.CommitAsync(cancellationToken);
        return result;
    }
}