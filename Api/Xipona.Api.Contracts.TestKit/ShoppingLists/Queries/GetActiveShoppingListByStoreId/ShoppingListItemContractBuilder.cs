using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.Xipona.Api.Contracts.TestKit.Common;
using ProjectHermes.Xipona.Api.Core.TestKit;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
public class ShoppingListItemContractBuilder : ContractTestBuilderBase<ShoppingListItemContract>
{
    public ShoppingListItemContractBuilder WithId(Guid id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public ShoppingListItemContractBuilder WithTypeId(Guid? typeId)
    {
        FillConstructorWith(nameof(typeId), typeId);
        return this;
    }

    public ShoppingListItemContractBuilder WithoutTypeId()
    {
        return WithTypeId(null);
    }

    public ShoppingListItemContractBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public ShoppingListItemContractBuilder WithoutName()
    {
        return WithName(null);
    }

    public ShoppingListItemContractBuilder WithIsDeleted(bool isDeleted)
    {
        FillConstructorWith(nameof(isDeleted), isDeleted);
        return this;
    }

    public ShoppingListItemContractBuilder WithComment(string comment)
    {
        FillConstructorWith(nameof(comment), comment);
        return this;
    }

    public ShoppingListItemContractBuilder WithoutComment()
    {
        return WithComment(null);
    }

    public ShoppingListItemContractBuilder WithIsTemporary(bool isTemporary)
    {
        FillConstructorWith(nameof(isTemporary), isTemporary);
        return this;
    }

    public ShoppingListItemContractBuilder WithPricePerQuantity(decimal pricePerQuantity)
    {
        FillConstructorWith(nameof(pricePerQuantity), pricePerQuantity);
        return this;
    }

    public ShoppingListItemContractBuilder WithQuantityType(QuantityTypeContract quantityType)
    {
        FillConstructorWith(nameof(quantityType), quantityType);
        return this;
    }

    public ShoppingListItemContractBuilder WithoutQuantityType()
    {
        return WithQuantityType(null);
    }

    public ShoppingListItemContractBuilder WithQuantityInPacket(float? quantityInPacket)
    {
        FillConstructorWith(nameof(quantityInPacket), quantityInPacket);
        return this;
    }

    public ShoppingListItemContractBuilder WithoutQuantityInPacket()
    {
        return WithQuantityInPacket(null);
    }

    public ShoppingListItemContractBuilder WithQuantityTypeInPacket(QuantityTypeInPacketContract quantityTypeInPacket)
    {
        FillConstructorWith(nameof(quantityTypeInPacket), quantityTypeInPacket);
        return this;
    }

    public ShoppingListItemContractBuilder WithoutQuantityTypeInPacket()
    {
        return WithQuantityTypeInPacket(null);
    }

    public ShoppingListItemContractBuilder WithItemCategory(ItemCategoryContract itemCategory)
    {
        FillConstructorWith(nameof(itemCategory), itemCategory);
        return this;
    }

    public ShoppingListItemContractBuilder WithoutItemCategory()
    {
        return WithItemCategory(null);
    }

    public ShoppingListItemContractBuilder WithManufacturer(ManufacturerContract manufacturer)
    {
        FillConstructorWith(nameof(manufacturer), manufacturer);
        return this;
    }

    public ShoppingListItemContractBuilder WithoutManufacturer()
    {
        return WithManufacturer(null);
    }

    public ShoppingListItemContractBuilder WithIsInBasket(bool isInBasket)
    {
        FillConstructorWith(nameof(isInBasket), isInBasket);
        return this;
    }

    public ShoppingListItemContractBuilder WithQuantity(float quantity)
    {
        FillConstructorWith(nameof(quantity), quantity);
        return this;
    }

    public ShoppingListItemContractBuilder WithIsDiscounted(bool isDiscounted)
    {
        FillConstructorWith(nameof(isDiscounted), isDiscounted);
        return this;
    }
}