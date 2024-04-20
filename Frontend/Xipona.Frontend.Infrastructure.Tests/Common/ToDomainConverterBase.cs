using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.Xipona.Frontend.TestTools;
using ProjectHermes.Xipona.Frontend.TestTools.Extensions;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Common;

public abstract class ToDomainConverterBase<TSource, TDest, TConverter> where TConverter : IToDomainConverter<TSource, TDest>
{
    protected bool ExcludingKey { get; set; } = true;

    [Fact]
    public void Convert_ShouldMapAllMembersCorrectly()
    {
        // Arrange
        var builder = new TestBuilder<TSource>();
        Customize(builder);
        var contract = builder.Create();

        var config = new MapperConfiguration(cfg =>
        {
            AddMapping(cfg);
            AddAdditionalMapping(cfg);
        });
        var mapper = config.CreateMapper();

        mapper.ConfigurationProvider.AssertConfigurationIsValid();

        var expectedResult = mapper.Map<TDest>(contract);
        var sut = CreateSut();

        // Act
        var result = sut.ToDomain(contract);

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt =>
        {
            if (ExcludingKey)
                opt.ExcludingKey();

            return opt;
        });
    }

    protected abstract TConverter CreateSut();

    protected virtual void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
    }

    protected abstract void AddMapping(IMappingExpression<TSource, TDest> mapping);

    protected virtual void Customize(IFixture fixture)
    {
    }

    public void AddMapping(IMapperConfigurationExpression cfg)
    {
        var mapping = cfg.CreateMap<TSource, TDest>(MemberList.Destination);
        AddMapping(mapping);
    }
}