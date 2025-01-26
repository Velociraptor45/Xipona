using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Core.TestKit.Files;
using ProjectHermes.Xipona.Api.Secrets;
using ProjectHermes.Xipona.Api.Secrets.Configs;
using ProjectHermes.Xipona.Api.Secrets.Vault;
using ProjectHermes.Xipona.Api.Secrets.Vault.Config;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http.Json;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Xipona.Api.Secrets.Tests;
public class SecretLoadingServiceTests
{
    private readonly VaultServiceFixture _fixture = new();

    [Fact]
    public async Task LoadLoggingApiKey_FromVault_ShouldReturnApiKey()
    {
        // Arrange
        _fixture.SetupVaultStore();
        _fixture.SetupRetrievingToken();
        _fixture.SetupRetrievingApiKey();
        _fixture.SetupCreatingHttpClient();
        var sut = _fixture.CreateSut();

        // Act
        var result = await sut.LoadLoggingApiKey();

        // Assert
        result.Should().Be(_fixture.ApiKey);
    }

    [Fact]
    public async Task LoadConnectionStringsAsync_FromVault_ShouldReturnConnectionStrings()
    {
        // Arrange
        _fixture.SetupVaultStore();
        _fixture.SetupRetrievingToken();
        _fixture.SetupRetrievingDbCredentials();
        _fixture.SetupCreatingHttpClient();
        _fixture.SetupDatabaseConfig();
        var sut = _fixture.CreateSut();

        // Act
        var result = await sut.LoadConnectionStringsAsync();

        // Assert
        result.Should().BeEquivalentTo(_fixture.ExpectedDbConnectionString);
    }

    [Fact]
    public async Task LoadLoggingApiKey_FromEnv_ShouldReturnApiKey()
    {
        // Arrange
        _fixture.SetupLoggingApiKeyInEnv();
        _fixture.SetupEnvStore();
        var sut = _fixture.CreateSut();

        // Act
        var result = await sut.LoadLoggingApiKey();

        // Assert
        result.Should().Be(_fixture.ApiKey);
    }

    [Fact]
    public async Task LoadConnectionStringsAsync_FromEnv_ShouldReturnConnectionStrings()
    {
        // Arrange
        _fixture.SetupDbCredentialsInEnv();
        _fixture.SetupDatabaseConfig();
        _fixture.SetupEnvStore();
        var sut = _fixture.CreateSut();

        // Act
        var result = await sut.LoadConnectionStringsAsync();

        // Assert
        result.Should().BeEquivalentTo(_fixture.ExpectedDbConnectionString);
    }

    [Fact]
    public async Task LoadLoggingApiKey_FromEnvFile_ShouldReturnApiKey()
    {
        // Arrange
        _fixture.SetupLoggingApiKeyInEnvFile();
        _fixture.SetupEnvStore();
        var sut = _fixture.CreateSut();

        // Act
        var result = await sut.LoadLoggingApiKey();

        // Assert
        result.Should().Be(_fixture.ApiKey);
    }

    [Fact]
    public async Task LoadConnectionStringsAsync_FromEnvFile_ShouldReturnConnectionStrings()
    {
        // Arrange
        _fixture.SetupDbCredentialsInEnvFile();
        _fixture.SetupDatabaseConfig();
        _fixture.SetupEnvStore();
        var sut = _fixture.CreateSut();

        // Act
        var result = await sut.LoadConnectionStringsAsync();

        // Assert
        result.Should().BeEquivalentTo(_fixture.ExpectedDbConnectionString);
    }

    private sealed class VaultServiceFixture
    {
        private static readonly TestBuilder<string> _stringBuilder = new();
        private readonly string _dbUsername = _stringBuilder.Create();
        private readonly string _dbPassword = _stringBuilder.Create();

        private readonly string _dbName = _stringBuilder.Create();
        private readonly string _dbAddress = _stringBuilder.Create();
        private readonly string _dbPort = _stringBuilder.Create();

        private readonly VaultCredentials _credentials = new TestBuilder<VaultCredentials>().Create();
        private readonly VaultConfig _config = new TestBuilder<VaultConfig>().Create();
        private readonly FileLoadingServiceMock _fileLoadingServiceMock = new(MockBehavior.Strict);

        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new(MockBehavior.Strict);
        private readonly MockHttpMessageHandler _handlerMock = new();
        private readonly string _baseAddress = $"https://sub{new TestBuilder<int>().Create()}domain.vault.com";
        private readonly string _token = _stringBuilder.Create();

        private ISecretStore? _secretStore;
        private readonly ConfigurationBuilder _configurationBuilder = new();

        public string ApiKey { get; } = _stringBuilder.Create();

