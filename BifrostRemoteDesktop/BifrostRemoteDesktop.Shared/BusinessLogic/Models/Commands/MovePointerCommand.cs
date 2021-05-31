using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Models.Commands
{

    public class MovePointerCommand : RemoteControlCommand
    {
        public MovePointerCommand(ISystemController systemController, double x, double y) : base(systemController)
        {
            _x = x;
            _y = y;
        }

        private readonly double _x;
        private readonly double _y;

        public override void Execute()
        {
            _systemController.SetPointerPosition(_x,_y);
        }
    }
}
