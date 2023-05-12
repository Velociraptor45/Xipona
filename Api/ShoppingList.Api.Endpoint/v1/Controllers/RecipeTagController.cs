using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.RecipeTags.Commands.CreateRecipeTag;
using ProjectHermes.ShoppingList.Api.ApplicationServices.RecipeTags.Queries.GetAll;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Api.Contracts.RecipeTags.Commands;
using ProjectHermes.ShoppingList.Api.Contracts.RecipeTags.Queries.GetAll;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;
using System.Threading;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
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
    [ProducesResponseType(typeof(IEnumerable<RecipeTagContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("all")]
    public async Task<IActionResult> GetAllRecipeTagsAsync(CancellationToken cancellationToken = default)
    {
        var query = new GetAllQuery();
        var recipeTags = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (!recipeTags.Any())
            return NoContent();

        var contracts = _converters.ToContract<IRecipeTag, RecipeTagContract>(recipeTags);
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