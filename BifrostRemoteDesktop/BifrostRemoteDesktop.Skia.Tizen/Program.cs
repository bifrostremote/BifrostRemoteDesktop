using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace BifrostRemoteDesktop.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new BifrostRemoteDesktop.App(), args);
            host.Run();
        }
    }
}
