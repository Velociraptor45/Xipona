using Xunit;

namespace ShoppingList.Api.Endpoint.IntegrationsTests;

[CollectionDefinition("IntegrationTests")]
public class DockerCollection : ICollectionFixture<DockerFixture>
{
}