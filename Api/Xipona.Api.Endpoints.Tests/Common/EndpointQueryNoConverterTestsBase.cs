using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
public class EndpointQueryNoConverterTestsBase<TQuery, TQueryReturnType, TReturnType, TFixture>
    : EndpointQueryTestsBase<bool, TQuery, TQueryReturnType, TReturnType, TFixture>
    where TQuery : IQuery<TQueryReturnType>
    where TFixture : EndpointQueryNoConverterTestsBase<TQuery, TQueryReturnType, TReturnType, TFixture>
        .EndpointQueryNoConverterFixtureBase

{
    public EndpointQueryNoConverterTestsBase(TFixture fixture) : base(fixture)
    {
    }


    public abstract class EndpointQueryNoConverterFixtureBase : EndpointQueryFixtureBase
    {
        public override void SetupQueryConverter()
        {
        }

        public override bool GetQueryConverterInput()
        {
            return false;
        }
    }
}
