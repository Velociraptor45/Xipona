using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common;

public class RecordStructTests
{
    private readonly Fixture _fixture;

    public RecordStructTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new DomainCustomization());
    }

    public static IEnumerable<object?[]> GetRecordStructs()
    {
        var types = typeof(ItemId).Assembly
            .GetTypes()
            .Where(t => t.IsValueType
                        && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEquatable<>)))
            .Select(t => new object[] { t })
            .ToList();
        return types;
    }

    [Theory]
    [MemberData(nameof(GetRecordStructs))]
    public void ToString_ShouldReturnNoBrackets(Type type)
    {
        // Arrange
        var sut = _fixture.Create(type, new SpecimenContext(_fixture));

        // Act
        var result = sut.ToString();

        // Assert
        result.Should().NotContain(type.Name);
        result.Should().NotContain("}");
        result.Should().NotContain("{");
    }
}