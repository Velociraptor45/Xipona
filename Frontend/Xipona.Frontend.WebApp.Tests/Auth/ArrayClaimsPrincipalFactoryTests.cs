using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Moq;
using System.Security.Claims;
using System.Text.Json;
using Xipona.Frontend.WebApp.Auth;

namespace Xipona.Frontend.WebApp.Tests.Auth;

public class ArrayClaimsPrincipalFactoryTests
{
    private readonly Mock<IAccessTokenProviderAccessor> _accessTokenProviderAccessorMock = new();
    
    [Fact]
    public async ValueTask CreateUserAsync_WithoutArray_ShouldNotTransform()
    {
        // Arrange
        var account = new RemoteUserAccount
        {
            AdditionalProperties = new Dictionary<string, object> { { "role", "User" } }
        };
        var options = new RemoteAuthenticationUserOptions();
        var sut = new ArrayClaimsPrincipalFactory<RemoteUserAccount>(_accessTokenProviderAccessorMock.Object);
        Claim[] expectedResult =
        [
            new("role", "User")
        ];

        // Act
        var result = await sut.CreateUserAsync(account, options);

        // Assert
        result.Claims.Should().BeEquivalentTo(expectedResult, opt => opt.Using(new ClaimComparer()));
    }
    
    [Fact]
    public async ValueTask CreateUserAsync_WithArrayAtRootLevel_ShouldNotTransform()
    {
        // Arrange
        var element = JsonElement.Parse("[\"User\", \"Admin\"]");
        var account = new RemoteUserAccount
        {
            AdditionalProperties = new Dictionary<string, object> { { "role",  element } }
        };
        var options = new RemoteAuthenticationUserOptions();
        var sut = new ArrayClaimsPrincipalFactory<RemoteUserAccount>(_accessTokenProviderAccessorMock.Object);
        Claim[] expectedResult =
        [
            new("role", "User"),
            new("role", "Admin")
        ];

        // Act
        var result = await sut.CreateUserAsync(account, options);

        // Assert
        result.Claims.Should().BeEquivalentTo(expectedResult, opt => opt.Using(new ClaimComparer()));
    }
    
    [Fact]
    public async ValueTask CreateUserAsync_WithNestedArray_ShouldNotTransform()
    {
        // Arrange
        var element = JsonElement.Parse("{\"sub\":[\"User\", \"Admin\"]}");
        var account = new RemoteUserAccount
        {
            AdditionalProperties = new Dictionary<string, object> { { "role",  element } }
        };
        var options = new RemoteAuthenticationUserOptions();
        var sut = new ArrayClaimsPrincipalFactory<RemoteUserAccount>(_accessTokenProviderAccessorMock.Object);
        Claim[] expectedResult =
        [
            new("role", "{\"sub\":[\"User\", \"Admin\"]}"),
            new("role.sub", "User"),
            new("role.sub", "Admin")
        ];

        // Act
        var result = await sut.CreateUserAsync(account, options);

        // Assert
        result.Claims.Should().BeEquivalentTo(expectedResult, opt => opt.Using(new ClaimComparer()));
    }
    
    [Fact]
    public async ValueTask CreateUserAsync_WithTwiceNestedArray_ShouldNotTransform()
    {
        // Arrange
        var element = JsonElement.Parse("{\"sub\":{\"sub2\":[\"User\", \"Admin\"]}}");
        var account = new RemoteUserAccount
        {
            AdditionalProperties = new Dictionary<string, object> { { "role",  element } }
        };
        var options = new RemoteAuthenticationUserOptions();
        var sut = new ArrayClaimsPrincipalFactory<RemoteUserAccount>(_accessTokenProviderAccessorMock.Object);
        Claim[] expectedResult =
        [
            new("role", "{\"sub\":{\"sub2\":[\"User\", \"Admin\"]}}"),
            new("role.sub.sub2", "User"),
            new("role.sub.sub2", "Admin")
        ];

        // Act
        var result = await sut.CreateUserAsync(account, options);

        // Assert
        result.Claims.Should().BeEquivalentTo(expectedResult, opt => opt.Using(new ClaimComparer()));
    }
    
    [Fact]
    public async ValueTask CreateUserAsync_WithTwiceNestedValue_ShouldNotTransform()
    {
        // Arrange
        var element = JsonElement.Parse("{\"sub\":{\"sub2\": \"Admin\"}}");
        var account = new RemoteUserAccount
        {
            AdditionalProperties = new Dictionary<string, object> { { "role",  element } }
        };
        var options = new RemoteAuthenticationUserOptions();
        var sut = new ArrayClaimsPrincipalFactory<RemoteUserAccount>(_accessTokenProviderAccessorMock.Object);
        Claim[] expectedResult =
        [
            new("role", "{\"sub\":{\"sub2\": \"Admin\"}}"),
            new("role.sub.sub2", "Admin")
        ];

        // Act
        var result = await sut.CreateUserAsync(account, options);

        // Assert
        result.Claims.Should().BeEquivalentTo(expectedResult, opt => opt.Using(new ClaimComparer()));
    }

    private class ClaimComparer : IEquivalencyStep
    {
        public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
            IValidateChildNodeEquivalency valueChildNodes)
        {
            if (!(comparands.Expectation is Claim right) || !(comparands.Subject is Claim left))
            {
                return EquivalencyResult.ContinueWithNext;
            }

            left.Type.Should().Be(right.Type);
            left.Value.Should().Be(right.Value);
            
            return EquivalencyResult.EquivalencyProven;
        }
    }
}