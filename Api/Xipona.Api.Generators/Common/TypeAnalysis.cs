using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;
using Xipona.Api.Generators.Extensions;

namespace Xipona.Api.Generators.Common;

public readonly record struct TypeAnalysis
{
    public TypeAnalysis(TypeSyntax type, GeneratorSyntaxContext ctx)
    {
        TypeName = type.GetName(ctx);
        TypeNamespace = type.GetNamespace(ctx);
        GenericArguments = type is TupleTypeSyntax tuple
            ? tuple.Elements.Select(el => new TypeAnalysis(el.Type, ctx)).ToList()
            : type.GetGenericTypeArguments(ctx).ToList();
        IsTuple = type is TupleTypeSyntax;

        var genericStart = TypeName.IndexOf('<');

        NonGenericTypeName = genericStart == -1
            ? TypeName
            : TypeName.Substring(0, genericStart);
    }


    public string TypeName { get; }
    public string NonGenericTypeName { get; }
    public string TypeNamespace { get; }
    public IReadOnlyCollection<TypeAnalysis> GenericArguments { get; }
    public bool IsTuple { get; }

    public IEnumerable<string> GetAllNamespaces()
    {
        return new[] { TypeNamespace }
            .Concat(GenericArguments.SelectMany(arg => arg.GetAllNamespaces()));
    }

    public override string ToString()
    {
        if (IsTuple)
        {
            var tupleBuilder = new StringBuilder();
            tupleBuilder.Append("(");
            for (int i = 0; i < GenericArguments.Count; i++)
            {
                var arg = GenericArguments.ElementAt(i);
                tupleBuilder.Append(arg.ToString());
                if (i < GenericArguments.Count - 1)
                    tupleBuilder.Append(", ");
            }

            tupleBuilder.Append(")");

            return tupleBuilder.ToString();
        }

        if (TypeNamespace == "System")
            return TypeName;

        if (GenericArguments.Count != 0)
        {
            var genericBuilder = new StringBuilder();
            genericBuilder.Append(TypeNamespace);
            genericBuilder.Append('.');
            genericBuilder.Append(NonGenericTypeName);
            genericBuilder.Append('<');

            for (int i = 0; i < GenericArguments.Count; i++)
            {
                var arg = GenericArguments.ElementAt(i);
                genericBuilder.Append(arg.ToString());
                if (i < GenericArguments.Count - 1)
                    genericBuilder.Append(", ");
            }

            genericBuilder.Append('>');

            return genericBuilder.ToString();
        }

        return $"{TypeNamespace}.{TypeName}";
    }
}