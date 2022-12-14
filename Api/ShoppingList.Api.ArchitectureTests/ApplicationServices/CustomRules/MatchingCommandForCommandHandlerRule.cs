using Mono.Cecil;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;

namespace ProjectHermes.ShoppingList.Api.ArchitectureTests.ApplicationServices.CustomRules;

internal sealed class MatchingCommandForCommandHandlerRule : ApplicationServicesCustomRuleBase
{
    public override bool MeetsRule(TypeDefinition type)
    {
        var commandTypeName = ((GenericInstanceType)type.Interfaces.First().InterfaceType).GenericArguments.First()
            .FullName;

        var foundTypes = Assembly
            .GetTypes()
            .Where(t =>
                t.FullName == commandTypeName
                && t.GetInterfaces().Any(i =>
                    i.IsGenericType
                    && i.GetGenericTypeDefinition() == typeof(ICommand<>)))
            .ToArray();

        return foundTypes.Length == 1;
    }
}