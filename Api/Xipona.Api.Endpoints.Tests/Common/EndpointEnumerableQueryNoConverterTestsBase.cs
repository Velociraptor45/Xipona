﻿using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;

namespace ProjectHermes.Xipona.Api.Endpoints.Tests.Common;
public abstract class EndpointEnumerableQueryNoConverterTestsBase<TQuery, TQueryReturnType, TReturnType, TFixture>
    : EndpointEnumerableQueryTestsBase<bool, TQuery, TQueryReturnType, TReturnType, TFixture>
    where TQuery : IQuery<IEnumerable<TQueryReturnType>>
    where TFixture : EndpointEnumerableQueryNoConverterTestsBase<TQuery, TQueryReturnType, TReturnType, TFixture>
        .EndpointEnumerableQueryNoConverterFixtureBase
{
    protected EndpointEnumerableQueryNoConverterTestsBase(TFixture fixture) : base(fixture)
    {
    }

    public abstract class EndpointEnumerableQueryNoConverterFixtureBase :
        EndpointEnumerableQueryTestsBase<bool, TQuery, TQueryReturnType, TReturnType, TFixture>.EndpointEnumerableQueryFixtureBase
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
