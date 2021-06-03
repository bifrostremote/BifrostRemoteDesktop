using BifrostRemoteDesktop.BusinessLogic.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Models.Commands
{

    public class MovePointerCommand : RemoteControlCommand<MovePointerCommandArgs>
    {
        public MovePointerCommand(ISystemController systemController, MovePointerCommandArgs args) : base(systemController, args) { }

        public override void Execute()
        {
            SystemController.SetPointerPosition(
                x: Args.TargetX,
                y: Args.TargetY);
        }
    }
}
