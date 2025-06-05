using Xipona.Api.Core.Constants;
using System.Diagnostics;

namespace Xipona.Api.Endpoint;
public static class Diagnostics
{
    public static readonly ActivitySource Instance = new(Application.ActivitySourceName);

    public static void DisposeInstance()
    {
        Instance.Dispose();
    }
}
