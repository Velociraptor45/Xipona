using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.CreateItem;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.CreateItemWithTypes;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.DeleteItem;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.ItemUpdateWithTypes;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.ModifyItem;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Commands.UpdateItem;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.AllQuantityTypesInPacket;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.GetItemTypePrices;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.ItemById;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.SearchItems;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.SearchItemsForShoppingLists;
using ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.TotalSearchResultCounts;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItem;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemPrice;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.UpdateItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.GetItemTypePrices;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsForShoppingLists;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Creations;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries.Quantities;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using System.Threading;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;

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
            //.RegisterSearchItemsByFilter()
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
            .RegisterDeleteItem();
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
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<SearchItemResultReadModel, SearchItemResultContract> contractConverter,
        CancellationToken cancellationToken)
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

    //private static IEndpointRouteBuilder RegisterSearchItemsByFilter(this IEndpointRouteBuilder builder)
    //{
    //    builder.MapGet($"/{_routeBase}/filter", SearchItemsByFilter)
    //        .WithName("SearchItemsByFilter")
    //        .Produces<List<SearchItemResultContract>>()
    //        .Produces(StatusCodes.Status204NoContent)
    //        .RequireAuthorization("User");

    //    return builder;
    //}

    //internal static async Task<IResult> SearchItemsByFilter(
    //    [FromQuery] IEnumerable<Guid> storeIds,
    //    [FromQuery] IEnumerable<Guid> itemCategoryIds,
    //    [FromQuery] IEnumerable<Guid> manufacturerIds,
    //    [FromServices] IQueryDispatcher queryDispatcher,
    //    [FromServices] IToContractConverter<SearchItemResultReadModel, SearchItemResultContract> contractConverter,
    //    CancellationToken cancellationToken)
    //{
    //    var query = new SearchItemsByFilterQuery(
    //        storeIds.Select(id => new StoreId(id)),
    //        itemCategoryIds.Select(id => new ItemCategoryId(id)),
    //        manufacturerIds.Select(id => new ManufacturerId(id)));

    //    var result = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

    //    if (result.Count == 0)
    //        return Results.NoContent();

    //    var contract = contractConverter.ToContract(result).ToList();
    //    return Results.Ok(contract);
    //}

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
}