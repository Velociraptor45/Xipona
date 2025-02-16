using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemDiscount;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemToShoppingList;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.FinishShoppingList;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemDiscount;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Queries.ActiveShoppingListByStoreId;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemDiscount;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.PutItemInBasket;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemDiscount;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromBasket;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using System.Threading;
using AddItemToShoppingListContract = ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList.AddItemToShoppingListContract;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
public static class ShoppingListEndpoints
{
    private const string _routeBase = "v1/shopping-lists";

    public static void RegisterShoppingListEndpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .RegisterGetActiveShoppingListByStoreId()
            .RegisterRemoveItemFromShoppingList()
            .RegisterAddTemporaryItemToShoppingList()
            .RegisterAddItemToShoppingList()
            .RegisterAddItemWithTypeToShoppingList()
            .RegisterAddItemsToShoppingLists()
            .RegisterPutItemInBasket()
            .RegisterRemoveItemFromBasket()
            .RegisterChangeItemQuantityOnShoppingList()
            .RegisterFinishList()
            .RegisterAddItemDiscount()
            .RegisterRemoveItemDiscount();
    }

    private static IEndpointRouteBuilder RegisterGetActiveShoppingListByStoreId(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/active/{{storeId:guid}}", GetActiveShoppingListByStoreId)
            .WithName("GetActiveShoppingListByStoreId")
            .Produces<ShoppingListContract>()
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetActiveShoppingListByStoreId(
        [FromRoute] Guid storeId,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<ShoppingListReadModel, ShoppingListContract> contractConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new ActiveShoppingListByStoreIdQuery(new StoreId(storeId));
            var readModel = await queryDispatcher.DispatchAsync(query, cancellationToken);
            var contract = contractConverter.ToContract(readModel);
            return Results.Ok(contract);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ShoppingListNotFound)
                return Results.NotFound(errorContract);
            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterRemoveItemFromShoppingList(this IEndpointRouteBuilder builder)
    {
        builder.MapDelete($"/{_routeBase}/{{id:guid}}/items", RemoveItemFromShoppingList)
            .WithName("RemoveItemFromShoppingList")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> RemoveItemFromShoppingList(
        [FromRoute] Guid id,
        [FromBody] RemoveItemFromShoppingListContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<ItemIdContract, OfflineTolerantItemId> itemIdConverter,
        CancellationToken cancellationToken)
    {
        OfflineTolerantItemId tolerantItemId;
        try
        {
            tolerantItemId = itemIdConverter.ToDomain(contract.ItemId);
        }
        catch (ArgumentException)
        {
            return Results.BadRequest("No item id was specified.");
        }

        var itemTypeId = contract.ItemTypeId.HasValue
            ? new ItemTypeId(contract.ItemTypeId.Value)
            : (ItemTypeId?)null;

        var command = new RemoveItemFromShoppingListCommand(
            new ShoppingListId(id),
            tolerantItemId,
            itemTypeId);

        try
        {
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ShoppingListNotFound or ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterAddTemporaryItemToShoppingList(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/{{id:guid}}/items/temporary", AddTemporaryItemToShoppingList)
            .WithName("AddTemporaryItemToShoppingList")
            .Produces<TemporaryShoppingListItemContract>()
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> AddTemporaryItemToShoppingList(
        [FromRoute] Guid id,
        [FromBody] AddTemporaryItemToShoppingListContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<(Guid, AddTemporaryItemToShoppingListContract), AddTemporaryItemToShoppingListCommand> domainConverter,
        [FromServices] IToContractConverter<TemporaryShoppingListItemReadModel, TemporaryShoppingListItemContract> contractConverter,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(contract.ItemName))
        {
            return Results.BadRequest("Item name mustn't be empty");
        }

        try
        {
            var command = domainConverter.ToDomain((id, contract));
            var shoppingListItem = await commandDispatcher.DispatchAsync(command, cancellationToken);
            var itemContract = contractConverter.ToContract(shoppingListItem);
            return Results.Ok(itemContract);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ShoppingListNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterAddItemToShoppingList(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/{{id:guid}}/items", AddItemToShoppingList)
            .WithName("AddItemToShoppingList")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> AddItemToShoppingList(
        [FromRoute] Guid id,
        [FromBody] AddItemToShoppingListContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        var command = new AddItemToShoppingListCommand(
            new ShoppingListId(id),
            new ItemId(contract.ItemId),
            contract.SectionId.HasValue ? new SectionId(contract.SectionId.Value) : null,
            new QuantityInBasket(contract.Quantity));

        try
        {
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ShoppingListNotFound or ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterAddItemWithTypeToShoppingList(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/{{id:guid}}/items/{{itemId:guid}}/{{itemTypeId:guid}}", AddItemWithTypeToShoppingList)
            .WithName("AddItemWithTypeToShoppingList")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> AddItemWithTypeToShoppingList(
        [FromRoute] Guid id,
        [FromRoute] Guid itemId,
        [FromRoute] Guid itemTypeId,
        [FromBody] AddItemWithTypeToShoppingListContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        var command = new AddItemWithTypeToShoppingListCommand(
            new ShoppingListId(id),
            new ItemId(itemId),
            new ItemTypeId(itemTypeId),
            contract.SectionId.HasValue ? new SectionId(contract.SectionId.Value) : null,
            new QuantityInBasket(contract.Quantity));

        try
        {
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ShoppingListNotFound or ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterAddItemsToShoppingLists(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/add-items-to-shopping-lists", AddItemsToShoppingLists)
            .WithName("AddItemsToShoppingLists")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> AddItemsToShoppingLists(
        [FromBody] AddItemsToShoppingListsContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<AddItemsToShoppingListsContract, AddItemsToShoppingListsCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        var command = domainConverter.ToDomain(contract);

        try
        {
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);

            if (e.Reason.ErrorCode is ErrorReasonCode.StoreNotFound or ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }
        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterPutItemInBasket(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/{{id:guid}}/items/basket/add", PutItemInBasket)
            .WithName("PutItemInBasket")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> PutItemInBasket(
        [FromRoute] Guid id,
        [FromBody] PutItemInBasketContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<ItemIdContract, OfflineTolerantItemId> itemIdConverter,
        CancellationToken cancellationToken)
    {
        OfflineTolerantItemId itemId;
        try
        {
            itemId = itemIdConverter.ToDomain(contract.ItemId);
        }
        catch (ArgumentException)
        {
            return Results.BadRequest("No item id was specified.");
        }

        var itemTypeId = contract.ItemTypeId.HasValue
            ? new ItemTypeId(contract.ItemTypeId.Value)
            : (ItemTypeId?)null;
        var command = new PutItemInBasketCommand(new ShoppingListId(id), itemId, itemTypeId);

        try
        {
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ShoppingListNotFound or ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterRemoveItemFromBasket(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/{{id:guid}}/items/basket/remove", RemoveItemFromBasket)
            .WithName("RemoveItemFromBasket")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> RemoveItemFromBasket(
        [FromRoute] Guid id,
        [FromBody] RemoveItemFromBasketContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<ItemIdContract, OfflineTolerantItemId> itemIdConverter,
        CancellationToken cancellationToken)
    {
        OfflineTolerantItemId itemId;
        try
        {
            itemId = itemIdConverter.ToDomain(contract.ItemId);
        }
        catch (ArgumentException)
        {
            return Results.BadRequest("No item id was specified.");
        }

        var itemTypeId = contract.ItemTypeId.HasValue
            ? new ItemTypeId(contract.ItemTypeId.Value)
            : (ItemTypeId?)null;
        var command = new RemoveItemFromBasketCommand(new ShoppingListId(id), itemId, itemTypeId);

        try
        {
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ShoppingListNotFound or ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterChangeItemQuantityOnShoppingList(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/{{id:guid}}/items/quantity", ChangeItemQuantityOnShoppingList)
            .WithName("ChangeItemQuantityOnShoppingList")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> ChangeItemQuantityOnShoppingList(
        [FromRoute] Guid id,
        [FromBody] ChangeItemQuantityOnShoppingListContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<ItemIdContract, OfflineTolerantItemId> itemIdConverter,
        CancellationToken cancellationToken)
    {
        OfflineTolerantItemId itemId;
        try
        {
            itemId = itemIdConverter.ToDomain(contract.ItemId);
        }
        catch (ArgumentException)
        {
            return Results.BadRequest("No item id was specified.");
        }

        var itemTypeId = contract.ItemTypeId.HasValue
            ? new ItemTypeId(contract.ItemTypeId.Value)
            : (ItemTypeId?)null;
        var command = new ChangeItemQuantityOnShoppingListCommand(new ShoppingListId(id), itemId,
            itemTypeId, new QuantityInBasket(contract.Quantity));

        try
        {
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ShoppingListNotFound or ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterFinishList(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/{{id:guid}}/finish", FinishList)
            .WithName("FinishList")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> FinishList(
        [FromRoute] Guid id,
        [FromQuery] DateTimeOffset? finishedAt,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        var command = new FinishShoppingListCommand(new ShoppingListId(id), finishedAt ?? DateTimeOffset.UtcNow);
        try
        {
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ShoppingListNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterAddItemDiscount(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/{{id:guid}}/items/add-discount", AddItemDiscount)
            .WithName("AddItemDiscount")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> AddItemDiscount(
        [FromRoute] Guid id,
        [FromBody] AddItemDiscountContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<(Guid, AddItemDiscountContract), AddItemDiscountCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        var command = domainConverter.ToDomain((id, contract));
        try
        {
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ShoppingListNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterRemoveItemDiscount(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/{{id:guid}}/items/remove-discount", RemoveItemDiscount)
            .WithName("RemoveItemDiscount")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> RemoveItemDiscount(
        [FromRoute] Guid id,
        [FromBody] RemoveItemDiscountContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<(Guid, RemoveItemDiscountContract), RemoveItemDiscountCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        var command = domainConverter.ToDomain((id, contract));
        try
        {
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ShoppingListNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }
}