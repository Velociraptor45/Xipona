using ProjectHermes.Xipona.Api.Core.TestKit;

namespace ProjectHermes.Xipona.Api.Contracts.TestKit.Common;

public class ContractTestBuilderBase<TModel>() : TestBuilderBase<TModel>(new ContractCustomization());