        public ConnectionStrings ExpectedDbConnectionString => new()
        {
            ShoppingDatabase = $"server={_dbAddress};port={_dbPort};database={_dbName};user id={_dbUsername};pwd={_dbPassword};AllowUserVariables=true;UseAffectedRows=false"
        };

        public void SetupVaultStore()
        {
            _secretStore = new VaultService(_credentials, _config, _httpClientFactoryMock.Object);
        }

        public void SetupEnvStore()
        {
            _secretStore = new EnvSecretStore(_configurationBuilder.Build(), _fileLoadingServiceMock.Object);
        }

        public SecretLoadingService CreateSut()
        {
            TestPropertyNotSetException.ThrowIfNull(_secretStore);

            return new(_configurationBuilder.Build(), _secretStore);
        }

        public void SetupDbCredentialsInEnvFile()
        {
            var usernameFile = _stringBuilder.Create();
            var passwordFile = _stringBuilder.Create();

            List<KeyValuePair<string, string?>> collection =
            [
                new("PH_XIPONA_DB_USERNAME_FILE", usernameFile),
                new("PH_XIPONA_DB_PASSWORD_FILE", passwordFile)
            ];
            _configurationBuilder.AddInMemoryCollection(collection);

            _fileLoadingServiceMock.SetupReadFile(usernameFile, _dbUsername);
            _fileLoadingServiceMock.SetupReadFile(passwordFile, _dbPassword);
        }

        public void SetupDbCredentialsInEnv()
        {
            List<KeyValuePair<string, string?>> collection =
            [
                new("PH_XIPONA_DB_USERNAME", _dbUsername),
                new("PH_XIPONA_DB_PASSWORD", _dbPassword)
            ];
            _configurationBuilder.AddInMemoryCollection(collection);
        }

        public void SetupLoggingApiKeyInEnvFile()
        {
            var apiKeyFile = _stringBuilder.Create();

            List<KeyValuePair<string, string?>> collection =
            [
                new("PH_XIPONA_OTEL_API_KEY_FILE", apiKeyFile)
            ];
            _configurationBuilder.AddInMemoryCollection(collection);

            _fileLoadingServiceMock.SetupReadFile(apiKeyFile, ApiKey);
        }

        public void SetupLoggingApiKeyInEnv()
        {
            List<KeyValuePair<string, string?>> collection =
            [
                new("PH_XIPONA_OTEL_API_KEY", ApiKey)
            ];
            _configurationBuilder.AddInMemoryCollection(collection);
        }

        public void SetupDatabaseConfig()
        {
            List<KeyValuePair<string, string?>> collection =
            [
                new("Database:Name", _dbName),
                new("Database:Address", _dbAddress),
                new("Database:Port", _dbPort)
            ];

            _configurationBuilder.AddInMemoryCollection(collection);
        }

        public void SetupCreatingHttpClient()
        {

            _httpClientFactoryMock.Setup(x => x.CreateClient("vault"))
                .Returns(new HttpClient(_handlerMock)
                {
                    BaseAddress = new Uri(_baseAddress)
                });
        }

        public void SetupRetrievingDbCredentials()
        {
            _handlerMock.When($"{_baseAddress}/v1/{_config.MountPoint}/data/{_config.Paths.Database}")
                .With(msg =>
                {
                    var token = msg.Headers.GetValues("X-Vault-Token").First();
                    return token == _token && msg.Method == HttpMethod.Get;
                })
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    data = new
                    {
                        data = new
                        {
                            username = _dbUsername,
                            password = _dbPassword
                        }
                    }
                }));

        }

        public void SetupRetrievingApiKey()
        {
            _handlerMock.When($"{_baseAddress}/v1/{_config.MountPoint}/data/{_config.Paths.Logging}")
                .With(msg =>
                {
                    var token = msg.Headers.GetValues("X-Vault-Token").First();
                    return token == _token && msg.Method == HttpMethod.Get;
                })
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    data = new
                    {
                        data = new
                        {
                            apiKey = ApiKey
                        }
                    }
                }));
        }

        public void SetupRetrievingToken()
        {

            _handlerMock.When($"{_baseAddress}/v1/auth/userpass/login/{_credentials.Username}")
                .With(msg =>
                {
                    var content = msg.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                    var expectedContent = $$"""
                      {
                        "password": "{{_credentials.Password}}"
                      }
                      """
                        .Replace(Environment.NewLine, string.Empty)
                        .Replace(" ", string.Empty);

                    return content == expectedContent && msg.Method == HttpMethod.Post;
                })
                .Respond(HttpStatusCode.OK, JsonContent.Create(new
                {
                    auth = new
                    {
                        client_token = _token
                    }
                }));
        }
    }
}
