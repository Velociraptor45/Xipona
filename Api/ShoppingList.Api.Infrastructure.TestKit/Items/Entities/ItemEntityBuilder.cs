using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;
using Item = ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities.Item;
using ItemType = ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities.ItemType;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Items.Entities;

public class ItemEntityBuilder : TestBuilder<Item>
{
    public ItemEntityBuilder()
    {
        WithQuantityTypeInPacket(new DomainTestBuilder<QuantityTypeInPacket>().Create().ToInt());
        WithQuantityType(new DomainTestBuilder<QuantityType>().Create().ToInt());
    }

    public ItemEntityBuilder WithId(ItemId id)
    {
        return WithId(id.Value);
    }

    public ItemEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(i => i.Id, id);
        return this;
    }

    public ItemEntityBuilder WithDeleted(bool deleted)
    {
        FillPropertyWith(i => i.Deleted, deleted);
        return this;
    }

    public ItemEntityBuilder WithIsTemporary(bool isTemporary)
    {
        FillPropertyWith(i => i.IsTemporary, isTemporary);
        return this;
    }

    public ItemEntityBuilder WithQuantityTypeInPacket(int quantityTypeInPacket)
    {
        FillPropertyWith(i => i.QuantityTypeInPacket, quantityTypeInPacket);
        return this;
    }

    public ItemEntityBuilder WithQuantityType(int quantityType)
    {
        FillPropertyWith(i => i.QuantityType, quantityType);
        return this;
    }

    public ItemEntityBuilder WithEmptyAvailabilities()
    {
        return WithAvailabilities(Enumerable.Empty<AvailableAt>());
    }

    public ItemEntityBuilder WithAvailabilities(IEnumerable<AvailableAt> availabilities)
    {
        FillPropertyWith(i => i.AvailableAt, availabilities.ToList());
        return this;
    }

    public ItemEntityBuilder WithoutPredecessorId()
    {
        return WithPredecessorId(null);
    }

    public ItemEntityBuilder WithPredecessorId(ItemId id)
    {
        return WithPredecessorId(id.Value);
    }

    public ItemEntityBuilder WithPredecessorId(Guid? id)
    {
        FillPropertyWith(i => i.PredecessorId, id);
        return this;
    }

    public ItemEntityBuilder WithoutPredecessor()
    {
        return WithPredecessor(null);
    }

    public ItemEntityBuilder WithPredecessor(Item? predecessor)
    {
        FillPropertyWith(i => i.Predecessor, predecessor);
        return this;
    }

    public ItemEntityBuilder WithItemTypes(IEnumerable<ItemType> itemTypes)
    {
        FillPropertyWith(i => i.ItemTypes, itemTypes.ToList());
        return this;
    }

    public ItemEntityBuilder WithEmptyItemTypes()
    {
        return WithItemTypes(Enumerable.Empty<ItemType>());
    }

    public ItemEntityBuilder WithoutCreatedFrom()
    {
        return WithCreatedFrom(null);
    }

    public ItemEntityBuilder WithCreatedFrom(Guid? createFromId)
    {
        FillPropertyWith(i => i.CreatedFrom, createFromId);
        return this;
    }

    public ItemEntityBuilder WithItemCategoryId(Guid? itemCategoryId)
    {
        FillPropertyWith(i => i.ItemCategoryId, itemCategoryId);
        return this;
    }
}