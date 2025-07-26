using Xipona.Api.ApplicationServices.Items.Commands.MergeItems;
using Xipona.Api.Contracts.Items.Commands.MergeItems;
using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Searches;

namespace Xipona.Api.Endpoint.v1.Converters.ToDomain.Items;

public class MergeItemsCommandConverter : IToDomainConverter<MergeItemsContract, MergeItemsCommand>
{
    public MergeItemsCommand ToDomain(MergeItemsContract source)
    {
        return new MergeItemsCommand(
            new MergedItem(
                new ItemName(source.Item.Name),
                source.Item.Types
                    .Select(t => new MergedItemType(new ItemId(t.OriginatingItemId), new ItemTypeName(t.Name)))
                    .ToList()));
    }
}
