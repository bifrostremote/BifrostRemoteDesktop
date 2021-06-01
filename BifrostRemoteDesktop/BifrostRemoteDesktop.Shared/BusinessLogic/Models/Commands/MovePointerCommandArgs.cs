using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Models.Commands
{
    public class MovePointerCommandArgs : RemoteControlCommandArgs
    {
        public double TargetX { get; set; }
        public double TargetY { get; set; }
    }
}
