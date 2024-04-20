using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;

public class CreateRecipeTagCommand : ICommand<IRecipeTag>
{
    public CreateRecipeTagCommand(string name)
    {
        Name = name;
    }

    public string Name { get; }
}