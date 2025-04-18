using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Contracts.Common;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Reasons;
using ProjectHermes.Xipona.Api.Endpoint.v1.Endpoints;
using ProjectHermes.Xipona.Api.Repositories.Users.Contexts;
using ProjectHermes.Xipona.Api.Repositories.Users.Entities;
using ProjectHermes.Xipona.Api.TestTools.AutoFixture;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using System;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

namespace ProjectHermes.Xipona.Api.Endpoint.IntegrationTests.v1.Endpoints;

public class AccountEndpointIntegrationTests
{
    public sealed class Login : IAssemblyFixture<DockerFixture>
    {
        private readonly LoginFixture _fixture;
        public Login(DockerFixture dockerFixture)
        {
            _fixture = new LoginFixture(dockerFixture);
        }

        [Fact]
        public async Task Login_WithValidAccessToken_ShouldCreateUser()
        {
            // Arrange
            _fixture.SetupExpectedUser();
            await _fixture.PrepareDatabaseAsync();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedUser);

            // Act
            var result = await _fixture.ActAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContent>();

            using var assertionServiceScope = _fixture.CreateServiceScope();

            var users = await _fixture.LoadAllUsersAsync(assertionServiceScope);
            users.Should().HaveCount(1);
            var user = users[0];
            user.Should().BeEquivalentTo(_fixture.ExpectedUser,
                opt => opt.ExcludeRowVersion().WithCreatedAtPrecision(TimeSpan.FromSeconds(30)));
        }

        private sealed class LoginFixture : AccountEndpointFixture
        {
            public LoginFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public User? ExpectedUser { get; private set; }

            public async Task<IResult> ActAsync()
            {
                var scope = CreateServiceScope();
                var ctx = new DefaultHttpContext();
                ctx.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIwMDVjNDJkYy0zNTI5LTQwODYtOTY4OS1kMjNiNjNkYTYzY2EifQ.e8h7WEI5NhKdP1dz71haA85V1WqkPqR-kKtguRT8hVj257w2hYsqH39ffdNnGxxCIuq5scZt5qSzfnx5rQuRz_YbF0IdmN8hIFyjWuFadP9tXrTq9x_xU45i_E1oxQQrHcD1_9SLWE8WiaAqY4stv7Nz1Kot0Z-W1HRXr9AXBm296bTg3SRH2NrxDv2h9onRPNPAduLx_ZRN4B7IZAYatHY5ki39JTzo7J9X9AfxqNEdudUOLU7XYcVx8VfjSx3VU0DlL8E0nZ4zW1K_TcN3-iayguhPcj5_4fjRi05ZLBHpXJE4N4XpXZrsJtx1HTEH1ymzuxzF4cBu17fgjEpJ0w";

                return await AccountEndpoints.Login(
                    ctx,
                    new JwtSecurityTokenHandler(),
                    scope.ServiceProvider.GetRequiredService<ICommandDispatcher>(),
                    scope.ServiceProvider.GetRequiredService<IToContractConverter<IReason, ErrorContract>>(),
                    default);
            }

            public async Task PrepareDatabaseAsync()
            {
                await ApplyMigrationsAsync(ArrangeScope);
                //var context = ArrangeScope.ServiceProvider.GetRequiredService<UserContext>();

                //await context.SaveChangesAsync();
            }

            public void SetupExpectedUser()
            {
                ExpectedUser = new User()
                {
                    Id = Guid.Parse("005c42dc-3529-4086-9689-d23b63da63ca"),
                    CreatedAt = DateTimeOffset.UtcNow,
                };
            }

        }
    }

    private abstract class AccountEndpointFixture : DatabaseFixture
    {
        protected AccountEndpointFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            ArrangeScope = CreateServiceScope();
        }

        protected readonly IServiceScope ArrangeScope;

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<UserContext>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ArrangeScope.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
