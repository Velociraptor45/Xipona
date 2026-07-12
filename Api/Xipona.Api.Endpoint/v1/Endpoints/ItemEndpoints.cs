using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Threading;
using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.ApplicationServices.Items.Commands;
using Xipona.Api.ApplicationServices.Items.Commands.CreateItem;
using Xipona.Api.ApplicationServices.Items.Commands.CreateItemWithTypes;
using Xipona.Api.ApplicationServices.Items.Commands.DeleteItem;
using Xipona.Api.ApplicationServices.Items.Commands.ItemUpdateWithTypes;
using Xipona.Api.ApplicationServices.Items.Commands.MakeTemporaryItemPermanent;
using Xipona.Api.ApplicationServices.Items.Commands.MarkItemAsFavorite;
using Xipona.Api.ApplicationServices.Items.Commands.MergeItems;
using Xipona.Api.ApplicationServices.Items.Commands.ModifyItem;
using Xipona.Api.ApplicationServices.Items.Commands.ModifyItemWithTypes;
using Xipona.Api.ApplicationServices.Items.Commands.UnmarkItemAsFavorite;
using Xipona.Api.ApplicationServices.Items.Commands.UpdateItem;
using Xipona.Api.ApplicationServices.Items.Queries.AllQuantityTypes;
using Xipona.Api.ApplicationServices.Items.Queries.AllQuantityTypesInPacket;
using Xipona.Api.ApplicationServices.Items.Queries.GetItemTypePrices;
using Xipona.Api.ApplicationServices.Items.Queries.ItemById;
using Xipona.Api.ApplicationServices.Items.Queries.SearchItems;
using Xipona.Api.ApplicationServices.Items.Queries.SearchItemsByFilters;
using Xipona.Api.ApplicationServices.Items.Queries.SearchItemsByItemCategory;
using Xipona.Api.ApplicationServices.Items.Queries.SearchItemsForMerge;
using Xipona.Api.ApplicationServices.Items.Queries.SearchItemsForShoppingLists;
using Xipona.Api.ApplicationServices.Items.Queries.TotalSearchResultCounts;
using Xipona.Api.Contracts.Common;
using Xipona.Api.Contracts.Items.Commands.CreateItem;
using Xipona.Api.Contracts.Items.Commands.CreateItemWithTypes;
using Xipona.Api.Contracts.Items.Commands.MakeTemporaryItemPermanent;
using Xipona.Api.Contracts.Items.Commands.MergeItems;
using Xipona.Api.Contracts.Items.Commands.ModifyItem;
using Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using Xipona.Api.Contracts.Items.Commands.UpdateItem;
using Xipona.Api.Contracts.Items.Commands.UpdateItemPrice;
using Xipona.Api.Contracts.Items.Commands.UpdateItemWithTypes;
using Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using Xipona.Api.Contracts.Items.Queries.Get;
using Xipona.Api.Contracts.Items.Queries.GetItemTypePrices;
using Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using Xipona.Api.Contracts.Items.Queries.SearchItemsForMerge;
using Xipona.Api.Contracts.Items.Queries.SearchItemsForShoppingLists;
using Xipona.Api.Contracts.Items.Queries.Shared;
using Xipona.Api.Core.Converter;
using Xipona.Api.Core.Extensions;
using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.Common.Reasons;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Creations;
using Xipona.Api.Domain.Items.Services.Queries;
using Xipona.Api.Domain.Items.Services.Queries.Quantities;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Endpoint.v1.Endpoints;

public static class ItemEndpoints
{
    private const string _routeBase = "v1/items";

