using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.ItemById;

public class ItemByIdQuery : IQuery<ItemReadModel>
{
    public ItemByIdQuery(ItemId itemId)
    {
        ItemId = itemId;
    }

    public ItemId ItemId { get; }
}