using ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.Xipona.Frontend.Infrastructure.Converters.Items.ToContract;
using ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Common;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.TestTools;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Tests.Converters.Items.ToContract;

public class ModifyItemTypeContractConverterTests
{
    public class WithIdFilled :
        ToContractConverterBase<EditedItemType, ModifyItemTypeContract, ModifyItemTypeContractConverter>
    {
        protected override EditedItemType CreateSource()
        {
            return new TestBuilder<EditedItemType>().Create() with { Id = Guid.NewGuid() };
        }

        protected override ModifyItemTypeContractConverter CreateSut()
        {
            return new(new ItemAvailabilityContractConverter());
        }

        protected override void AddMapping(IMappingExpression<EditedItemType, ModifyItemTypeContract> mapping)
        {
            mapping
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Availabilities, opt => opt.MapFrom(src => src.Availabilities));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityContractConverterTests().AddMapping(cfg);
        }
    }

    public class WithIdEmpty :
        ToContractConverterBase<EditedItemType, ModifyItemTypeContract, ModifyItemTypeContractConverter>
    {
        protected override EditedItemType CreateSource()
        {
            return new TestBuilder<EditedItemType>().Create() with { Id = Guid.Empty };
        }

        protected override ModifyItemTypeContractConverter CreateSut()
        {
            return new(new ItemAvailabilityContractConverter());
        }

        protected override void AddMapping(IMappingExpression<EditedItemType, ModifyItemTypeContract> mapping)
        {
            mapping
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (Guid?)null))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Availabilities, opt => opt.MapFrom(src => src.Availabilities));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityContractConverterTests().AddMapping(cfg);
        }
    }
}