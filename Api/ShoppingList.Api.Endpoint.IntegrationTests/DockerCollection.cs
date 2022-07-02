using Xunit;

namespace ShoppingList.Api.Endpoint.IntegrationTests;

[CollectionDefinition("IntegrationTests")]
public class DockerCollection : ICollectionFixture<DockerFixture>
{
}