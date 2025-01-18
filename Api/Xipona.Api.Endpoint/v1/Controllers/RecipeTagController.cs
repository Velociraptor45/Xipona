using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;
using ProjectHermes.Xipona.Api.ApplicationServices.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Commands;
using ProjectHermes.Xipona.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Endpoint.v1.Converters;
using System.Threading;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Controllers;

[ApiController]
[Authorize(Policy = "User")]
[Route("v1/recipe-tags")]
public class RecipeTagController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IEndpointConverters _converters;

    public RecipeTagController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
        IEndpointConverters converters)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _converters = converters;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<RecipeTagContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("all")]
    public async Task<IActionResult> GetAllRecipeTagsAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetAllQuery();
        var recipeTags = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (recipeTags.Count == 0)
            return NoContent();

        var contracts = _converters.ToContract<IRecipeTag, RecipeTagContract>(recipeTags).ToList();
        return Ok(contracts);
    }

    [HttpPost]
    [ProducesResponseType(typeof(RecipeTagContract), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("")]
    public async Task<IActionResult> CreateRecipeTagAsync(CreateRecipeTagContract contract,
        CancellationToken cancellationToken = default)
    {
        var command = _converters.ToDomain<CreateRecipeTagContract, CreateRecipeTagCommand>(contract);
        try
        {
            var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
            var resultContract = _converters.ToContract<IRecipeTag, RecipeTagContract>(result);
            return CreatedAtAction(nameof(GetAllRecipeTagsAsync), new { }, resultContract);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            return UnprocessableEntity(errorContract);
        }
    }
}