using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/monitoring")]
public class MonitoringController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [Route("alive")]
    public IActionResult IsAlive()
    {
        return Ok(true);
    }
}