using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Controllers
{
    public interface IPlatformPointerProvider

    {
        IPointerController GetPlatformPointerController();
    }
}
