using Xipona.Api.Core.TestKit;

namespace Xipona.Api.Contracts.TestKit.Common;

public class ContractTestBuilder<TModel>()
    : TestBuilder<TModel, ContractTestBuilder<TModel>>(new ContractCustomization());