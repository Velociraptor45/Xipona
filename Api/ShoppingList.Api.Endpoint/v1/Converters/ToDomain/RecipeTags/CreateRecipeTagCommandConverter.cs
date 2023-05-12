using ProjectHermes.ShoppingList.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;
using ProjectHermes.ShoppingList.Api.Contracts.RecipeTags.Commands;
using ProjectHermes.ShoppingList.Api.Core.Converter;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.RecipeTags;

public class CreateRecipeTagCommandConverter : IToDomainConverter<CreateRecipeTagContract, CreateRecipeTagCommand>
{
    public CreateRecipeTagCommand ToDomain(CreateRecipeTagContract source)
    {
        return new CreateRecipeTagCommand(source.Name);
    }
}