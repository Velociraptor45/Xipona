using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using ProjectHermes.Xipona.Api.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Api.Domain.Tests.Common.Models;

public class AggregateRootTests
{
    public class EnrichWithRowVersion
    {
        private readonly EnrichWithRowVersionFixture _fixture;

        public EnrichWithRowVersion()
        {
            _fixture = new EnrichWithRowVersionFixture();
        }

        [Fact]
        public void EnrichWithRowVersion_WithoutExistingRowVersion_ShouldSetRowVersion()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupRowVersion();

            TestPropertyNotSetException.ThrowIfNull(_fixture.RowVersion);

            // Act
            sut.EnrichWithRowVersion(_fixture.RowVersion.Value);

            // Assert
            sut.RowVersion.Should().Be(_fixture.RowVersion.Value);
        }

        [Fact]
        public void EnrichWithRowVersion_WithExistingRowVersion_ShouldThrowException()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupRowVersion();

            TestPropertyNotSetException.ThrowIfNull(_fixture.RowVersion);

            sut.EnrichWithRowVersion(_fixture.RowVersion.Value);

            // Act
            Action act = () => sut.EnrichWithRowVersion(_fixture.RowVersion.Value);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        private sealed class EnrichWithRowVersionFixture
        {
            public uint? RowVersion { get; private set; }

            public void SetupRowVersion()
            {
                RowVersion = new DomainTestBuilder<uint>().Create();
            }

            public TestAggregateRoot CreateSut()
            {
                return new TestAggregateRoot();
            }
        }
    }

    private class TestAggregateRoot : AggregateRoot
    {
    }
}