using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Queries;

namespace Xipona.Api.ApplicationServices.Items.Commands.CreateItemWithTypes;

public class CreateItemWithTypesCommand : ICommand<ItemReadModel>
{
    public CreateItemWithTypesCommand(IItem item)
    {
        Item = item;
    }

    public IItem Item { get; }
}