    public static void RegisterItemEndpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .RegisterGetItemById()
            .RegisterGetItemTypePrices()
            .RegisterGetTotalSearchResultCount()
            .RegisterSearchItems()
            .RegisterSearchItemsByFilter()
            .RegisterSearchItemsForShoppingList()
            .RegisterSearchItemsByItemCategory()
            .RegisterGetAllQuantityTypes()
            .RegisterGetAllQuantityTypesInPacket()
            .RegisterCreateItem()
            .RegisterCreateItemWithTypes()
            .RegisterModifyItemWithTypes()
            .RegisterModifyItem()
            .RegisterUpdateItem()
            .RegisterUpdateItemPrice()
            .RegisterUpdateItemWithTypes()
            .RegisterMakeTemporaryItemPermanent()
            .RegisterDeleteItem()
            .RegisterMergeItems()
            .RegisterSearchItemsForMerge()
            .RegisterMarkItemAsFavorite()
            .RegisterUnmarkItemAsFavorite();
    }

    private static IEndpointRouteBuilder RegisterGetItemById(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/{{id:guid}}", GetItemById)
            .WithName("GetItemById")
            .Produces<ItemContract>()
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetItemById(
        [FromRoute] Guid id,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<ItemReadModel, ItemContract> contractConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new ItemByIdQuery(new ItemId(id));
            var result = await queryDispatcher.DispatchAsync(query, cancellationToken);
            var contract = contractConverter.ToContract(result);
            return Results.Ok(contract);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);
            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterGetItemTypePrices(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/{{id:guid}}/type-prices", GetItemTypePrices)
            .WithName("GetItemTypePrices")
            .Produces<ItemTypePricesContract>()
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetItemTypePrices(
        [FromRoute] Guid id,
        [FromQuery] Guid storeId,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<ItemTypePricesReadModel, ItemTypePricesContract> contractConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetItemTypePricesQuery(new ItemId(id), new StoreId(storeId));
            var result = await queryDispatcher.DispatchAsync(query, cancellationToken);
            var contract = contractConverter.ToContract(result);
            return Results.Ok(contract);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);
            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterGetTotalSearchResultCount(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/search-result-count", GetTotalSearchResultCount)
            .WithName("GetTotalSearchResultCount")
            .Produces<int>()
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetTotalSearchResultCount(
        [FromQuery] string searchInput,
        [FromServices] IQueryDispatcher queryDispatcher,
        CancellationToken cancellationToken)
    {
        var query = new TotalSearchResultCountQuery(searchInput);
        var result = await queryDispatcher.DispatchAsync(query, cancellationToken);
        return Results.Ok(result);
    }

    private static IEndpointRouteBuilder RegisterSearchItems(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/search", SearchItems)
            .WithName("SearchItems")
            .Produces<List<SearchItemResultContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> SearchItems(
        [FromQuery] string searchInput,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<SearchItemResultReadModel, SearchItemResultContract> contractConverter,
        CancellationToken cancellationToken,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (pageSize > 100)
            return Results.BadRequest("Page size cannot be greater than 100");

        var query = new SearchItemQuery(searchInput, page, pageSize);
        var result = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (result.Count == 0)
            return Results.NoContent();

        var contract = contractConverter.ToContract(result).ToList();
        return Results.Ok(contract);
    }

    private static IEndpointRouteBuilder RegisterSearchItemsByFilter(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/filter", SearchItemsByFilter)
            .WithName("SearchItemsByFilter")
            .Produces<List<SearchItemResultContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> SearchItemsByFilter(
        [FromQuery] Guid[] storeIds,
        [FromQuery] Guid[] itemCategoryIds,
        [FromQuery] Guid[] manufacturerIds,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<SearchItemResultReadModel, SearchItemResultContract> contractConverter,
        CancellationToken cancellationToken)
    {
        var query = new SearchItemsByFilterQuery(
            storeIds.Select(id => new StoreId(id)),
            itemCategoryIds.Select(id => new ItemCategoryId(id)),
            manufacturerIds.Select(id => new ManufacturerId(id)));

        var result = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (result.Count == 0)
            return Results.NoContent();

        var contract = contractConverter.ToContract(result).ToList();
        return Results.Ok(contract);
    }

    private static IEndpointRouteBuilder RegisterSearchItemsForShoppingList(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/search/{{storeId:guid}}", SearchItemsForShoppingList)
            .WithName("SearchItemsForShoppingList")
            .Produces<List<SearchItemForShoppingListResultContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> SearchItemsForShoppingList(
        [FromRoute] Guid storeId,
        [FromQuery] string searchInput,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices]
        IToContractConverter<SearchItemForShoppingResultReadModel, SearchItemForShoppingListResultContract>
            contractConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new SearchItemsForShoppingListQuery(searchInput, new StoreId(storeId));
            var result = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

            if (result.Count == 0)
                return Results.NoContent();

            var contract = contractConverter.ToContract(result).ToList();
            return Results.Ok(contract);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.StoreNotFound)
                return Results.NotFound(errorContract);
            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterSearchItemsByItemCategory(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/search/by-item-category/{{itemCategoryId:guid}}", SearchItemsByItemCategory)
            .WithName("SearchItemsByItemCategory")
            .Produces<List<SearchItemByItemCategoryResultContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> SearchItemsByItemCategory(
        [FromRoute] Guid itemCategoryId,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices]
        IToContractConverter<SearchItemByItemCategoryResult, SearchItemByItemCategoryResultContract> contractConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new SearchItemsByItemCategoryQuery(new ItemCategoryId(itemCategoryId));
            var result = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

            if (result.Count == 0)
                return Results.NoContent();

            var contract = contractConverter.ToContract(result).ToList();
            return Results.Ok(contract);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ItemCategoryNotFound or ErrorReasonCode.StoresNotFound)
                return Results.NotFound(errorContract);
            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterGetAllQuantityTypes(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/quantity-types", GetAllQuantityTypes)
            .WithName("GetAllQuantityTypes")
            .Produces<List<QuantityTypeContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetAllQuantityTypes(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<QuantityTypeReadModel, QuantityTypeContract> contractConverter,
        CancellationToken cancellationToken)
    {
        var query = new AllQuantityTypesQuery();
        var result = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (result.Count == 0)
            return Results.NoContent();

        var contract = contractConverter.ToContract(result).ToList();
        return Results.Ok(contract);
    }

    private static IEndpointRouteBuilder RegisterGetAllQuantityTypesInPacket(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/quantity-types-in-packet", GetAllQuantityTypesInPacket)
            .WithName("GetAllQuantityTypesInPacket")
            .Produces<List<QuantityTypeInPacketContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetAllQuantityTypesInPacket(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices]
        IToContractConverter<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract> contractConverter,
        CancellationToken cancellationToken)
    {
        var query = new AllQuantityTypesInPacketQuery();
        var result = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (result.Count == 0)
            return Results.NoContent();

        var contract = contractConverter.ToContract(result).ToList();
        return Results.Ok(contract);
    }

    private static IEndpointRouteBuilder RegisterCreateItem(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{_routeBase}/without-types", CreateItem)
            .WithName("CreateItem")
            .Produces<ItemContract>(StatusCodes.Status201Created)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> CreateItem(
        [FromBody] CreateItemContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<CreateItemContract, ItemCreation> domainConverter,
        [FromServices] IToContractConverter<ItemReadModel, ItemContract> contractConverter,
        CancellationToken cancellationToken)
    {
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null
            || contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null)
        {
            return Results.BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        try
        {
            var model = domainConverter.ToDomain(contract);
            var command = new CreateItemCommand(model);
            var result = await commandDispatcher.DispatchAsync(command, cancellationToken);
            var createdContract = contractConverter.ToContract(result);
            return Results.CreatedAtRoute("GetItemById", new { id = createdContract.Id }, createdContract);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterCreateItemWithTypes(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{_routeBase}/with-types", CreateItemWithTypes)
            .WithName("CreateItemWithTypes")
            .Produces<ItemContract>(StatusCodes.Status201Created)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> CreateItemWithTypes(
        [FromBody] CreateItemWithTypesContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<CreateItemWithTypesContract, IItem> domainConverter,
        [FromServices] IToContractConverter<ItemReadModel, ItemContract> contractConverter,
        CancellationToken cancellationToken)
    {
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null
           || contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null)
        {
            return Results.BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        try
        {
            var model = domainConverter.ToDomain(contract);
            var command = new CreateItemWithTypesCommand(model);
            var result = await commandDispatcher.DispatchAsync(command, cancellationToken);
            var createdContract = contractConverter.ToContract(result);
            return Results.CreatedAtRoute("GetItemById", new { id = createdContract.Id }, createdContract);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterModifyItemWithTypes(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/with-types/{{id:guid}}/modify", ModifyItemWithTypes)
            .WithName("ModifyItemWithTypes")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> ModifyItemWithTypes(
        [FromRoute] Guid id,
        [FromBody] ModifyItemWithTypesContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<(Guid, ModifyItemWithTypesContract), ModifyItemWithTypesCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null
            || contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null)
        {
            return Results.BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        try
        {
            var command = domainConverter.ToDomain((id, contract));
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterModifyItem(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/without-types/{{id:guid}}/modify", ModifyItem)
            .WithName("ModifyItem")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> ModifyItem(
        [FromRoute] Guid id,
        [FromBody] ModifyItemContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<(Guid, ModifyItemContract), ModifyItemCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null
            || contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null)
        {
            return Results.BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        try
        {
            var command = domainConverter.ToDomain((id, contract));
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterUpdateItem(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/without-types/{{id:guid}}/update", UpdateItem)
            .WithName("UpdateItem")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> UpdateItem(
        [FromRoute] Guid id,
        [FromBody] UpdateItemContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<(Guid, UpdateItemContract), UpdateItemCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null
            || contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null)
        {
            return Results.BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        try
        {
            var command = domainConverter.ToDomain((id, contract));
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterUpdateItemPrice(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/{{id:guid}}/update-price", UpdateItemPrice)
            .WithName("UpdateItemPrice")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> UpdateItemPrice(
        [FromRoute] Guid id,
        [FromBody] UpdateItemPriceContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<(Guid, UpdateItemPriceContract), UpdateItemPriceCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = domainConverter.ToDomain((id, contract));
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ItemNotFound or ErrorReasonCode.ItemTypeNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterUpdateItemWithTypes(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/with-types/{{id:guid}}/update", UpdateItemWithTypes)
            .WithName("UpdateItemWithTypes")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> UpdateItemWithTypes(
        [FromRoute] Guid id,
        [FromBody] UpdateItemWithTypesContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<(Guid, UpdateItemWithTypesContract), UpdateItemWithTypesCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null
            || contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null)
        {
            return Results.BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        try
        {
            var command = domainConverter.ToDomain((id, contract));
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ItemNotFound or ErrorReasonCode.StoreNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterMakeTemporaryItemPermanent(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}/temporary/{{id:guid}}", MakeTemporaryItemPermanent)
            .WithName("MakeTemporaryItemPermanent")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> MakeTemporaryItemPermanent(
        [FromRoute] Guid id,
        [FromBody] MakeTemporaryItemPermanentContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<(Guid, MakeTemporaryItemPermanentContract), MakeTemporaryItemPermanentCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null
            || contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null)
        {
            return Results.BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        try
        {
            var command = domainConverter.ToDomain((id, contract));
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterDeleteItem(this IEndpointRouteBuilder builder)
    {
        builder.MapDelete($"/{_routeBase}/{{id:guid}}", DeleteItem)
            .WithName("DeleteItem")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> DeleteItem(
        [FromRoute] Guid id,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeleteItemCommand(new ItemId(id));
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterMergeItems(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{_routeBase}/merge", MergeItems)
            .WithName("MergeItems")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> MergeItems(
        [FromBody] MergeItemsContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToDomainConverter<MergeItemsContract, MergeItemsCommand> commandConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = commandConverter.ToDomain(contract);
            var itemId = await commandDispatcher.DispatchAsync(command, cancellationToken);
            return Results.CreatedAtRoute("GetItemById", new { id = itemId.Value }, itemId.Value);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterSearchItemsForMerge(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/merge/search", SearchItemsForMerge)
            .WithName("SearchItemsForMerge")
            .Produces<List<SearchItemsForMergeResultContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> SearchItemsForMerge(
        [FromQuery] Guid itemCategory,
        [FromQuery] Guid? manufacturer,
        [FromQuery] int quantityType,
        [FromQuery] float? quantity,
        [FromQuery] int? quantityTypeInPacket,
        [FromQuery] Guid[] excludedItemIds,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<SearchItemsForMergeResult, SearchItemsForMergeResultContract> contractConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        if ((quantity is null && quantityTypeInPacket is not null)
            || (quantity is not null && quantityTypeInPacket is null))
            return Results.BadRequest("Quantity and QuantityTypeInPacket must either both be null or both not be null");

        try
        {
            ItemQuantityInPacket? itemQuantityInPacket = null;
            if (quantity is not null && quantityTypeInPacket is not null)
                itemQuantityInPacket = new ItemQuantityInPacket(
                    new Quantity(quantity.Value),
                    quantityTypeInPacket.Value.ToEnum<QuantityTypeInPacket>());

            var command = new SearchItemsForMergeQuery(
                new ItemCategoryId(itemCategory),
                manufacturer is null ? null : new ManufacturerId(manufacturer.Value),
                new ItemQuantity(
                    quantityType.ToEnum<QuantityType>(),
                    itemQuantityInPacket),
                excludedItemIds.Select(i => new ItemId(i)).ToList());

            var results = (await queryDispatcher.DispatchAsync(command, cancellationToken)).ToList();
            
            if (results.Count == 0)
                return Results.NoContent();
            
            var contracts = contractConverter.ToContract(results).ToList();
            return Results.Ok(contracts);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterMarkItemAsFavorite(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{_routeBase}/{{id:guid}}/favorite", MarkItemAsFavorite)
            .WithName("MarkItemAsFavorite")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> MarkItemAsFavorite(
        [FromRoute] Guid id,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new MarkItemAsFavoriteCommand(new ItemId(id));
            await commandDispatcher.DispatchAsync(command, cancellationToken);
            return Results.NoContent();
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterUnmarkItemAsFavorite(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{_routeBase}/{{id:guid}}/un-favorite", UnmarkItemAsFavorite)
            .WithName("UnmarkItemAsFavorite")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> UnmarkItemAsFavorite(
        [FromRoute] Guid id,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = new UnmarkItemAsFavoriteCommand(new ItemId(id));
            await commandDispatcher.DispatchAsync(command, cancellationToken);
            return Results.NoContent();
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }
    }
}