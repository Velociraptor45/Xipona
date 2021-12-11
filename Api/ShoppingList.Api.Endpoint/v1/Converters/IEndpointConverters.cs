namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters
{
    public interface IEndpointConverters
    {
        TContract ToContract<TDomain, TContract>(TDomain domain);
        TDomain ToDomain<TContract, TDomain>(TContract contract);
    }
}