using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Controls
{

    public partial class MouseProvider : IMouseProvider
    {
        public IMouseController GetPlatformMouseController()
        {
            throw new NotImplementedException("This feature is not implemented on the current platform.");
        }
    }
}
