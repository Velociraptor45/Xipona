using Xipona.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;
using Xipona.Api.Contracts.RecipeTags.Commands;
using Xipona.Api.Core.Converter;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.RecipeTags;

public class CreateRecipeTagCommandConverter : IToDomainConverter<CreateRecipeTagContract, CreateRecipeTagCommand>
{
    public CreateRecipeTagCommand ToDomain(CreateRecipeTagContract source)
    {
        return new CreateRecipeTagCommand(source.Name);
    }
}