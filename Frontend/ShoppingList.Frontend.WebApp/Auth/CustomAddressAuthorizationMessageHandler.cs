using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using ProjectHermes.ShoppingList.Frontend.WebApp.Configs;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Auth;

public class CustomAddressAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public CustomAddressAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation,
        ConnectionConfig config)
        : base(provider, navigation)
    {
        ConfigureHandler(new[] { config.ApiUri });
    }
}