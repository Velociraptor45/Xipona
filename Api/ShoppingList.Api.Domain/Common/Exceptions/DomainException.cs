using ProjectHermes.ShoppingList.Api.Domain.Common.Reason;
using System.Runtime.Serialization;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;

[Serializable]
public class DomainException : Exception
{
    public DomainException(IReason reason)
    {
        Reason = reason;
    }

    protected DomainException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        Reason = (IReason)info.GetValue("Reason", typeof(IReason))!;
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("Reason", Reason);
    }

    public IReason Reason { get; }
}