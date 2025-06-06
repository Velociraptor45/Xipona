using Xipona.Api.Core.TestKit;

namespace Xipona.Api.Domain.TestKit.Common;

public abstract class DomainTestBuilderBase<TModel>() : TestBuilderBase<TModel>(new DomainCustomization());