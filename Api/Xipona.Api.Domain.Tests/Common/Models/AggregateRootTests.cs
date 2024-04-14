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
            sut.EnrichWithRowVersion(_fixture.RowVersion);

            // Assert
            sut.RowVersion.Should().BeEquivalentTo(_fixture.RowVersion);
        }

        [Fact]
        public void EnrichWithRowVersion_WithExistingRowVersion_ShouldThrowException()
        {
            // Arrange
            var sut = _fixture.CreateSut();
            _fixture.SetupRowVersion();

            TestPropertyNotSetException.ThrowIfNull(_fixture.RowVersion);

            sut.EnrichWithRowVersion(_fixture.RowVersion);

            // Act
            Action act = () => sut.EnrichWithRowVersion(_fixture.RowVersion);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        private sealed class EnrichWithRowVersionFixture
        {
            public byte[]? RowVersion { get; private set; }

            public void SetupRowVersion()
            {
                RowVersion = new DomainTestBuilder<byte[]>().Create();
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