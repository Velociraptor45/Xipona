﻿using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Items.Entities;

public class ItemTypeEntityBuilder : TestBuilderBase<ItemType>
{
    public ItemTypeEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public ItemTypeEntityBuilder WithItemId(Guid itemId)
    {
        FillPropertyWith(p => p.ItemId, itemId);
        return this;
    }

    public ItemTypeEntityBuilder WithName(string name)
    {
        FillPropertyWith(p => p.Name, name);
        return this;
    }

    public ItemTypeEntityBuilder WithPredecessorId(Guid? predecessorId)
    {
        FillPropertyWith(p => p.PredecessorId, predecessorId);
        return this;
    }

    public ItemTypeEntityBuilder WithoutPredecessorId()
    {
        return WithPredecessorId(null);
    }

    public ItemTypeEntityBuilder WithItem(Item? item)
    {
        FillPropertyWith(p => p.Item, item);
        return this;
    }

    public ItemTypeEntityBuilder WithoutItem()
    {
        return WithItem(null);
    }

    public ItemTypeEntityBuilder WithPredecessor(ItemType? predecessor)
    {
        FillPropertyWith(p => p.Predecessor, predecessor);
        return this;
    }

    public ItemTypeEntityBuilder WithoutPredecessor()
    {
        return WithPredecessor(null);
    }

    public ItemTypeEntityBuilder WithAvailableAt(ICollection<ItemTypeAvailableAt> availableAt)
    {
        FillPropertyWith(p => p.AvailableAt, availableAt);
        return this;
    }

    public ItemTypeEntityBuilder WithEmptyAvailableAt()
    {
        return WithAvailableAt(new List<ItemTypeAvailableAt>());
    }
}