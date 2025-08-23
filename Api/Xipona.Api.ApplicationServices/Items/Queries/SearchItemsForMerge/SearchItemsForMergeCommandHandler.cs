using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Domain.Manufacturers.Models;

namespace Xipona.Api.ApplicationServices.Items.Queries.SearchItemsForMerge;

public record SearchItemsForMergeCommand(ItemCategoryId ItemCategoryId, ManufacturerId? ManufacturerId,
    ItemQuantity ItemQuantity, IReadOnlyCollection<ItemId> ExcludedItemIds) : ICommand<IEnumerable<SearchItemsForMergeResult>>;

public class SearchItemsForMergeCommandHandler : ICommandHandler<SearchItemsForMergeCommand, IEnumerable<SearchItemsForMergeResult>>
{
    private readonly Func<CancellationToken, ItemMergeSearchService> _serviceDelegate;

    public SearchItemsForMergeCommandHandler(Func<CancellationToken, ItemMergeSearchService> serviceDelegate)
    {
        _serviceDelegate = serviceDelegate;
    }

    public async Task<IEnumerable<SearchItemsForMergeResult>> HandleAsync(SearchItemsForMergeCommand command,
        CancellationToken cancellationToken)
    {
        var service = _serviceDelegate(cancellationToken);
        return await service.SearchAsync(command.ItemCategoryId, command.ManufacturerId, command.ItemQuantity,
            command.ExcludedItemIds);
    }
}
