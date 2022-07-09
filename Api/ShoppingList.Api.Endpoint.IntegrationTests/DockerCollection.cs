using Xunit;

namespace ProjectHermes.ShoppingList.Api.Endpoint.IntegrationTests;

[CollectionDefinition("IntegrationTests")]
public class DockerCollection : ICollectionFixture<DockerFixture>
{
}