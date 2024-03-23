using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.CreateItemWithTypes;

public class CreateItemWithTypesCommand : ICommand<ItemReadModel>
{
    public CreateItemWithTypesCommand(IItem item)
    {
        Item = item;
    }

    public IItem Item { get; }
}