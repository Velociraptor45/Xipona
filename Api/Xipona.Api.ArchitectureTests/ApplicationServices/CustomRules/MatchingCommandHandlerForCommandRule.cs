using Mono.Cecil;
using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;

namespace ProjectHermes.Xipona.Api.ArchitectureTests.ApplicationServices.CustomRules;

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