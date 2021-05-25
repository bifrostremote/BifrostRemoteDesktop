using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Controls
{

    public partial interface IMouseProvider
    {
        IMouseController GetMouseController();
    }

    public partial class MouseProvider : IMouseProvider
    {
        public IMouseController GetMouseController()
        {
            this.GetPlatformMouseController();
            throw new NotImplementedException("This feature is not implemented on the current platform.");
        }
    }
}
