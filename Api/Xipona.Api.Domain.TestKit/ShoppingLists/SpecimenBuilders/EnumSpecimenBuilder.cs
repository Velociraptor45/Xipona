using AutoFixture.Kernel;
using ProjectHermes.Xipona.Api.Domain.TestKit.Shared;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.SpecimenBuilders;

public class EnumSpecimenBuilder<TEnum> : ISpecimenBuilder
    where TEnum : Enum
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is TEnum || (request is SeededRequest sr && (Type)sr.Request == typeof(TEnum)))
        {
            var values = ((TEnum[])Enum.GetValues(typeof(TEnum)))
                .ToList();

            return CommonFixture.ChooseRandom(values);
        }

        return new NoSpecimen();
    }
}