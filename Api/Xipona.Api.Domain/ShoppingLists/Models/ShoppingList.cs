using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.ErrorReasons;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

public class ShoppingList : AggregateRoot, IShoppingList
{
    private readonly Dictionary<SectionId, IShoppingListSection> _sections;
    private readonly Dictionary<(ItemId, ItemTypeId?), Discount> _discounts;

    public ShoppingList(ShoppingListId id, StoreId storeId, DateTimeOffset? completionDate,
        IEnumerable<IShoppingListSection> sections, DateTimeOffset createdAt, IEnumerable<Discount> discounts)
    {
        Id = id;
        StoreId = storeId;
        CompletionDate = completionDate;
        CreatedAt = createdAt;
        _sections = sections.ToDictionary(s => s.Id);
        _discounts = discounts.ToDictionary(d => (d.ItemId, d.ItemTypeId));
    }

    public ShoppingListId Id { get; }
    public StoreId StoreId { get; }
    public DateTimeOffset? CompletionDate { get; private set; }
    public DateTimeOffset CreatedAt { get; }

    public IReadOnlyCollection<IShoppingListSection> Sections => _sections.Values.ToList().AsReadOnly();
    public IReadOnlyCollection<ShoppingListItem> Items => Sections.SelectMany(s => s.Items).ToList().AsReadOnly();
    public IReadOnlyCollection<Discount> Discounts => _discounts.Values.ToList().AsReadOnly();

    public void AddItem(ShoppingListItem item, SectionId sectionId, bool throwIfAlreadyPresent = true)
    {
        if (throwIfAlreadyPresent && Items.Any(it => it.Id == item.Id && it.TypeId == item.TypeId))
            throw new DomainException(new ItemAlreadyOnShoppingListReason(item.Id, Id));

        if (!_sections.TryGetValue(sectionId, out IShoppingListSection? section))
            throw new DomainException(new SectionNotPartOfStoreReason(sectionId, StoreId));

        _sections[sectionId] = section.AddItem(item, throwIfAlreadyPresent);
    }

    public void RemoveItemAndItsTypes(ItemId itemId)
    {
        var sections = _sections.Values.Where(s => s.ContainsItemOrItsTypes(itemId)).ToArray();
        if (sections.Length == 0)
            return;

        foreach (var section in sections)
        {
            _sections[section.Id] = section.RemoveItemAndItsTypes(itemId);
        }
    }

    public void RemoveItem(ItemId itemId)
    {
        RemoveItem(itemId, null);
    }

    public void RemoveItem(ItemId itemId, ItemTypeId? itemTypeId)
    {
        IShoppingListSection? section = GetItemSection(itemId, itemTypeId);
        if (section == null)
            return;

        _sections[section.Id] = section.RemoveItem(itemId, itemTypeId);
    }

    public void PutItemInBasket(ItemId itemId)
    {
        PutItemInBasket(itemId, null);
    }

    public void PutItemInBasket(ItemId itemId, ItemTypeId? itemTypeId)
    {
        IShoppingListSection? section = GetItemSection(itemId, itemTypeId);
        if (section == null)
            throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

        _sections[section.Id] = section.PutItemInBasket(itemId, itemTypeId);
    }

    public void RemoveFromBasket(ItemId itemId, ItemTypeId? itemTypeId)
    {
        IShoppingListSection? section = GetItemSection(itemId, itemTypeId);
        if (section == null)
            throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

        _sections[section.Id] = section.RemoveItemFromBasket(itemId, itemTypeId);
    }

    public void ChangeItemQuantity(ItemId itemId, ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        IShoppingListSection? section = GetItemSection(itemId, itemTypeId);
        if (section == null)
            throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

        _sections[section.Id] = section.ChangeItemQuantity(itemId, itemTypeId, quantity);
    }

    public void AddSection(IShoppingListSection section)
    {
        if (!_sections.TryAdd(section.Id, section))
            throw new DomainException(new SectionAlreadyInShoppingListReason(Id, section.Id));
    }

    public IShoppingList Finish(DateTimeOffset completionDate, IDateTimeService dateTimeService)
    {
        if (CompletionDate != null)
            throw new DomainException(new ShoppingListAlreadyFinishedReason(Id));

        CompletionDate = completionDate;

        var notInBasketSections = new Dictionary<SectionId, IShoppingListSection>(_sections);
        foreach (SectionId key in _sections.Keys)
        {
            notInBasketSections[key] = notInBasketSections[key].RemoveItemsInBasket();
        }

        foreach (SectionId key in notInBasketSections.Keys)
        {
            _sections[key] = _sections[key].RemoveItemsNotInBasket();
        }

        return new ShoppingList(ShoppingListId.New, StoreId, null, notInBasketSections.Values, dateTimeService.UtcNow,
            []);
    }

    public void TransferItem(SectionId sectionId, ItemId itemId, ItemTypeId? itemTypeId)
    {
        if (!_sections.TryGetValue(sectionId, out var newSection))
            throw new DomainException(new SectionNotFoundReason(sectionId));

        var oldSection = GetItemSection(itemId, itemTypeId);
        if (oldSection is null)
            throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId, itemTypeId));

        if (oldSection.Id == sectionId)
            return;

        var item = oldSection.Items.First(i => i.Id == itemId && i.TypeId == itemTypeId);

        _sections[oldSection.Id] = oldSection.RemoveItem(itemId, itemTypeId);
        _sections[newSection.Id] = newSection.AddItem(item);
    }

    public Discount? GetDiscountFor(ItemId itemId, ItemTypeId? itemTypeId)
    {
        return _discounts.TryGetValue((itemId, itemTypeId), out var discount) ? discount : null;
    }

    public void AddDiscount(Discount discount)
    {
        if (!IsItemOnShoppingList(discount.ItemId, discount.ItemTypeId))
            throw new DomainException(new ItemNotOnShoppingListReason(Id, discount.ItemId, discount.ItemTypeId));

        _discounts[(discount.ItemId, discount.ItemTypeId)] = discount;
    }

    public void RemoveDiscount(ItemId itemId, ItemTypeId? itemTypeId)
    {
        _discounts.Remove((itemId, itemTypeId));
    }

    private bool IsItemOnShoppingList(ItemId itemId, ItemTypeId? itemTypeId)
    {
        return GetItemSection(itemId, itemTypeId) is not null;
    }

    private IShoppingListSection? GetItemSection(ItemId itemId, ItemTypeId? itemTypeId)
    {
        return _sections.Values.FirstOrDefault(s => s.ContainsItem(itemId, itemTypeId));
    }
}