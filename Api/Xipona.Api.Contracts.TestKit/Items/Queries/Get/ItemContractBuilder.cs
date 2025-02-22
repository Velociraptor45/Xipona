using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Core.TestKit;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Items.Queries.Get;

public class ItemContractBuilder : TestBuilderBase<ItemContract>
{
    public ItemContractBuilder WithId(Guid id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public ItemContractBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public ItemContractBuilder WithoutName()
    {
        return WithName(null);
    }

    public ItemContractBuilder WithIsDeleted(bool isDeleted)
    {
        FillConstructorWith(nameof(isDeleted), isDeleted);
        return this;
    }

    public ItemContractBuilder WithComment(string comment)
    {
        FillConstructorWith(nameof(comment), comment);
        return this;
    }

    public ItemContractBuilder WithoutComment()
    {
        return WithComment(null);
    }

    public ItemContractBuilder WithIsTemporary(bool isTemporary)
    {
        FillConstructorWith(nameof(isTemporary), isTemporary);
        return this;
    }

    public ItemContractBuilder WithQuantityType(QuantityTypeContract quantityType)
    {
        FillConstructorWith(nameof(quantityType), quantityType);
        return this;
    }

    public ItemContractBuilder WithoutQuantityType()
    {
        return WithQuantityType(null);
    }

    public ItemContractBuilder WithQuantityInPacket(float? quantityInPacket)
    {
        FillConstructorWith(nameof(quantityInPacket), quantityInPacket);
        return this;
    }

    public ItemContractBuilder WithoutQuantityInPacket()
    {
        return WithQuantityInPacket(null);
    }

    public ItemContractBuilder WithQuantityTypeInPacket(QuantityTypeInPacketContract quantityTypeInPacket)
    {
        FillConstructorWith(nameof(quantityTypeInPacket), quantityTypeInPacket);
        return this;
    }

    public ItemContractBuilder WithoutQuantityTypeInPacket()
    {
        return WithQuantityTypeInPacket(null);
    }

    public ItemContractBuilder WithItemCategory(ItemCategoryContract itemCategory)
    {
        FillConstructorWith(nameof(itemCategory), itemCategory);
        return this;
    }

    public ItemContractBuilder WithoutItemCategory()
    {
        return WithItemCategory(null);
    }

    public ItemContractBuilder WithManufacturer(ManufacturerContract manufacturer)
    {
        FillConstructorWith(nameof(manufacturer), manufacturer);
        return this;
    }

    public ItemContractBuilder WithoutManufacturer()
    {
        return WithManufacturer(null);
    }

    public ItemContractBuilder WithAvailabilities(IEnumerable<ItemAvailabilityContract> availabilities)
    {
        FillConstructorWith(nameof(availabilities), availabilities);
        return this;
    }

    public ItemContractBuilder WithEmptyAvailabilities()
    {
        return WithAvailabilities(Enumerable.Empty<ItemAvailabilityContract>());
    }

    public ItemContractBuilder WithItemTypes(IEnumerable<ItemTypeContract> itemTypes)
    {
        FillConstructorWith(nameof(itemTypes), itemTypes);
        return this;
    }

    public ItemContractBuilder WithEmptyItemTypes()
    {
        return WithItemTypes(Enumerable.Empty<ItemTypeContract>());
    }
}