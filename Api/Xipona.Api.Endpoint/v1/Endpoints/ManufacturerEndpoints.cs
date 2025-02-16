using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.DeleteManufacturer;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Commands.ModifyManufacturer;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Queries.AllActiveManufacturers;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Queries.ManufacturerById;
using ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Queries.ManufacturerSearch;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.Manufacturers.Commands;
using ProjectHermes.Xipona.Api.Contracts.Manufacturers.Queries;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Shared;
using System.Threading;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;

public static class ManufacturerEndpoints
{
    private const string _routeBase = "v1/manufacturers";

    public static void RegisterManufacturerEndpoints(this IEndpointRouteBuilder builder)
    {
        builder
            .RegisterGetManufacturerById()
            .RegisterSearchManufacturersByName()
            .RegisterGetAllActiveManufacturers()
            .RegisterModifyManufacturer()
            .RegisterCreateManufacturer()
            .RegisterDeleteManufacturer();
    }

    private static IEndpointRouteBuilder RegisterGetManufacturerById(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/{{id:guid}}", GetManufacturerById)
            .WithName("GetManufacturerById")
            .Produces<ManufacturerContract>()
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetManufacturerById(
        [FromRoute] Guid id,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<IManufacturer, ManufacturerContract> contractConverter,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = new ManufacturerByIdQuery(new ManufacturerId(id));
            var result = await queryDispatcher.DispatchAsync(query, cancellationToken);
            var contract = contractConverter.ToContract(result);
            return Results.Ok(contract);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ManufacturerNotFound)
                return Results.NotFound(errorContract);
            return Results.UnprocessableEntity(errorContract);
        }
    }

    private static IEndpointRouteBuilder RegisterSearchManufacturersByName(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}", SearchManufacturersByName)
            .WithName("SearchManufacturersByName")
            .Produces<List<ManufacturerSearchResultContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> SearchManufacturersByName(
        [FromQuery] string searchInput,
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<ManufacturerSearchResultReadModel, ManufacturerSearchResultContract> contractConverter,
        CancellationToken cancellationToken,
        [FromQuery] bool includeDeleted = false)
    {
        searchInput = searchInput.Trim();
        if (string.IsNullOrEmpty(searchInput))
        {
            return Results.BadRequest("Search input mustn't be null or empty");
        }

        var query = new ManufacturerSearchQuery(searchInput, includeDeleted);
        var manufacturerReadModels = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (manufacturerReadModels.Count == 0)
            return Results.NoContent();

        var manufacturerContracts = contractConverter.ToContract(manufacturerReadModels).ToList();

        return Results.Ok(manufacturerContracts);
    }

    private static IEndpointRouteBuilder RegisterGetAllActiveManufacturers(this IEndpointRouteBuilder builder)
    {
        builder.MapGet($"/{_routeBase}/active", GetAllActiveManufacturers)
            .WithName("GetAllActiveManufacturers")
            .Produces<List<ManufacturerContract>>()
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> GetAllActiveManufacturers(
        [FromServices] IQueryDispatcher queryDispatcher,
        [FromServices] IToContractConverter<ManufacturerReadModel, ManufacturerContract> contractConverter,
        CancellationToken cancellationToken)
    {
        var query = new AllActiveManufacturersQuery();
        var readModels = (await queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (readModels.Count == 0)
            return Results.NoContent();

        var contracts = contractConverter.ToContract(readModels).ToList();

        return Results.Ok(contracts);
    }

    private static IEndpointRouteBuilder RegisterModifyManufacturer(this IEndpointRouteBuilder builder)
    {
        builder.MapPut($"/{_routeBase}", ModifyManufacturer)
            .WithName("ModifyManufacturer")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> ModifyManufacturer(
        [FromBody] ModifyManufacturerContract contract,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<ModifyManufacturerContract, ModifyManufacturerCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = domainConverter.ToDomain(contract);
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);

            if (e.Reason.ErrorCode == ErrorReasonCode.ManufacturerNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }

    private static IEndpointRouteBuilder RegisterCreateManufacturer(this IEndpointRouteBuilder builder)
    {
        builder.MapPost($"/{_routeBase}", CreateManufacturer)
            .WithName("CreateManufacturer")
            .Produces<ManufacturerContract>(StatusCodes.Status201Created)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> CreateManufacturer(
        [FromQuery] string name,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IManufacturer, ManufacturerContract> contractConverter,
        [FromServices] IToDomainConverter<string, CreateManufacturerCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        var command = domainConverter.ToDomain(name);
        var model = await commandDispatcher.DispatchAsync(command, cancellationToken);

        var contract = contractConverter.ToContract(model);

        return Results.CreatedAtRoute("GetManufacturerById", new { id = contract.Id }, contract);
    }

    private static IEndpointRouteBuilder RegisterDeleteManufacturer(this IEndpointRouteBuilder builder)
    {
        builder.MapDelete($"/{_routeBase}/{{id:guid}}", DeleteManufacturer)
            .WithName("DeleteManufacturer")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorContract>(StatusCodes.Status404NotFound)
            .Produces<ErrorContract>(StatusCodes.Status422UnprocessableEntity)
            .RequireAuthorization("User");

        return builder;
    }

    internal static async Task<IResult> DeleteManufacturer(
        [FromRoute] Guid id,
        [FromServices] ICommandDispatcher commandDispatcher,
        [FromServices] IToContractConverter<IReason, ErrorContract> errorContractConverter,
        [FromServices] IToDomainConverter<Guid, DeleteManufacturerCommand> domainConverter,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = domainConverter.ToDomain(id);
            await commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = errorContractConverter.ToContract(e.Reason);

            if (e.Reason.ErrorCode == ErrorReasonCode.ManufacturerNotFound)
                return Results.NotFound(errorContract);

            return Results.UnprocessableEntity(errorContract);
        }

        return Results.NoContent();
    }
}