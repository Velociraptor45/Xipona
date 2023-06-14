using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

public class ShoppingList : AggregateRoot, IShoppingList
{
    private readonly Dictionary<SectionId, IShoppingListSection> _sections;

    public ShoppingList(ShoppingListId id, StoreId storeId, DateTimeOffset? completionDate,
        IEnumerable<IShoppingListSection> sections)
    {
        Id = id;
        StoreId = storeId;
        CompletionDate = completionDate;
        _sections = sections.ToDictionary(s => s.Id);
    }

    public ShoppingListId Id { get; }
    public StoreId StoreId { get; }
    public DateTimeOffset? CompletionDate { get; private set; }

    public IReadOnlyCollection<IShoppingListSection> Sections => _sections.Values.ToList().AsReadOnly();
    public IReadOnlyCollection<IShoppingListItem> Items => Sections.SelectMany(s => s.Items).ToList().AsReadOnly();

    public void AddItem(IShoppingListItem item, SectionId sectionId, bool throwIfAlreadyPresent = true)
    {
        if (throwIfAlreadyPresent && Items.Any(it => it.Id == item.Id && it.TypeId == item.TypeId))
            throw new DomainException(new ItemAlreadyOnShoppingListReason(item.Id, Id));

        if (!_sections.ContainsKey(sectionId))
            throw new DomainException(new SectionNotPartOfStoreReason(sectionId, StoreId));

        var section = _sections[sectionId];
        _sections[sectionId] = section.AddItem(item, throwIfAlreadyPresent);
    }

    public void RemoveItemAndItsTypes(ItemId itemId)
    {
        var sections = _sections.Values.Where(s => s.ContainsItemOrItsTypes(itemId)).ToArray();
        if (!sections.Any())
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
        IShoppingListSection? section = _sections.Values.FirstOrDefault(s => s.ContainsItem(itemId, itemTypeId));
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
        IShoppingListSection? section = _sections.Values.FirstOrDefault(s => s.ContainsItem(itemId, itemTypeId));
        if (section == null)
            throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

        _sections[section.Id] = section.PutItemInBasket(itemId, itemTypeId);
    }

    public void RemoveFromBasket(ItemId itemId, ItemTypeId? itemTypeId)
    {
        IShoppingListSection? section = _sections.Values.FirstOrDefault(s => s.ContainsItem(itemId, itemTypeId));
        if (section == null)
            throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

        _sections[section.Id] = section.RemoveItemFromBasket(itemId, itemTypeId);
    }

    public void ChangeItemQuantity(ItemId itemId, ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        IShoppingListSection? section = _sections.Values.FirstOrDefault(s => s.ContainsItem(itemId, itemTypeId));
        if (section == null)
            throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId));

        _sections[section.Id] = section.ChangeItemQuantity(itemId, itemTypeId, quantity);
    }

    public void AddSection(IShoppingListSection section)
    {
        if (_sections.ContainsKey(section.Id))
            throw new DomainException(new SectionAlreadyInShoppingListReason(Id, section.Id));

        _sections.Add(section.Id, section);
    }

    public IShoppingList Finish(DateTimeOffset completionDate)
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

        return new ShoppingList(ShoppingListId.New, StoreId, null, notInBasketSections.Values);
    }

    public void TransferItem(SectionId sectionId, ItemId itemId, ItemTypeId? itemTypeId)
    {
        if (!_sections.TryGetValue(sectionId, out var newSection))
            throw new DomainException(new SectionNotFoundReason(sectionId));

        var oldSection = _sections.FirstOrDefault(s => s.Value.ContainsItem(itemId, itemTypeId)).Value;
        if (oldSection is null)
            throw new DomainException(new ItemNotOnShoppingListReason(Id, itemId, itemTypeId));

        if (oldSection.Id == sectionId)
            return;

        var item = oldSection.Items.First(i => i.Id == itemId && i.TypeId == itemTypeId);

        _sections[oldSection.Id] = oldSection.RemoveItem(itemId, itemTypeId);
        _sections[newSection.Id] = newSection.AddItem(item);
    }
}