using Mono.Cecil;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;

namespace ProjectHermes.ShoppingList.Api.ArchitectureTests.ApplicationServices.CustomRules;

internal sealed class MatchingCommandHandlerForCommandRule : ApplicationServicesCustomRuleBase
{
    public override bool MeetsRule(TypeDefinition type)
    {
        var foundTypes = Assembly
            .GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType
                && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)
                && i.GetGenericArguments().First().FullName == type.FullName))
            .ToArray();

        return foundTypes.Length == 1;
    }
}