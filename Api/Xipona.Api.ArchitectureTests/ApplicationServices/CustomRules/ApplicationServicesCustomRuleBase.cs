using Mono.Cecil;

namespace ProjectHermes.Xipona.Api.ArchitectureTests.ApplicationServices.CustomRules;

internal abstract class ApplicationServicesCustomRuleBase : ICustomRule
{
    protected Assembly Assembly { get; } = Assembly.Load(new AssemblyName("Xipona.Api.ApplicationServices"));

    public abstract bool MeetsRule(TypeDefinition type);
}