using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Queries;

namespace Xipona.Api.ApplicationServices.Items.Queries.ItemById;

public class ItemByIdQuery : IQuery<ItemReadModel>
{
    public ItemByIdQuery(ItemId itemId)
    {
        ItemId = itemId;
    }

    public ItemId ItemId { get; }
}