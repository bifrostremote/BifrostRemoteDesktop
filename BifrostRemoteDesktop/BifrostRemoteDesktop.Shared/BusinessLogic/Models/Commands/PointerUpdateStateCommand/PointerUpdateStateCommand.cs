using BifrostRemoteDesktop.BusinessLogic.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Models.Commands
{
    public class PointerUpdateStateCommand : RemoteControlCommand<PointerUpdateStateCommandArgs>
    {

        public PointerUpdateStateCommand(
            ISystemController systemController, PointerUpdateStateCommandArgs args) : base(systemController, args)
        {

        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
