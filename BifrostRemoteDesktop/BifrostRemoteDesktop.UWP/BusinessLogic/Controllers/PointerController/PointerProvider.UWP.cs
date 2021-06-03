using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Controllers
{

    public partial class PlatformPointerProvider : IPlatformPointerProvider
    {
        public IPointerController GetPlatformPointerController()
        {
            return new UWPPointerController();
        }
    }
}
