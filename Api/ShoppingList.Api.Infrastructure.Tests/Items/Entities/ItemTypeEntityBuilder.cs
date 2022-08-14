using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Tests.Items.Entities;

public class ItemTypeEntityBuilder : TestBuilder<ItemType>
{
    public ItemTypeEntityBuilder WithAvailabilities(IEnumerable<ItemTypeAvailableAt> availabilities)
    {
        FillPropertyWith(t => t.AvailableAt, availabilities.ToList());
        return this;
    }

    public ItemTypeEntityBuilder WithoutPredecessorId()
    {
        return WithPredecessorId(null);
    }

    public ItemTypeEntityBuilder WithPredecessorId(Guid? id)
    {
        FillPropertyWith(t => t.PredecessorId, id);
        return this;
    }

    public ItemTypeEntityBuilder WithoutPredecessor()
    {
        return WithPredecessor(null);
    }

    public ItemTypeEntityBuilder WithPredecessor(ItemType? predecessor)
    {
        FillPropertyWith(i => i.Predecessor, predecessor);
        return this;
    }

    public ItemTypeEntityBuilder WithoutItem()
    {
        return WithItem(null);
    }

    public ItemTypeEntityBuilder WithItem(Item? item)
    {
        FillPropertyWith(t => t.Item, item);
        return this;
    }
}