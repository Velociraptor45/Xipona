using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.RecipeTags.Queries.GetAll;
using ProjectHermes.ShoppingList.Api.Contracts.RecipeTags.Queries.GetAll;
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
}