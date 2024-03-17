using ProjectHermes.Xipona.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Commands;
using ProjectHermes.Xipona.Api.Core.Converter;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.RecipeTags;

public class CreateRecipeTagCommandConverter : IToDomainConverter<CreateRecipeTagContract, CreateRecipeTagCommand>
{
    public CreateRecipeTagCommand ToDomain(CreateRecipeTagContract source)
    {
        return new CreateRecipeTagCommand(source.Name);
    }
}