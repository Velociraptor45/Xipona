using System.Linq;
using Xipona.Api.Contracts.Items.Commands.MergeItems;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Items.States.Merges;

namespace Xipona.Frontend.Infrastructure.Converters.Items.ToContract;

public class MergeItemsContractConverter : IToContractConverter<MergedItem, MergeItemsContract>
{
    public MergeItemsContract ToContract(MergedItem source)
    {
        return new MergeItemsContract(
            new MergedItemContract(
                source.Name,
                source.Types.Select(t => new MergedItemTypeContract(t.OriginalItem.Id, t.Name))));
    }
}
