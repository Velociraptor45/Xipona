using Mono.Cecil;

namespace ProjectHermes.ShoppingList.Api.ArchitectureTests.ApplicationServices.CustomRules;

internal abstract class ApplicationServicesCustomRuleBase : ICustomRule
{
    protected Assembly Assembly { get; } = Assembly.Load(new AssemblyName("ShoppingList.Api.ApplicationServices"));

    public abstract bool MeetsRule(TypeDefinition type);
}