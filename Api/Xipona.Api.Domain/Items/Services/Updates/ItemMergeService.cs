using System.Text;
using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Models.Factories;
using Xipona.Api.Domain.Items.Ports;
using Xipona.Api.Domain.Items.Reasons;

namespace Xipona.Api.Domain.Items.Services.Updates;

public interface IItemMergeService
{
    Task<ItemId> MergeAsync(MergedItem item);
}

public class ItemMergeService : IItemMergeService
{
    private readonly IItemFactory _itemFactory;
    private readonly IItemTypeFactory _itemTypeFactory;
    private readonly IItemRepository _itemRepository;

    public ItemMergeService(IItemFactory itemFactory, IItemTypeFactory itemTypeFactory, IItemRepository itemRepository)
    {
        _itemFactory = itemFactory;
        _itemTypeFactory = itemTypeFactory;
        _itemRepository = itemRepository;
    }

    public async Task<ItemId> MergeAsync(MergedItem item)
    {
        var originalItemIds = item.Types.Select(t => t.OriginatingItemId).ToList();
        var distinctOriginalItemIds = originalItemIds.Distinct().ToList();

        if (originalItemIds.Count != distinctOriginalItemIds.Count)
            throw new DomainException(new CannotMergeItemWithItselfReason());

        var originalItems = (await _itemRepository.FindByAsync(originalItemIds)).ToList();

        // validate selected items
        foreach (var originalItem in originalItems)
        {
            if (originalItem.IsDeleted)
                throw new DomainException(new CannotMergeDeletedItemReason(originalItem.Id));
            if (originalItem.HasItemTypes)
                throw new DomainException(new CannotMergeItemWithTypesReason(originalItem.Id));
            if (originalItem.IsTemporary)
                throw new DomainException(new CannotMergeTemporaryItemReason(originalItem.Id));
        }

        EnsureSameItemCategory(originalItems);
        EnsureSameManufacturer(originalItems);
        EnsureSameQuantity(originalItems);

        // build new merged item
        Dictionary<ItemId, ItemTypeId> itemMergeMap = new();
        var types = item.Types
            .Select(t =>
            {
                var originalItem = GetOriginalItem(t.OriginatingItemId);
                var type = _itemTypeFactory.CreateNew(t.Name, originalItem.Availabilities);
                itemMergeMap[originalItem.Id] = type.Id;
                return type;
            })
            .ToList();

        var newItem = _itemFactory.CreateNew(item.Name, BuildComment(originalItems), originalItems[0].ItemQuantity,
            originalItems[0].ItemCategoryId!.Value, originalItems[0].ManufacturerId, null, types);
        await _itemRepository.StoreAsync(newItem);

        // mark original items as merged
        foreach (var originalItem in originalItems)
        {
            var newItemTypeId = itemMergeMap[originalItem.Id];
            originalItem.MarkAsMerged(newItem.Id, newItemTypeId);
            await _itemRepository.StoreAsync(originalItem);
        }

        return newItem.Id;

        IItem GetOriginalItem(ItemId itemId)
        {
            return originalItems.FirstOrDefault(i => i.Id == itemId)
                   ?? throw new DomainException(new ItemNotFoundReason(itemId));
        }
    }

    private static void EnsureSameQuantity(List<IItem> originalItems)
    {
        var firstItemQuantity = originalItems[0].ItemQuantity;
        foreach (var item in originalItems.Skip(1))
        {
            if (item.ItemQuantity != firstItemQuantity)
                throw new DomainException(new CannotMergeItemsWithDifferentQuantitiesReason());
        }
    }

    private static void EnsureSameItemCategory(List<IItem> originalItems)
    {
        var firstItemCategoryId = originalItems[0].ItemCategoryId;
        foreach (var item in originalItems.Skip(1))
        {
            if (item.ItemCategoryId != firstItemCategoryId)
                throw new DomainException(new CannotMergeItemsWithDifferentItemCategoriesReason());
        }
    }

    private static void EnsureSameManufacturer(List<IItem> originalItems)
    {
        var firstItemCategoryId = originalItems[0].ItemCategoryId;
        foreach (var item in originalItems.Skip(1))
        {
            if (item.ItemCategoryId != firstItemCategoryId)
                throw new DomainException(new CannotMergeItemsWithDifferentManufacturersReason());
        }
    }

    private static Comment BuildComment(List<IItem> originalItems)
    {
        var originalItemsWithComments = originalItems.Where(i => !string.IsNullOrWhiteSpace(i.Comment.Value)).ToList();
        if (originalItemsWithComments.Count == 0)
            return new(string.Empty);

        var commentStringBuilder = new StringBuilder();
        for (var i = 0; i < originalItemsWithComments.Count; i++)
        {
            if (i != 0)
                commentStringBuilder.Append(Environment.NewLine);

            commentStringBuilder.Append(originalItemsWithComments[i].Comment);
        }

        var newComment = commentStringBuilder.ToString();
        return new Comment(newComment);
    }
}

public record MergedItem(ItemName Name, IReadOnlyCollection<MergedItemType> Types);
public record MergedItemType(ItemId OriginatingItemId, ItemTypeName Name);
