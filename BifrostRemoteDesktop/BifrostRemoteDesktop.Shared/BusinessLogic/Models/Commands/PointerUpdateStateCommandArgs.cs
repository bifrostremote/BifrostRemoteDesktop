using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Models.Commands
{
    public class PointerUpdateStateCommandArgs : RemoteControlCommandArgs
    {
        public PointerUpdateStateCommandArgs() { }

        public bool IsLeftPointerButtonPressed { get; set; }
        public bool IsRightPointerButtonPressed { get; set; }
        public bool IsMiddlePointerButtonPressed { get; set; }
    }
}
