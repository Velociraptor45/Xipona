using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Serializers;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Tests.Serializers;

public class ApiRequestSerializerTests
{
    public static readonly IEnumerable<object[]> SerializationTypes = typeof(IApiRequest)
        .Assembly
        .GetTypes()
        .Where(t => t.IsAssignableTo(typeof(IApiRequest)) && !t.IsAbstract)
        .Select(t => new object[] { t })
        .ToList();

    [Theory]
    [MemberData(nameof(SerializationTypes))]
    public void Serialize_Deserialize_WithValidData_ShouldReturnOriginalStructure(Type type)
    {
        // Arrange
        var sut = new ApiRequestSerializer();
        var fixture = new Fixture();

        var requests = new List<IApiRequest>()
        {
            (IApiRequest)fixture.Create(type, new SpecimenContext(fixture))
        };

        // Act
        var serialized = sut.Serialize(requests);
        var deserialized = sut.Deserialize<List<IApiRequest>>(serialized);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized.Should().HaveCount(1);
        var typedDeserialized = Convert.ChangeType(deserialized.First(), type)!;
        var expected = Convert.ChangeType(requests.First(), type)!;

        typedDeserialized.Should().BeEquivalentTo(expected, opt => opt.Excluding(info => info.Path.EndsWith("RequestId")));
    }
}