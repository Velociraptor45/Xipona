using Microsoft.AspNetCore.Mvc;

namespace ShoppingList.WebApp.Controllers
{
    [ApiController]
    [Route("v1/shopping-list")]
    public class ShoppingListController : ControllerBase
    {
        [HttpGet]
        [Route("active-shopping-list/{storeId}")]
        public Task<IActionResult> GetActiveShoppingListByStoreId([FromRoute("storeId")] int storeId)
        {
        }
    }
}