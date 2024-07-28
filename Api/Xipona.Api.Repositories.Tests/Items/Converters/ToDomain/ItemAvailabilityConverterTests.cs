using AutoMapper;
using ProjectHermes.Xipona.Api.Core.Tests.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Repositories.Items.Converters.ToDomain;
using ProjectHermes.Xipona.Api.Repositories.Items.Entities;
using ProjectHermes.Xipona.Api.Repositories.TestKit.Items.Entities;

namespace ProjectHermes.Xipona.Api.Repositories.Tests.Items.Converters.ToDomain;

public class ItemAvailabilityConverterTests : ToDomainConverterTestBase<AvailableAt, ItemAvailability, ItemAvailabilityConverter>
{
    public override ItemAvailabilityConverter CreateSut()
    {
        return new ItemAvailabilityConverter();
    }

    protected override AvailableAt CreateSource()
    {
        return new AvailableAtEntityBuilder().Create();
    }

    protected override void AddMapping(IMappingExpression<AvailableAt, ItemAvailability> mapping)
    {
        mapping
            .ForCtorParam(nameof(AvailableAt.StoreId), opt => opt.MapFrom(src => new StoreId(src.StoreId)))
            .ForCtorParam(nameof(AvailableAt.Price), opt => opt.MapFrom(src => new Price(src.Price)))
            .ForCtorParam(nameof(AvailableAt.DefaultSectionId),
                opt => opt.MapFrom(src => new SectionId(src.DefaultSectionId)));
    }
}