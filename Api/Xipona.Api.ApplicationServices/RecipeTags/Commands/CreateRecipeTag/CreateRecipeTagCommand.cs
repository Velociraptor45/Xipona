using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.RecipeTags.Models;

namespace Xipona.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;

public class CreateRecipeTagCommand : ICommand<IRecipeTag>
{
    public CreateRecipeTagCommand(string name)
    {
        Name = name;
    }

    public string Name { get; }
}