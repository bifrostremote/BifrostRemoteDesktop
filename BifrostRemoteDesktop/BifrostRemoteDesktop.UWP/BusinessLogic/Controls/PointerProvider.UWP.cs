using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Controls
{

    public partial class PointerProvider : IPlatformPointerProvider
    {
        public IPointerController GetPlatformPointerController()
        {
            return new UWPPointerController();
        }
    }
}
