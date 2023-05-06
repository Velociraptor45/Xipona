using ProjectHermes.ShoppingList.Api.Core.Converter;

namespace ProjectHermes.ShoppingList.Api.Core.TestKit.Common;

public abstract class ToDomainConverterBase<TSource, TDest, TConverter> where TConverter : IToDomainConverter<TSource, TDest>
{
    [Fact]
    public void Convert_ShouldMapAllMembersCorrectly()
    {
        // Arrange
        var builder = new TestBuilder<TSource>();
        Customize(builder);
        var contract = builder.Create();

        var mapper = new MapperConfiguration(cfg =>
        {
            AddMapping(cfg);
            AddAdditionalMapping(cfg);
        }).CreateMapper();

        mapper.ConfigurationProvider.AssertConfigurationIsValid();

        var expectedResult = mapper.Map<TDest>(contract);
        var sut = CreateSut();

        // Act
        var result = sut.ToDomain(contract);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
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