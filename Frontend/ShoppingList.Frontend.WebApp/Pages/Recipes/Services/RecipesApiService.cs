using ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Recipes.Services;

public class RecipesApiService : IRecipesApiService
{
    private readonly IApiClient _client;
    private readonly IShoppingListNotificationService _notificationService;

    public RecipesApiService(IApiClient client, IShoppingListNotificationService notificationService)
    {
        _client = client;
        _notificationService = notificationService;
    }

    //public async Task<IEnumerable<IngredientQuantityType>> GetAllIngredientQuantityTypes()
    //{
    //    return await _client.GetAllIngredientQuantityTypes();
    //}

    //public async Task<IEnumerable<RecipeSearchResult>> SearchAsync(string searchInput)
    //{
    //    return await _client.SearchRecipesByNameAsync(searchInput);
    //}

    //public async Task<Recipe> GetAsync(Guid recipeId)
    //{
    //    try
    //    {
    //        return await _client.GetRecipeByIdAsync(recipeId);
    //    }
    //    catch (ApiException e)
    //    {
    //        var contract = e.DeserializeContent<ErrorContract>();
    //        _notificationService.NotifyError("Loading recipe failed", contract.Message);
    //    }

    //    return null;
    //}

    //public async Task<Recipe> CreateAsync(Recipe recipe)
    //{
    //    try
    //    {
    //        return await _client.CreateRecipeAsync(recipe);
    //    }
    //    catch (ApiException e)
    //    {
    //        var contract = e.DeserializeContent<ErrorContract>();
    //        _notificationService.NotifyError("Creating recipe failed", contract.Message);
    //    }

    //    return null;
    //}

    //public async Task<bool> ModifyAsync(Recipe recipe)
    //{
    //    try
    //    {
    //        await _client.ModifyRecipeAsync(recipe);
    //        return true;
    //    }
    //    catch (ApiException e)
    //    {
    //        var contract = e.DeserializeContent<ErrorContract>();
    //        _notificationService.NotifyError("Modifying recipe failed", contract.Message);
    //    }

    //    return false;
    //}
}