using Moq;
using ProjectHermes.Xipona.Api.Endpoint.v1.Converters;

namespace ProjectHermes.Xipona.Api.Endpoints.TestKit.v1.Converters;

public class EndpointConvertersMock : Mock<IEndpointConverters>
{
    public EndpointConvertersMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupToContract<TDomain, TContract>(TDomain domain, TContract returnValue)
    {
        Setup(m => m.ToContract<TDomain, TContract>(domain))
            .Returns(returnValue);
    }

    public void SetupToContract<TDomain, TContract>(IEnumerable<TDomain> domain, IEnumerable<TContract> returnValue)
    {
        Setup(m => m.ToContract<TDomain, TContract>(domain))
            .Returns(returnValue);
    }

    public void SetupToDomain<TContract, TDomain>(TContract contract, TDomain returnValue)
    {
        Setup(m => m.ToDomain<TContract, TDomain>(contract))
            .Returns(returnValue);
    }

    public void SetupToDomain<TContract, TDomain>(IEnumerable<TContract> contract, IEnumerable<TDomain> returnValue)
    {
        Setup(m => m.ToDomain<TContract, TDomain>(contract))
            .Returns(returnValue);
    }
}