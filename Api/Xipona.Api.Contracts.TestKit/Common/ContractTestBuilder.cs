using ProjectHermes.Xipona.Api.Core.TestKit;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Common;

public class ContractTestBuilder<TModel> : TestBuilder<TModel, ContractTestBuilder<TModel>>
{
    public ContractTestBuilder() : base(new ContractCustomization())
    {
    }
}