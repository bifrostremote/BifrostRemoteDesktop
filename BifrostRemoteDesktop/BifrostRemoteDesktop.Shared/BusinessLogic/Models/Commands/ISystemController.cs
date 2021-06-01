using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Models.Commands
{
    public interface ISystemController
    {
        bool SetPointerPosition(double x, double y);
        void SetPointerState();
    }
}
