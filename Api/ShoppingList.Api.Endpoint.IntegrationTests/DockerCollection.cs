using Xunit;

namespace ProjectHermes.ShoppingList.Api.Endpoint.IntegrationTests;

[CollectionDefinition(Name)]
public class DockerCollection : ICollectionFixture<DockerFixture>
{
    public const string Name = "IntegrationTests";
}