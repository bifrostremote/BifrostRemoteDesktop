using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Controllers
{
    public partial class PlatformPointerProvider : IPlatformPointerProvider
    {
        public IPointerController GetPointerController()
        {
            return this.GetPlatformPointerController();
        }
    }
}
