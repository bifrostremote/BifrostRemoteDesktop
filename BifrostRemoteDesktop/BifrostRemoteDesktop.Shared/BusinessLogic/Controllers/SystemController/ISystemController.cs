using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Controllers
{
    public interface ISystemController
    {
        bool SetPointerPosition(double x, double y);
        void SetPointerState();
    }
}
