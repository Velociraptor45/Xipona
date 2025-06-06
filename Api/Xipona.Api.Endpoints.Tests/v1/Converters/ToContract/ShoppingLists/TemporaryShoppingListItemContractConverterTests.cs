using AutoMapper;
using Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using Xipona.Api.Core.Tests.Converter;
using Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using Xipona.Api.Endpoint.v1.Converters.ToContract.ShoppingLists;
using Xipona.Api.TestTools.Extensions;

namespace Xipona.Api.Endpoints.Tests.v1.Converters.ToContract.ShoppingLists;

public class TemporaryShoppingListItemContractConverterTests :
    ToContractConverterTestBase<TemporaryShoppingListItemReadModel, TemporaryShoppingListItemContract,
        TemporaryShoppingListItemContractConverter>
{
    protected override TemporaryShoppingListItemContractConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<TemporaryShoppingListItemReadModel, TemporaryShoppingListItemContract> mapping)
    {
        mapping
            .ForCtorParam(nameof(TemporaryShoppingListItemContract.ItemId).LowerFirstChar(),
                opt => opt.MapFrom(src => src.Id.Value))
            .ForCtorParam(nameof(TemporaryShoppingListItemContract.IsInBasket).LowerFirstChar(),
                opt => opt.MapFrom(src => src.IsInBasket))
            .ForCtorParam(nameof(TemporaryShoppingListItemContract.QuantityInBasket).LowerFirstChar(),
                opt => opt.MapFrom(src => src.QuantityInBasket.Value));
    }
}