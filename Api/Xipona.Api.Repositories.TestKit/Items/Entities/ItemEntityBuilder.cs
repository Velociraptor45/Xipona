using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.Repositories.Items.Entities;
using Item = ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item;
using ItemType = ProjectHermes.Xipona.Api.Repositories.Items.Entities.ItemType;

namespace ProjectHermes.Xipona.Api.Repositories.TestKit.Items.Entities;

public class ItemEntityBuilder : TestBuilder<Item>
{
    public ItemEntityBuilder()
    {
        WithQuantityTypeInPacket(new DomainTestBuilder<QuantityTypeInPacket>().Create().ToInt());
        WithQuantityType(new DomainTestBuilder<QuantityType>().Create().ToInt());
        WithoutPredecessor();
    }

    public ItemEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public ItemEntityBuilder WithName(string name)
    {
        FillPropertyWith(p => p.Name, name);
        return this;
    }

    public ItemEntityBuilder WithDeleted(bool deleted)
    {
        FillPropertyWith(p => p.Deleted, deleted);
        return this;
    }

    public ItemEntityBuilder WithComment(string comment)
    {
        FillPropertyWith(p => p.Comment, comment);
        return this;
    }

    public ItemEntityBuilder WithIsTemporary(bool isTemporary)
    {
        FillPropertyWith(p => p.IsTemporary, isTemporary);
        return this;
    }

    public ItemEntityBuilder WithQuantityType(int quantityType)
    {
        FillPropertyWith(p => p.QuantityType, quantityType);
        return this;
    }

    public ItemEntityBuilder WithQuantityInPacket(float? quantityInPacket)
    {
        FillPropertyWith(p => p.QuantityInPacket, quantityInPacket);
        return this;
    }

    public ItemEntityBuilder WithoutQuantityInPacket()
    {
        return WithQuantityInPacket(null);
    }

    public ItemEntityBuilder WithQuantityTypeInPacket(int? quantityTypeInPacket)
    {
        FillPropertyWith(p => p.QuantityTypeInPacket, quantityTypeInPacket);
        return this;
    }

    public ItemEntityBuilder WithoutQuantityTypeInPacket()
    {
        return WithQuantityTypeInPacket(null);
    }

    public ItemEntityBuilder WithItemCategoryId(Guid? itemCategoryId)
    {
        FillPropertyWith(p => p.ItemCategoryId, itemCategoryId);
        return this;
    }

    public ItemEntityBuilder WithoutItemCategoryId()
    {
        return WithItemCategoryId(null);
    }

    public ItemEntityBuilder WithManufacturerId(Guid? manufacturerId)
    {
        FillPropertyWith(p => p.ManufacturerId, manufacturerId);
        return this;
    }

    public ItemEntityBuilder WithoutManufacturerId()
    {
        return WithManufacturerId(null);
    }

    public ItemEntityBuilder WithCreatedFrom(Guid? createdFrom)
    {
        FillPropertyWith(p => p.CreatedFrom, createdFrom);
        return this;
    }

    public ItemEntityBuilder WithoutCreatedFrom()
    {
        return WithCreatedFrom(null);
    }

    public ItemEntityBuilder WithPredecessorId(Guid? predecessorId)
    {
        FillPropertyWith(p => p.PredecessorId, predecessorId);
        return this;
    }

    public ItemEntityBuilder WithoutPredecessorId()
    {
        return WithPredecessorId(null);
    }

    public ItemEntityBuilder WithPredecessor(Item? predecessor)
    {
        FillPropertyWith(p => p.Predecessor, predecessor);
        return this;
    }

    public ItemEntityBuilder WithoutPredecessor()
    {
        return WithPredecessor(null);
    }

    public ItemEntityBuilder WithItemType(ItemType itemType)
    {
        return WithItemTypes(itemType.ToMonoList());
    }

    public ItemEntityBuilder WithItemTypes(ICollection<ItemType> itemTypes)
    {
        FillPropertyWith(p => p.ItemTypes, itemTypes);
        return this;
    }

    public ItemEntityBuilder WithEmptyItemTypes()
    {
        return WithItemTypes(new List<ItemType>());
    }

    // TCG keep
    public ItemEntityBuilder WithAvailableAt(AvailableAt availableAt)
    {
        return WithAvailableAt(new List<AvailableAt> { availableAt });
    }

    public ItemEntityBuilder WithAvailableAt(ICollection<AvailableAt> availableAt)
    {
        FillPropertyWith(p => p.AvailableAt, availableAt);
        return this;
    }

    public ItemEntityBuilder WithEmptyAvailableAt()
    {
        return WithAvailableAt(new List<AvailableAt>());
    }

    public ItemEntityBuilder WithCreatedAt(DateTimeOffset createdAt)
    {
        FillPropertyWith(p => p.CreatedAt, createdAt);
        return this;
    }

    public ItemEntityBuilder WithUpdatedOn(DateTimeOffset? updatedOn)
    {
        FillPropertyWith(p => p.UpdatedOn, updatedOn);
        return this;
    }

    public ItemEntityBuilder WithoutUpdatedOn()
    {
        return WithUpdatedOn(null);
    }
}