using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Xipona.Api.ApplicationServices.Users.Commands.Login;
using Xipona.Api.Contracts.Users.Commands.Login;
using Xipona.Api.Domain.Users.Models;
using Xipona.Api.Endpoint.Middleware;
using Xipona.Api.Endpoint.v1.Endpoints;
using Xipona.Api.Endpoints.Tests.Common;
using Xipona.Api.Endpoints.Tests.Common.StatusResults;
using Xipona.Api.TestTools.Exceptions;

namespace Xipona.Api.Endpoints.Tests.v1.Endpoints.UserTests;

public class LoginTests : EndpointCommandWithReturnTypeTestsBase<Guid, LoginCommand, IUser, UserInfoContract, LoginTests.LoginFixture>
{
    public LoginTests() : base(new LoginFixture())
    {
    }

    [Fact]
    public async Task EndpointCall_WithMissingUserNameClaim_ShouldReturnBadRequest()
    {
        // Arrange
        Fixture.SetupParameters();
        Fixture.SetupCommand();
        Fixture.SetupDispatcherSuccess();
        Fixture.SetupHttpContextWithoutUserNameClaim();
        Fixture.SetupExpectedBadRequestMessageForMissingUserNameClaim();

        // Act
        var result = await Fixture.ExecuteTestMethod();

        // Assert
        result.Should().BeOfType<BadRequest<string>>();
        var badRequestResult = result as BadRequest<string>;
        badRequestResult!.Value.Should().BeEquivalentTo(Fixture.ExpectedBadRequestMessage);
    }

    public sealed class LoginFixture : EndpointCommandWithReturnTypeFixtureBase
    {
        private readonly Guid _userId = Guid.Parse("005c42dc-3529-4086-9689-d23b63da63ca");
        private HttpContext? _httpContext;
        private readonly AuthenticationOptions _authOptions = new()
        {
            NameClaimType = "given_name"
        };

        public LoginFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new BadRequestStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult());
        }

        public override string RoutePattern => "/v1/users/login";
        public override HttpMethod HttpMethod => HttpMethod.Post;
        public override Type OkResultReturnType => typeof(UserInfoContract);

        public override Task<IResult> ExecuteTestMethod()
        {
            TestPropertyNotSetException.ThrowIfNull(_httpContext);

            return UserEndpoints.Login(
                _httpContext,
                new JwtSecurityTokenHandler(),
                CommandDispatcherMock.Object,
                ErrorConverterMock.Object,
                _authOptions,
                default);
        }

        public override void SetupParameters()
        {
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Headers.Authorization = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIwMDVjNDJkYy0zNTI5LTQwODYtOTY4OS1kMjNiNjNkYTYzY2EiLCJnaXZlbl9uYW1lIjoiSm9obi1vaCBEZWVyIn0.CEBWi9GOoMZKC0Lu_qSXbCGbbv7UlCSeR_ttPAC1N-YVd2RXnHVAKe0BLhvgRdB8jMY0LXse2LrODcKtC0eLvFjMCg6TWGSuS1cM-QP-yrHRPpzvNNRZojkSSfeTQcmo475dSCcNpqMtJhvYj0d6bAuunFv0vm5yL3tWRNgtDKPUvsx8DkbTcq1-r6F8K7LCxUWjWO6mj-Vva0AbkJQIgO32e3j7Ny2ArykTpvFG00-5uVxc9po7OMIEw7ld23PFwqkrAD9Z9plxXK9GTTNHj-4sh0P-MqrONrXOlER5Bqx6lzis0freHPV_NspXXoMGK599S9eFG2CPsU7pR-SXjg";
        }

        public override void SetupParametersForBadRequest()
        {
            SetupHttpContextWithoutSubject();
        }

        public override void SetupExpectedBadRequestMessage()
        {
            ExpectedBadRequestMessage = "Token contains invalid 'sub' claim: ";
        }

        public void SetupExpectedBadRequestMessageForMissingUserNameClaim()
        {
            ExpectedBadRequestMessage = $"Token doesn't contain a 'given_name' claim";
        }

        private void SetupHttpContextWithoutSubject()
        {
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Headers.Authorization = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJnaXZlbl9uYW1lIjoiSm9obi1vaCBEZWVyIn0.H7H_ZVsKb1QKjzRLuH28EIc7v1QmlPUyvUe8uSDqK0__ymJoXUUPkRIGUmhSwdGdNaKtLjB6OyIlLYxCse-pcRKxD9uu0H1PvDgMZY_omzBFL9PJsp4ogqr6dw7n8KHXdhZ9OTEV80M_vXe8fISrOcRwEH6nUNMroVuzoO967W2LIME5yIpNiWQUPSTFlSe0A_Sak2widRSZIoMswZS9WlrQUCY4e3FS7-E9DoUDcZx0ZvzHhDEQRAlT_vOt9BFR2o6qd2Hp5Ixfj3AXJkhUJr84MKL5FMloFftqQbCFrA6ub05fYJisjC1r0yxAOEpJQp0dB8diZxWOnvGsmWJGBA";
        }

        public void SetupHttpContextWithoutUserNameClaim()
        {
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Headers.Authorization = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIwMDVjNDJkYy0zNTI5LTQwODYtOTY4OS1kMjNiNjNkYTYzY2EifQ.e8h7WEI5NhKdP1dz71haA85V1WqkPqR-kKtguRT8hVj257w2hYsqH39ffdNnGxxCIuq5scZt5qSzfnx5rQuRz_YbF0IdmN8hIFyjWuFadP9tXrTq9x_xU45i_E1oxQQrHcD1_9SLWE8WiaAqY4stv7Nz1Kot0Z-W1HRXr9AXBm296bTg3SRH2NrxDv2h9onRPNPAduLx_ZRN4B7IZAYatHY5ki39JTzo7J9X9AfxqNEdudUOLU7XYcVx8VfjSx3VU0DlL8E0nZ4zW1K_TcN3-iayguhPcj5_4fjRi05ZLBHpXJE4N4XpXZrsJtx1HTEH1ymzuxzF4cBu17fgjEpJ0w";
        }

        public override void RegisterEndpoints(WebApplication app)
        {
            app.RegisterUserEndpoints();
        }

        public override Guid GetCommandConverterInput()
        {
            return _userId;
        }

        public override void SetupCommand()
        {
            Command = new LoginCommand(new UserId(_userId));
        }

        public override void SetupExpectedResult()
        {
            ExpectedResult = new UserInfoContract()
            {
                DisplayName = "John-oh Deer"
            };
        }
    }
}