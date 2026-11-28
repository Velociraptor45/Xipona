using FluentAssertions;
using FluentAssertions.Equivalency;
using System.Security.Claims;
using Xipona.Api.Endpoint.Middleware;
using Xunit;

namespace Xipona.Api.WebApp.Auth;

public class RoleClaimsTransformationTests
{
    [Fact]
    public async Task TransformAsync_WithoutArray_ShouldNotTransform()
    {
        // Arrange
        var principal = new ClaimsPrincipal([new ClaimsIdentity([new Claim("role", "User")])]);
        var authOptions = new AuthenticationOptions { RoleClaimType = "role" };
        var sut = new RoleClaimsTransformation(authOptions);

        // Act
        var result = await sut.TransformAsync(principal);

        // Assert
        result.Claims.Should().HaveCount(1);
        var claim = result.Claims.First();
        claim.Type.Should().Be("role");
        claim.Value.Should().Be("User");
    }
    
    [Fact]
    public async Task TransformAsync_WithArrayAtRootLevel_ShouldTransform()
    {
        // Arrange
        var principal = new ClaimsPrincipal([new ClaimsIdentity([new Claim("role", "[\"User\", \"Admin\"]")])]);
        var authOptions = new AuthenticationOptions { RoleClaimType = "role" };
        var sut = new RoleClaimsTransformation(authOptions);
        Claim[] expectedResult =
        [
            new("role", "[\"User\", \"Admin\"]"),
            new("role", "User"),
            new("role", "Admin")
        ];

        // Act
        var result = await sut.TransformAsync(principal);

        // Assert
        var claims = result.Claims.ToList();
        claims.Should().HaveCount(3);
        claims.Should().BeEquivalentTo(expectedResult, opt => opt.Using(new ClaimComparer()));
    }
    
    [Fact]
    public async Task TransformAsync_WithNestedArray_ShouldTransform()
    {
        // Arrange
        var principal = new ClaimsPrincipal([new ClaimsIdentity([new Claim("role", "{\"sub\":[\"User\", \"Admin\"]}")])]);
        var authOptions = new AuthenticationOptions { RoleClaimType = "role.sub" };
        var sut = new RoleClaimsTransformation(authOptions);
        Claim[] expectedResult =
        [
            new("role", "{\"sub\":[\"User\", \"Admin\"]}"),
            new("role.sub", "User"),
            new("role.sub", "Admin")
        ];

        // Act
        var result = await sut.TransformAsync(principal);

        // Assert
        var claims = result.Claims.ToList();
        claims.Should().HaveCount(3);
        claims.Should().BeEquivalentTo(expectedResult, opt => opt.Using(new ClaimComparer()));
    }
    
    [Fact]
    public async Task TransformAsync_WithTwiceNestedArray_ShouldTransform()
    {
        // Arrange
        var principal = new ClaimsPrincipal([new ClaimsIdentity([new Claim("role", "{\"sub\":{\"sub2\":[\"User\", \"Admin\"]}}")])]);
        var authOptions = new AuthenticationOptions { RoleClaimType = "role.sub.sub2" };
        var sut = new RoleClaimsTransformation(authOptions);
        Claim[] expectedResult =
        [
            new("role", "{\"sub\":{\"sub2\":[\"User\", \"Admin\"]}}"),
            new("role.sub.sub2", "User"),
            new("role.sub.sub2", "Admin")
        ];

        // Act
        var result = await sut.TransformAsync(principal);

        // Assert
        var claims = result.Claims.ToList();
        claims.Should().HaveCount(3);
        claims.Should().BeEquivalentTo(expectedResult, opt => opt.Using(new ClaimComparer()));
    }
    
    [Fact]
    public async Task TransformAsync_WithTwiceNestedValue_ShouldTransform()
    {
        // Arrange
        var principal = new ClaimsPrincipal([new ClaimsIdentity([new Claim("role", "{\"sub\":{\"sub2\": \"Admin\"}}")])]);
        var authOptions = new AuthenticationOptions { RoleClaimType = "role.sub.sub2" };
        var sut = new RoleClaimsTransformation(authOptions);
        Claim[] expectedResult =
        [
            new("role", "{\"sub\":{\"sub2\": \"Admin\"}}"),
            new("role.sub.sub2", "Admin")
        ];

        // Act
        var result = await sut.TransformAsync(principal);

        // Assert
        var claims = result.Claims.ToList();
        claims.Should().HaveCount(2);
        claims.Should().BeEquivalentTo(expectedResult, opt => opt.Using(new ClaimComparer()));
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